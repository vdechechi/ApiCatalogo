using ApiCatalogo.Repositorys.Categorias;
using ApiCatalogo.Repositorys.Produtos;

namespace ApiCatalogo.Repositorys.UnitOfWork
{
    public interface IUnitOfWork
    {
        IProdutosRepository ProdutosRepository { get; }
        ICategoriasRepository CategoriasRepository { get; }
        Task CommitAsync(); //Confirmar todas as alterações pendentes
    }
}
