using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Almoxarifado.Models
{
    public class Classificacao
    {
        private long id;
        private string classificacao;
        private string descricacao;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get => id; set => id = value; }

        [Display(Name ="Classificação")]
        [MaxLength(55,ErrorMessage ="Maior que 55 caracter")]
        [Required(ErrorMessage ="{0} campo obrigatório")]
        public string Classifi { get => classificacao; set => classificacao = value; }
        [Display(Name ="Descrição")]
        [MaxLength(300,ErrorMessage="Max 300 caracter")]
        [Required(ErrorMessage = "{0} campo obrigatório")]
        public string Descricacao { get => descricacao; set => descricacao = value; }
    }
}
