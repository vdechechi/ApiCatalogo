using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiCatalogo.Migrations
{
    /// <inheritdoc />
    public partial class PopulaProdutos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("insert into produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, CategoriaId, DataCadastro) values ('Coca Cola', 'Refrigerante de Cola', 9.99, 'cocacola.jpg', 50, 1, now())");
            mb.Sql("insert into produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, CategoriaId, DataCadastro) values ('X-Bacon', 'Hamburguer saboroso com carne, queijo, bacon e salada', 24.90, 'xbacon.jpg', 100, 2, now())");
            mb.Sql("insert into produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, CategoriaId, DataCadastro) values ('Petit Gateau', 'Receita secreta de petit gateau que derrete na boca', 19.99, 'petitgateau.jpg', 50, 3, now())");
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder mb)
        {

            mb.Sql("delete from produtos");

        }
    }
}
