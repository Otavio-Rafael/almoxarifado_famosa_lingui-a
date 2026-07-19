using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Almoxarifado.Models;

namespace Moxarifi.Models
{
    public class Imagem
    {
        private long id;
        private string? caminho;// indicado do caminho onde as imagens estão do projeto
        private string? nomearquivo; // nome do arquivo que será gravado
        private Produto? imovelRef; // indicação do Imo’vel a qual pertence esta imagem
        private long produtoRefId; // chave primária do objeto Imovel
        private List<IFormFile> arquivo = new List<IFormFile>(); // lista de Imagens que serão         
                                                                 //carregadas  
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get => id; set => id = value; }
        public string? Caminho { get => caminho; set => caminho = value; }
        public string? Nomearquivo { get => nomearquivo; set => nomearquivo = value; }
        
        public Produto? ProdutoRef { get => imovelRef; set => imovelRef = value; }
        [Display(Name = "Escolha o Produto")]
        public long ProdutoRefId { get => produtoRefId; set => produtoRefId = value; }
        [NotMapped] // indicação para a informação abaixo não persistir no banco de dados
        [Display(Name ="Imagem")]
        public List<IFormFile> Arquivo { get => arquivo; set => arquivo = value; }

    }
}
