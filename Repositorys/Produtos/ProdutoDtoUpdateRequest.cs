using System.ComponentModel.DataAnnotations;

namespace ApiCatalogo.Repositorys.Produtos
{
    public class ProdutoDtoUpdateRequest : IValidatableObject
    {
        [Range(1,9999, ErrorMessage = "Estoque deve tar entre 1 e 9.999")]
        public int Estoque { get; set; }
        public DateTime DataCadastro { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(DataCadastro.Date <= DateTime.Now.Date)
            {
                yield return new ValidationResult("A data de cadastro deve ser maior que a data de cadastro atual",
                    new[] {nameof(this.DataCadastro)});

            }
        }
    }
}
