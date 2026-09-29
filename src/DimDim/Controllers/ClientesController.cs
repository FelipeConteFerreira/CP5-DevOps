using DimDim.Data;
using DimDim.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace DimDim.Controllers;

public class ClientesController : Controller
{
    private readonly AppDb _db;
    public ClientesController(AppDb db) => _db = db;

    public async Task<IActionResult> Index() =>
        View(await _db.Clientes.Include(c => c.Transacoes).AsNoTracking().OrderBy(c => c.Id).ToListAsync());

    public IActionResult Create() => View("Form", new Cliente());

    [HttpPost]
    public async Task<IActionResult> Create(Cliente c)
    {
        ModelState.Remove(nameof(Cliente.Transacoes));
        if (!ModelState.IsValid) return View("Form", c);
        c.DataCadastro = DateTime.Now;
        _db.Add(c);
        try { await _db.SaveChangesAsync(); }
        catch (DbUpdateException) { ModelState.AddModelError("Email", "Este e-mail já está cadastrado."); return View("Form", c); }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var c = await _db.Clientes.FindAsync(id);
        return c == null ? NotFound() : View("Form", c);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, Cliente c)
    {
        ModelState.Remove(nameof(Cliente.Transacoes));
        if (!ModelState.IsValid) return View("Form", c);
        var atual = await _db.Clientes.FindAsync(id);
        if (atual == null) return NotFound();
        atual.Nome = c.Nome;
        atual.Email = c.Email;
        try { await _db.SaveChangesAsync(); }
        catch (DbUpdateException) { ModelState.AddModelError("Email", "Este e-mail já está cadastrado."); return View("Form", c); }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var c = await _db.Clientes.FindAsync(id);
        if (c != null) { _db.Remove(c); await _db.SaveChangesAsync(); }
        return RedirectToAction(nameof(Index));
    }
}
