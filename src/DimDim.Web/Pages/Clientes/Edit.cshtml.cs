using DimDim.Web.Data;
using DimDim.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DimDim.Web.Pages.Clientes;

public class EditModel(DimDimContext db) : PageModel
{
    [BindProperty] public Cliente Cliente { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var c = await db.Clientes.FindAsync(id);
        if (c is null) return NotFound();
        Cliente = c;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (!ModelState.IsValid) return Page();
        var c = await db.Clientes.FindAsync(id);
        if (c is null) return NotFound();
        c.Nome = Cliente.Nome; c.Cpf = Cliente.Cpf; c.Email = Cliente.Email;
        await db.SaveChangesAsync();
        TempData["msg"] = $"Cliente {c.Id} atualizado.";
        return RedirectToPage("Index");
    }
}
