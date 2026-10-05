using DimDim.Web.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DimDim.Web.Pages.Contas;

// Base dos formulários de conta: carrega o combo de clientes (a FK da tabela conta)
public abstract class ContaPage(DimDimContext db) : PageModel
{
    protected DimDimContext Db => db;

    protected async Task CarregarClientesAsync() =>
        ViewData["Clientes"] = new SelectList(
            await db.Clientes.OrderBy(c => c.Nome).Select(c => new { c.Id, Rotulo = c.Id + " — " + c.Nome }).ToListAsync(),
            "Id", "Rotulo");
}
