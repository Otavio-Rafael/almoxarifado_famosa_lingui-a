using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Moxarifi.Data;
using Moxarifi.Models;
using X.PagedList.Extensions;

namespace Moxarifi.Controllers
{
    public class MovimentacaoEstoquesController : Controller
    {
        private readonly MoxarifiContext _context;

        public MovimentacaoEstoquesController(MoxarifiContext context)
        {
            _context = context;
        }

        // GET: MovimentacaoEstoques
        public async Task<IActionResult> Index(int? pagina)
        {
            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;

            var lista = await _context.MovimentacaoEstoque
                                .Include(m => m.Produto)
                                .ToListAsync();

            return View(lista.ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: MovimentacaoEstoques/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movimentacaoEstoque = await _context.MovimentacaoEstoque
                .Include(m => m.Produto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movimentacaoEstoque == null)
            {
                return NotFound();
            }

            return View(movimentacaoEstoque);
        }

        // GET: MovimentacaoEstoques/Create
        public IActionResult Create()
        {
            ViewData["ProdutoId"] = new SelectList(_context.Produto, "Id", "Nome");
            return View();
        }

        // POST: MovimentacaoEstoques/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Quantidade,ProdutoId,Data,TipoMovi")] MovimentacaoEstoque movimentacaoEstoque)
        {
            if (ModelState.IsValid)
            {
                var produto = await _context.Produto.FindAsync(movimentacaoEstoque.ProdutoId);

                if (produto != null)
                {
                    if (movimentacaoEstoque.TipoMovi == TipoMovimentacao.Entrada)
                    {
                        produto.Quantidade += movimentacaoEstoque.Quantidade;
                    }
                    else if (movimentacaoEstoque.TipoMovi == TipoMovimentacao.Saida)
                    {
                        if (produto.Quantidade < movimentacaoEstoque.Quantidade)
                        {
                            ModelState.AddModelError("", "Quantidade insuficiente em estoque!");
                            ViewData["ProdutoId"] = new SelectList(_context.Produto, "Id", "Descricao", movimentacaoEstoque.ProdutoId);
                            return View(movimentacaoEstoque);
                        }
                        produto.Quantidade -= movimentacaoEstoque.Quantidade;
                    }
                }

                _context.Add(movimentacaoEstoque);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ProdutoId"] = new SelectList(_context.Produto, "Id", "Descricao", movimentacaoEstoque.ProdutoId);
            return View(movimentacaoEstoque);
        }
        // GET: MovimentacaoEstoques/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movimentacaoEstoque = await _context.MovimentacaoEstoque.FindAsync(id);
            if (movimentacaoEstoque == null)
            {
                return NotFound();
            }
            ViewData["ProdutoId"] = new SelectList(_context.Produto, "Id", "Descricao", movimentacaoEstoque.ProdutoId);
            return View(movimentacaoEstoque);
        }

        // POST: MovimentacaoEstoques/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Quantidade,ProdutoId,Data,TipoMovi")] MovimentacaoEstoque movimentacaoEstoque)
        {
            if (id != movimentacaoEstoque.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movimentacaoEstoque);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovimentacaoEstoqueExists(movimentacaoEstoque.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProdutoId"] = new SelectList(_context.Produto, "Id", "Descricao", movimentacaoEstoque.ProdutoId);
            return View(movimentacaoEstoque);
        }

        // GET: MovimentacaoEstoques/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movimentacaoEstoque = await _context.MovimentacaoEstoque
                .Include(m => m.Produto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movimentacaoEstoque == null)
            {
                return NotFound();
            }

            return View(movimentacaoEstoque);
        }

        // POST: MovimentacaoEstoques/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var movimentacaoEstoque = await _context.MovimentacaoEstoque.FindAsync(id);
            if (movimentacaoEstoque != null)
            {
                _context.MovimentacaoEstoque.Remove(movimentacaoEstoque);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovimentacaoEstoqueExists(long id)
        {
            return _context.MovimentacaoEstoque.Any(e => e.Id == id);
        }
    }
}
