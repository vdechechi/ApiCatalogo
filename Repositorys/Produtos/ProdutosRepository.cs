
using ApiCatalogo.Context;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using ApiCatalogo.Repositorys.Generico;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace ApiCatalogo.Repositorys.Produtos
{
    public class ProdutosRepository : Repository<Produto>, IProdutosRepository
    {
        public ProdutosRepository(AppDbContext context) : base(context)
        {
            
        }

        //public IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParameters)
        //{
        //    return GetAll()
        //        .OrderBy(on => on.Nome)
        //        .Skip((produtosParameters.PageNumber - 1) * produtosParameters.PageSize)
        //        .Take(produtosParameters.PageSize).ToList();
        //}

        public async Task<IPagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParameters)
        {
            var produtos = await GetAllAsync();

            var produtosOrdenados = produtos.OrderBy( p => p.Id ).AsQueryable();

            //var result = PagedList<Produto>.toPagedList(produtosOrdenados, produtosParameters.PageNumber, produtosParameters.PageSize);

            var result = await produtosOrdenados.ToPagedListAsync(produtosParameters.PageNumber, produtosParameters.PageSize);

            return result;
        }

        public async Task<IPagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosFiltroPrecoParams)
        {
            var produtos = await GetAllAsync();


            if(produtosFiltroPrecoParams.Preco.HasValue && !string.IsNullOrEmpty(produtosFiltroPrecoParams.PrecoCriterio))
            {
                if(produtosFiltroPrecoParams.PrecoCriterio.Equals("maior", StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p => p.Preco > produtosFiltroPrecoParams.Preco.Value).OrderBy(p => p.Id);
                }
                else if (produtosFiltroPrecoParams.PrecoCriterio.Equals("menor", StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p => p.Preco < produtosFiltroPrecoParams.Preco.Value).OrderBy(p => p.Id);
                }
                else if (produtosFiltroPrecoParams.PrecoCriterio.Equals("igual", StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p => p.Preco == produtosFiltroPrecoParams.Preco.Value).OrderBy(p => p.Id);
                }
            }
            var produtosFiltrados = await produtos.ToPagedListAsync(produtosFiltroPrecoParams.PageNumber, produtosFiltroPrecoParams.PageSize);

            return produtosFiltrados;
        }

        public async Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int id)
        {
            var produtos = await GetAllAsync();

            var produtosCategoria = produtos.Where(c => c.CategoriaId == id);

            return produtosCategoria;
        }
    }
}
