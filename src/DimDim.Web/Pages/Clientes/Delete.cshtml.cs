using DimDim.Web.Data;
using DimDim.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DimDim.Web.Pages.Clientes;

public class DeleteModel(DimDimContext db) : PageModel
{
    public Cliente Cliente { get; private set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var c = await db.Clientes.Include(x => x.Contas).FirstOrDefaultAsync(x => x.Id == id);
        if (c is null) return NotFound();
        Cliente = c;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var c = await db.Clientes.FindAsync(id);
        if (c is null) return NotFound();
        db.Clientes.Remove(c);
        await db.SaveChangesAsync();
        TempData["msg"] = $"Cliente {id} excluído.";
        return RedirectToPage("Index");
    }
}
