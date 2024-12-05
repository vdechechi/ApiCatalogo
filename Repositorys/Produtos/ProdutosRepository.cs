
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

        //public IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParameters)
        //{
        //    return GetAll()
        //        .OrderBy(on => on.Nome)
        //        .Skip((produtosParameters.PageNumber - 1) * produtosParameters.PageSize)
        //        .Take(produtosParameters.PageSize).ToList();
        //}

        public PagedList<Produto> GetProdutos(ProdutosParameters produtosParameters)
        {
            var produtos = GetAll().OrderBy(p=> p.Id).AsQueryable();
            var produtosOrdenados = PagedList<Produto>.toPagedList(produtos, produtosParameters.PageNumber, produtosParameters.PageSize);

            return produtosOrdenados;
        }

        public PagedList<Produto> GetProdutosFiltroPreco(ProdutosFiltroPreco produtosFiltroPrecoParams)
        {
            var produtos = GetAll().AsQueryable();

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
            var produtosFiltrados = PagedList<Produto>.toPagedList(produtos, produtosFiltroPrecoParams.PageNumber, produtosFiltroPrecoParams.PageSize);

            return produtosFiltrados;
        }

        public IEnumerable<Produto> GetProdutosPorCategoria(int id)
        {
            return GetAll().Where(c => c.CategoriaId == id);
        }
    }
}
