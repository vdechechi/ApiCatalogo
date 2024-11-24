using ApiCatalogo.Models;

namespace ApiCatalogo.DTO
{
    public class ProdutoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public string ImagemUrl { get; set; } = string.Empty;
        public int CategoriaId { get; set; }
    }
}
