using DimDim.Web.Data;
using DimDim.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DimDim.Web.Pages.Contas;

public class DeleteModel(DimDimContext db) : PageModel
{
    public Conta Conta { get; private set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var c = await db.Contas.Include(x => x.Cliente).FirstOrDefaultAsync(x => x.Id == id);
        if (c is null) return NotFound();
        Conta = c;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var c = await db.Contas.FindAsync(id);
        if (c is null) return NotFound();
        db.Contas.Remove(c);
        await db.SaveChangesAsync();
        TempData["msg"] = $"Conta {id} excluída.";
        return RedirectToPage("Index");
    }
}
