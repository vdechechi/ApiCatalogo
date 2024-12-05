using ApiCatalogo.Pagination;
using ApiCatalogo.Repositorys.Generico;

namespace ApiCatalogo.Repositorys.Produtos
{
    public interface IProdutosRepository : IRepository<Produto>
    {
        //IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParameters);
        IEnumerable<Produto> GetProdutosPorCategoria(int id);
        PagedList<Produto> GetProdutos(ProdutosParameters produtosParameters);
        PagedList<Produto> GetProdutosFiltroPreco(ProdutosFiltroPreco produtosFiltroPrecoParams);




    }
}
