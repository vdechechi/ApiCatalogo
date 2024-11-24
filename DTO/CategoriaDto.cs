using System.ComponentModel.DataAnnotations;

namespace ApiCatalogo.DTO
{
    public class CategoriaDto

    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 80 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "A URL da imagem é obrigatória")]
        [MaxLength(1024, ErrorMessage = "A URL da imagem deve ter no máximo 1024 caracteres")]
        public string ImagemUrl { get; set; } = string.Empty;
    }
}
