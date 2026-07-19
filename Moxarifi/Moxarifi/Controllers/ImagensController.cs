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
using X.PagedList.Extensions;

namespace Moxarifi.Controllers
{
    public class ImagensController : Controller
    {
        private readonly MoxarifiContext _context;
        private readonly IWebHostEnvironment _iWebHostEnvironment;
        private string[] permittedExtensions = { ".png", ".jpeg", ".jpg", ".pdf" };
        private readonly long _fileSizeLimit = 10 * 1024 * 1024; // Mega*kilo*byte
        private readonly string pastaImagens = "Imagens";
        private string _pastaProjeto;

        public ImagensController(MoxarifiContext context, IWebHostEnvironment iwhe)
        {
            _context = context;
            _iWebHostEnvironment = iwhe;
            _pastaProjeto = _iWebHostEnvironment.WebRootPath;
        }

        // GET: Imagens
        public async Task<IActionResult> Index(int? pagina)
        {
            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;

            var lista = await _context.Imagem
                                .Include(i => i.ProdutoRef)
                                .ToListAsync();

            return View(lista.ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: Imagens/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imagem = await _context.Imagem
                .Include(i => i.ProdutoRef)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (imagem == null)
            {
                return NotFound();
            }

            return View(imagem);
        }

        // GET: Imagens/Create
        public IActionResult Create()
        {
            ViewData["ProdutoRefId"] = new SelectList(_context.Produto, "Id", "Nome");
            return View();
        }

        // POST: Imagens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Caminho,Nomearquivo,ProdutoRefId")] Imagem imagem)
        public async Task<IActionResult> Create([Bind("Id,ProdutoRefId,Arquivo")] Imagem imagem)
        {
            if (ModelState.IsValid)
            {
                _context.Database.BeginTransaction();
                foreach (var formFile in imagem.Arquivo)
                {
                    Imagem iaux = new Imagem();
                    var ext = Path.GetExtension(formFile.FileName).ToLowerInvariant();
                    if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
                    {
                        ViewData["ProdutoRefId"] = new SelectList(_context.Produto, "Id", "Nome",
                imagem.ProdutoRefId);
                        ViewData["Erro"] = "Arquivo " + formFile.FileName + " com extensão inválida. ";
                        _context.Database.RollbackTransaction();
                        return View(imagem);
                    }
                    if (formFile.Length > 0 & formFile.Length <= _fileSizeLimit)
                    {
                        iaux.ProdutoRefId = imagem.ProdutoRefId;

                        var filePath = Path.Combine(_pastaProjeto, pastaImagens);
                        filePath = Path.Combine(filePath.ToString(), Path.GetRandomFileName() + ext);
                        iaux.Nomearquivo = Path.GetFileName(filePath);
                        iaux.Caminho = pastaImagens;
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                        _context.Add(iaux);
                    }
                    else
                    {
                        ViewData["ImovelRefId"] = new SelectList(_context.Produto, "Id", "Nome",
                imagem.ProdutoRefId);
                        ViewData["Erro"] = "Arquivo " + formFile.FileName + " excede o tamanho permitido. ";
                        _context.Database.RollbackTransaction();
                        return View(imagem);
                    }
                }
                await _context.SaveChangesAsync();
                _context.Database.CommitTransaction();
                return RedirectToAction(nameof(Index));



            }
            ViewData["ProdutoRefId"] = new SelectList(_context.Produto, "Id", "Descricao", imagem.ProdutoRefId);
            return View(imagem);
        }

        // GET: Imagens/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imagem = await _context.Imagem.FindAsync(id);
            if (imagem == null)
            {
                return NotFound();
            }
            ViewData["ProdutoRefId"] = new SelectList(_context.Produto, "Id", "Descricao", imagem.ProdutoRefId);
            return View(imagem);
        }

        // POST: Imagens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Caminho,Nomearquivo,ProdutoRefId")] Imagem imagem)
        {
            if (id != imagem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(imagem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImagemExists(imagem.Id))
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
            ViewData["ProdutoRefId"] = new SelectList(_context.Produto, "Id", "Descricao", imagem.ProdutoRefId);
            return View(imagem);
        }

        // GET: Imagens/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imagem = await _context.Imagem
                .Include(i => i.ProdutoRef)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (imagem == null)
            {
                return NotFound();
            }

            return View(imagem);
        }

        // POST: Imagens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var imagem = await _context.Imagem.FindAsync(id);
            if (imagem != null)
            {
                _context.Imagem.Remove(imagem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImagemExists(long id)
        {
            return _context.Imagem.Any(e => e.Id == id);
        }
    }
}
