using DimDim.Web.Data;
using DimDim.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace DimDim.Web.Pages.Contas;

public class CreateModel(DimDimContext db) : ContaPage(db)
{
    [BindProperty] public Conta Conta { get; set; } = new();

    public async Task OnGetAsync() => await CarregarClientesAsync();

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) { await CarregarClientesAsync(); return Page(); }
        Conta.DataAbertura = DateTime.UtcNow;
        Db.Contas.Add(Conta);
        await Db.SaveChangesAsync();
        TempData["msg"] = $"Conta {Conta.Numero} criada (id {Conta.Id}).";
        return RedirectToPage("Index");
    }
}
