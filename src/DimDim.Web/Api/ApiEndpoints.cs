using DimDim.Web.Data;
using DimDim.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace DimDim.Web.Api;

// API REST para as mesmas tabelas que o front-end usa (JSON de exemplo em /json).
public static class ApiEndpoints
{
    public static void MapDimDimApi(this WebApplication app)
    {
        var cli = app.MapGroup("/api/clientes");
        cli.MapGet("/", async (DimDimContext db) => await db.Clientes.AsNoTracking().ToListAsync());
        cli.MapGet("/{id:int}", async (int id, DimDimContext db) =>
            await db.Clientes.FindAsync(id) is { } c ? Results.Ok(c) : Results.NotFound());
        cli.MapPost("/", async (Cliente c, DimDimContext db) =>
        {
            c.Id = 0; c.DataCadastro = DateTime.UtcNow;
            db.Clientes.Add(c); await db.SaveChangesAsync();
            return Results.Created($"/api/clientes/{c.Id}", c);
        });
        cli.MapPut("/{id:int}", async (int id, Cliente input, DimDimContext db) =>
        {
            var c = await db.Clientes.FindAsync(id);
            if (c is null) return Results.NotFound();
            c.Nome = input.Nome; c.Cpf = input.Cpf; c.Email = input.Email;
            await db.SaveChangesAsync();
            return Results.Ok(c);
        });
        cli.MapDelete("/{id:int}", async (int id, DimDimContext db) =>
        {
            var c = await db.Clientes.FindAsync(id);
            if (c is null) return Results.NotFound();
            db.Clientes.Remove(c); await db.SaveChangesAsync();
            return Results.NoContent();
        });

        var ct = app.MapGroup("/api/contas");
        ct.MapGet("/", async (DimDimContext db) => await db.Contas.AsNoTracking().ToListAsync());
        ct.MapGet("/{id:int}", async (int id, DimDimContext db) =>
            await db.Contas.FindAsync(id) is { } c ? Results.Ok(c) : Results.NotFound());
        ct.MapPost("/", async (Conta c, DimDimContext db) =>
        {
            if (!await db.Clientes.AnyAsync(x => x.Id == c.ClienteId))
                return Results.BadRequest(new { erro = "cliente_id inexistente" });
            c.Id = 0; c.DataAbertura = DateTime.UtcNow;
            db.Contas.Add(c); await db.SaveChangesAsync();
            return Results.Created($"/api/contas/{c.Id}", c);
        });
        ct.MapPut("/{id:int}", async (int id, Conta input, DimDimContext db) =>
        {
            var c = await db.Contas.FindAsync(id);
            if (c is null) return Results.NotFound();
            c.Numero = input.Numero; c.Tipo = input.Tipo; c.Saldo = input.Saldo; c.ClienteId = input.ClienteId;
            await db.SaveChangesAsync();
            return Results.Ok(c);
        });
        ct.MapDelete("/{id:int}", async (int id, DimDimContext db) =>
        {
            var c = await db.Contas.FindAsync(id);
            if (c is null) return Results.NotFound();
            db.Contas.Remove(c); await db.SaveChangesAsync();
            return Results.NoContent();
        });
    }
}
