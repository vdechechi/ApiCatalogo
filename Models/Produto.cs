using ApiCatalogo.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


[Table("produtos")]
public class Produto
{
    public Produto()
    {
        DataCadastro = DateTime.UtcNow;
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(80)]
    [MinLength(3)]
    public string Nome { get; set; } = String.Empty;

    [Required]
    [MaxLength(1024)]
    [MinLength(3)]
    public string Descricao { get; set; } = String.Empty;

    [Required]
    [Column(TypeName = "decimal (10,2)")]
    public decimal Preco { get; set; }

    [Required]
    [MaxLength(1024)]
    public string ImagemUrl { get; set; } = String.Empty;

    public int Estoque { get; set; }
    public DateTime DataCadastro { get; set; }

    public int CategoriaId{ get; set; }

    [JsonIgnore]
    public Categoria? Categoria { get; set; }
}
