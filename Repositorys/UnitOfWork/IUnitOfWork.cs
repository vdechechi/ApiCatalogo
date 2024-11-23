using ApiCatalogo.Repositorys.Categorias;
using ApiCatalogo.Repositorys.Produtos;

namespace ApiCatalogo.Repositorys.UnitOfWork
{
    public interface IUnitOfWork
    {
        IProdutosRepository ProdutosRepository { get; }
        ICategoriasRepository CategoriasRepository { get; }
        void Commit(); //Confirmar todas as alterações pendentes
    }
}
