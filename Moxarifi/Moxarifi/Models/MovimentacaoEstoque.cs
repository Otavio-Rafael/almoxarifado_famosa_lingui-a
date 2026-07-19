using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Almoxarifado.Models;

namespace Moxarifi.Models
{
    public class MovimentacaoEstoque
    {
        private long id;
        private int quantidade;
        //private ICollection<Produto>? produto;
        private DateTime data;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get => id; set => id = value; }

        [Display (Name ="Quantidade")]
        [Required(ErrorMessage = "Campo obrigatório")]
        public int Quantidade { get => quantidade; set => quantidade = value; }

        public Produto? Produto { get ; set ; }
        [Display(Name = "Escolha o Produto")]
        public long ProdutoId { get; set; }

        [Display(Name = "Data")]
        [DataType(DataType.Date)]
        public DateTime Data { get => data; set => data = value; } 
        [Display(Name ="Tipo de Movimentação")]
        [Required(ErrorMessage = "Selecione o tipo")]
        public TipoMovimentacao TipoMovi { get; set; }


    }
        public enum TipoMovimentacao
        {
            Entrada = 1,
            Saida = 2 
        }
}
