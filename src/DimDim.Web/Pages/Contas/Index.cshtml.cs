using DimDim.Web.Data;
using DimDim.Web.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DimDim.Web.Pages.Contas;

public class IndexModel(DimDimContext db) : PageModel
{
    public List<Conta> Contas { get; private set; } = new();

    public async Task OnGetAsync() =>
        Contas = await db.Contas.Include(c => c.Cliente).OrderBy(c => c.Id).ToListAsync();
}
