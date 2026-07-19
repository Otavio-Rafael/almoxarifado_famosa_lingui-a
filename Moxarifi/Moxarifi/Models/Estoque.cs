using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moxarifi.Models
{
    public class Estoque
    {
        private long id;
        private string nome;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get => id; set => id = value; }

        [Display(Name = "Local Armazenado")]
        [MaxLength(5, ErrorMessage = "Maximo de 5 caracteres")]
        [Required(ErrorMessage = "{0} campo é obrigatório")]
        public string Nome { get => nome; set => nome = value; }

    }
}
