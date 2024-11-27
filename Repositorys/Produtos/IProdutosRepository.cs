using ApiCatalogo.Pagination;
using ApiCatalogo.Repositorys.Generico;

namespace ApiCatalogo.Repositorys.Produtos
{
    public interface IProdutosRepository : IRepository<Produto>
    {

        IEnumerable<Produto> GetProdutosPorCategoria(int id);
        IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParameters);


    }
}
