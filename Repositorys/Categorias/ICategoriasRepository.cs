using ApiCatalogo.Models;

namespace ApiCatalogo.Repositorys.Categorias
{
    public interface ICategoriasRepository
    {
        IEnumerable<Categoria> GetCategorias();
        Categoria GetCategoria(int id);
        Categoria Create(Categoria categoria);
        Categoria Update(Categoria categoria);
        Categoria Delete(int id);
    }
}
