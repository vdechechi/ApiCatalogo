namespace ApiCatalogo.Models
{
    public class Categoria
    {
        public Categoria()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public string? Nome { get; set; } = String.Empty;
        public string? ImagemUrl { get; set; } = String.Empty;
    }
}
