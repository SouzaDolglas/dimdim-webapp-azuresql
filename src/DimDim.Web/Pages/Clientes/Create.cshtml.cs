using DimDim.Web.Data;
using DimDim.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DimDim.Web.Pages.Clientes;

public class CreateModel(DimDimContext db) : PageModel
{
    [BindProperty] public Cliente Cliente { get; set; } = new();

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        Cliente.DataCadastro = DateTime.UtcNow;
        db.Clientes.Add(Cliente);
        await db.SaveChangesAsync();
        TempData["msg"] = $"Cliente {Cliente.Nome} criado (id {Cliente.Id}).";
        return RedirectToPage("Index");
    }
}
