using ApiCatalogo.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiCatalogo.DTO
{
    public class ProdutoDto
    {
        public int Id { get; set; } 

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 80 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "A descrição é obrigatória")]
        [StringLength(1024, MinimumLength = 3, ErrorMessage = "A descrição deve ter entre 3 e 1024 caracteres")]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "O preço é obrigatório")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "A URL da imagem é obrigatória")]
        [MaxLength(1024, ErrorMessage = "A URL da imagem deve ter no máximo 1024 caracteres")]
        public string ImagemUrl { get; set; } = string.Empty;

        [Required(ErrorMessage = "O ID da categoria é obrigatório")]
        public int CategoriaId { get; set; }
    }
}
