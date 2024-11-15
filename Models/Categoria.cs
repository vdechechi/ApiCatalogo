using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiCatalogo.Models
{
    [Table("categorias")]
    public class Categoria
    {
        public Categoria()
        {
            Produtos = new List<Produto>(); // Usando List em vez de Collection
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(80)]
        [MinLength(3)]
        public string Nome { get; set; } = String.Empty; // Sem nulabilidade, pois tem valor padrão

        [Required]
        [MaxLength(1024)]
        public string ImagemUrl { get; set; } = String.Empty;
        public ICollection<Produto>? Produtos { get; set; } // Sem nulabilidade
    }
}
        