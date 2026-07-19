using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Almoxarifado.Models;
using Moxarifi.Models;

namespace Moxarifi.Data
{
    public class MoxarifiContext : DbContext
    {
        public MoxarifiContext (DbContextOptions<MoxarifiContext> options)
            : base(options)
        {
        }

        public DbSet<Almoxarifado.Models.Classificacao> Classificacao { get; set; } = default!;
        public DbSet<Almoxarifado.Models.Produto> Produto { get; set; } = default!;
        public DbSet<Moxarifi.Models.Estoque> Estoque { get; set; } = default!;
        public DbSet<Moxarifi.Models.Imagem> Imagem { get; set; } = default!;
        public DbSet<Moxarifi.Models.MovimentacaoEstoque> MovimentacaoEstoque { get; set; } = default!;
       
    }
}
