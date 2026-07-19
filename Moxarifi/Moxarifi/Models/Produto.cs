using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Moxarifi.Models;

namespace Almoxarifado.Models

{
    public class Produto
    {
        private long id;
        private string nome;
        private string descricao;
        private int quantidade;
        private ICollection<Classificacao>? classificacao;
        private ICollection<Estoque> estoque;
        private ICollection<Imagem>? listaImagem;
        private ICollection<MovimentacaoEstoque>? moviEsto;

        private List<IFormFile> arquivo = new List<IFormFile>();
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get => id; set => id = value; }


        [Display(Name ="Quantidade em estoque")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        public int Quantidade { get => quantidade; set => quantidade = value; }


        [Display(Name ="Descrição do produto")]
        [MaxLength(350,ErrorMessage ="Maximo de 350 caracteres")]
        [Required(ErrorMessage ="{0} campo é obrigatório")]
        public string Descricao { get => descricao; set => descricao = value; }


        [Display(Name = "Nome")]
        [MaxLength(60, ErrorMessage = "Nome maior que 60 caracteres")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        public string Nome { get => nome; set => nome = value; }
        [Display(Name = "Classificação")]
        public long ClassificacaoID { get; set; }
        public Classificacao? Classificacao { get; set; }
        [Display(Name ="Local armazenado")]
        public long EstoqueID { get; set; }
        public Estoque? Estoque { get; set; }
        //public Imagem Imagem { get; set; }
        //[NotMapped]
        //public List<IFormFile> Arquivo { get => arquivo; set => arquivo = value; }
        [Display(Name = "Foto")]
        
        public ICollection<Imagem>? ListaImagem { get => listaImagem; set => listaImagem = value; }
        public ICollection<MovimentacaoEstoque>? MoviEsto { get; set; }
    }
}
