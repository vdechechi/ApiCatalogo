using ApiCatalogo.Models;
using System.ComponentModel.DataAnnotations;

public class Produto
{
    public Produto()
    {
        Id = Guid.NewGuid();
        DataCadastro = DateTime.UtcNow;
    }
    public Guid Id { get; set; }
    public string Nome { get; set; } = String.Empty;
    public string Descricao { get; set; } = String.Empty;
    public string ImagemUrl { get; set; } = String.Empty;
    public decimal Preco { get; set; }
    public decimal Estoque { get; set; }
    public DateTime DataCadastro { get; set; }

    public Guid CategoriaId{ get; set; }

    public Categoria? Categoria { get; set; }
}
