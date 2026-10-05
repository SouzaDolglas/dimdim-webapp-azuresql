using DimDim.Web.Data;
using DimDim.Web.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DimDim.Web.Pages.Clientes;

public class IndexModel(DimDimContext db) : PageModel
{
    public List<Cliente> Clientes { get; private set; } = new();

    public async Task OnGetAsync() =>
        Clientes = await db.Clientes.Include(c => c.Contas).OrderBy(c => c.Id).ToListAsync();
}
