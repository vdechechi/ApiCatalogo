namespace ApiCatalogo.Models
{
    public class Categoria
    {
        public Categoria()
        {
            Id = Guid.NewGuid();
            Produtos = new List<Produto>(); // Usando List em vez de Collection
        }

        public Guid Id { get; set; }
        public string Nome { get; set; } = String.Empty; // Sem nulabilidade, pois tem valor padrão
        public string ImagemUrl { get; set; } = String.Empty;
        public ICollection<Produto>? Produtos { get; set; } // Sem nulabilidade
    }
}
        