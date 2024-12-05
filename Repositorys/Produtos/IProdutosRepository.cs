using ApiCatalogo.Pagination;
using ApiCatalogo.Repositorys.Generico;
using X.PagedList;

namespace ApiCatalogo.Repositorys.Produtos
{
    public interface IProdutosRepository : IRepository<Produto>
    {
        //IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParameters);
        Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int id);
        Task<IPagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParameters);
        Task<IPagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosFiltroPrecoParams);




    }
}
