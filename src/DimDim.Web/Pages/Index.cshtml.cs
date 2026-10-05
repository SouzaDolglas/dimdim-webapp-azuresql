using DimDim.Web.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DimDim.Web.Pages;

public class IndexModel(DimDimContext db) : PageModel
{
    public int Clientes { get; private set; }
    public int Contas { get; private set; }
    public decimal SaldoTotal { get; private set; }

    public async Task OnGetAsync()
    {
        Clientes = await db.Clientes.CountAsync();
        Contas = await db.Contas.CountAsync();
        SaldoTotal = await db.Contas.SumAsync(c => (decimal?)c.Saldo) ?? 0;
    }
}
