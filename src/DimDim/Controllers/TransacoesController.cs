using DimDim.Data;
using DimDim.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
namespace DimDim.Controllers;

public class TransacoesController : Controller
{
    private readonly AppDb _db;
    public TransacoesController(AppDb db) => _db = db;

    private async Task CarregarClientes(int? selecionado = null) =>
        ViewBag.Clientes = new SelectList(await _db.Clientes.AsNoTracking().OrderBy(c => c.Nome).ToListAsync(), "Id", "Nome", selecionado);

    public async Task<IActionResult> Index() =>
        View(await _db.Transacoes.Include(t => t.Cliente).AsNoTracking().OrderByDescending(t => t.Id).ToListAsync());

    public async Task<IActionResult> Create()
    {
        await CarregarClientes();
        return View("Form", new Transacao());
    }

    [HttpPost]
    public async Task<IActionResult> Create(Transacao t)
    {
        ModelState.Remove(nameof(Transacao.Cliente));
        if (!ModelState.IsValid) { await CarregarClientes(t.ClienteId); return View("Form", t); }
        t.DataTransacao = DateTime.Now;
        _db.Add(t);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var t = await _db.Transacoes.FindAsync(id);
        if (t == null) return NotFound();
        await CarregarClientes(t.ClienteId);
        return View("Form", t);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, Transacao t)
    {
        ModelState.Remove(nameof(Transacao.Cliente));
        if (!ModelState.IsValid) { await CarregarClientes(t.ClienteId); return View("Form", t); }
        var atual = await _db.Transacoes.FindAsync(id);
        if (atual == null) return NotFound();
        atual.ClienteId = t.ClienteId;
        atual.Valor = t.Valor;
        atual.Descricao = t.Descricao;
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var t = await _db.Transacoes.FindAsync(id);
        if (t != null) { _db.Remove(t); await _db.SaveChangesAsync(); }
        return RedirectToAction(nameof(Index));
    }
}
