using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Almoxarifado.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Moxarifi.Data;
using Moxarifi.Models;
using X.PagedList;
using X.PagedList.Extensions;
namespace Moxarifi.Controllers
{
    public class ProdutosController : Controller
    {
        private readonly MoxarifiContext _context;

        public ProdutosController(MoxarifiContext context)
        {
            _context = context;
        }

        // GET: Produtos
        // No topo do arquivo adiciona:


        // ← adiciona esse using no topo

        public async Task<IActionResult> Index(int? pagina)
        {
            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;

            var lista = await _context.Produto
                                .Include(p => p.Classificacao)
                                .Include(p => p.Estoque)
                                .Include(p => p.ListaImagem) // ← faltava esse
                                .ToListAsync();

            return View(lista.ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: Produtos/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produto
                .Include(p => p.Classificacao)
                .Include(p => p.Estoque)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        // GET: Produtos/Create
        public IActionResult Create()
        {
            ViewData["ClassificacaoID"] = new SelectList(_context.Classificacao, "Id", "Classifi");
            ViewData["EstoqueID"] = new SelectList(_context.Estoque, "Id", "Nome");
            return View();
        }

        // POST: Produtos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Quantidade,Descricao,Nome,ClassificacaoID,EstoqueID")] Produto produto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(produto);
                await _context.SaveChangesAsync(); // ← salva produto primeiro para gerar o Id

                // ← aqui gera a movimentação automática
                if (produto.Quantidade > 0)
                {
                    var movimentacao = new MovimentacaoEstoque
                    {
                        ProdutoId = produto.Id,
                        Quantidade = produto.Quantidade,
                        Data = DateTime.Today,
                        TipoMovi = TipoMovimentacao.Entrada
                    };
                    _context.Add(movimentacao);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index)); // ← movido para depois de tudo
            }
            ViewData["ClassificacaoID"] = new SelectList(_context.Classificacao, "Id", "Classifi", produto.ClassificacaoID);
            ViewData["EstoqueID"] = new SelectList(_context.Estoque, "Id", "Nome", produto.EstoqueID);
            return View(produto);
        }
        // GET: Produtos/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produto.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }
            ViewData["ClassificacaoID"] = new SelectList(_context.Classificacao, "Id", "Classifi", produto.ClassificacaoID);
            ViewData["EstoqueID"] = new SelectList(_context.Estoque, "Id", "Nome", produto.EstoqueID);
            return View(produto);
        }

        // POST: Produtos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Quantidade,Descricao,Nome,ClassificacaoID,EstoqueID")] Produto produto)
        {
            if (id != produto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(produto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProdutoExists(produto.Id))
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
            ViewData["ClassificacaoID"] = new SelectList(_context.Classificacao, "Id", "Classifi", produto.ClassificacaoID);
            ViewData["EstoqueID"] = new SelectList(_context.Estoque, "Id", "Nome", produto.EstoqueID);
            return View(produto);
        }

        // GET: Produtos/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produto
                .Include(p => p.Classificacao)
                .Include(p => p.Estoque)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        // POST: Produtos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var produto = await _context.Produto.FindAsync(id);
            if (produto != null)
            {
                _context.Produto.Remove(produto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProdutoExists(long id)
        {
            return _context.Produto.Any(e => e.Id == id);
        }
    }
}
