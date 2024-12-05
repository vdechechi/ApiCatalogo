using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using ApiCatalogo.Repositorys.Generico;
using X.PagedList;

namespace ApiCatalogo.Repositorys.Categorias
{
    public interface ICategoriasRepository : IRepository<Categoria>
    {
        Task<IPagedList<Categoria>> GetCategoriasAsync(CategoriasParameters categoriasParameters);
        Task<IPagedList<Categoria>> GetCategoriasFiltroNomeAsync(CategoriasFiltroNome categoriasFiltroParameters);

    }
}
