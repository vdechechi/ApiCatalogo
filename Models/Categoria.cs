using ApiCatalogo.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiCatalogo.Models
{
    [Table("categorias")]
    public class Categoria
    {
        public Categoria()
        {
            Produtos = new List<Produto>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 80 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "A URL da imagem é obrigatória")]
        [MaxLength(1024, ErrorMessage = "A URL da imagem deve ter no máximo 1024 caracteres")]
        public string ImagemUrl { get; set; } = string.Empty;

        public ICollection<Produto>? Produtos { get; set; }
    }
}
