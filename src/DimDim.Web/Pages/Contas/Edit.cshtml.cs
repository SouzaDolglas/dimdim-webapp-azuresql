using DimDim.Web.Data;
using DimDim.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace DimDim.Web.Pages.Contas;

public class EditModel(DimDimContext db) : ContaPage(db)
{
    [BindProperty] public Conta Conta { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var c = await Db.Contas.FindAsync(id);
        if (c is null) return NotFound();
        Conta = c;
        await CarregarClientesAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (!ModelState.IsValid) { await CarregarClientesAsync(); return Page(); }
        var c = await Db.Contas.FindAsync(id);
        if (c is null) return NotFound();
        c.Numero = Conta.Numero; c.Tipo = Conta.Tipo; c.Saldo = Conta.Saldo; c.ClienteId = Conta.ClienteId;
        await Db.SaveChangesAsync();
        TempData["msg"] = $"Conta {c.Id} atualizada.";
        return RedirectToPage("Index");
    }
}
