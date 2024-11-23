using ApiCatalogo.Context;
using ApiCatalogo.Repositorys.Categorias;
using ApiCatalogo.Repositorys.Produtos;

namespace ApiCatalogo.Repositorys.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private IProdutosRepository? _produtoRepo;

        private ICategoriasRepository? _categoriaRepo;

        public AppDbContext _context;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IProdutosRepository ProdutosRepository
        {
            get
            {
                return _produtoRepo ?? new ProdutosRepository(_context);
            }
        }

        public ICategoriasRepository CategoriasRepository
        {
            get
            {
                return _categoriaRepo ?? new CategoriasRepository(_context);
            }
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
