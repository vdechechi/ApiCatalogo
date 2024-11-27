
using ApiCatalogo.Context;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using ApiCatalogo.Repositorys.Generico;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Repositorys.Produtos
{
    public class ProdutosRepository : Repository<Produto>, IProdutosRepository
    {
        public ProdutosRepository(AppDbContext context) : base(context)
        {
            
        }

        public IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParameters)
        {
            return GetAll()
                .OrderBy(on => on.Nome)
                .Skip((produtosParameters.PageNumber - 1) * produtosParameters.PageSize)
                .Take(produtosParameters.PageSize).ToList();
        }

        public IEnumerable<Produto> GetProdutosPorCategoria(int id)
        {
            return GetAll().Where(c => c.CategoriaId == id);
        }
    }
}
