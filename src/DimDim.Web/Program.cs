using DimDim.Web.Api;
using DimDim.Web.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Application Insights: lê APPLICATIONINSIGHTS_CONNECTION_STRING das configurações do Web App
// (rodando local sem essa variável, a aplicação sobe sem telemetria)
if (!string.IsNullOrEmpty(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]))
    builder.Services.AddApplicationInsightsTelemetry();

// String de conexão vem da configuração do Web App (ConnectionStrings:DimDim), nunca do código
builder.Services.AddDbContext<DimDimContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("DimDim"),
        sql => sql.EnableRetryOnFailure()));

builder.Services.AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();
app.MapDimDimApi();

app.Run();
