using ApiCatalogo.Context;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using ApiCatalogo.Repositorys.Generico;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Runtime.CompilerServices;
using X.PagedList.Mvc.Core;
using X.PagedList;


namespace ApiCatalogo.Repositorys.Categorias
{
    public class CategoriasRepository : Repository<Categoria>, ICategoriasRepository
    {
        public CategoriasRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IPagedList<Categoria>> GetCategoriasAsync(CategoriasParameters categoriasParameters)
        {
            var categorias = await GetAllAsync();

            var categoriasOrdenadas = categorias.OrderBy(p => p.Id).AsQueryable();

            //var result = PagedList<Categoria>.ToPagedList(categoriasOrdenadas, categoriasParameters.PageNumber, categoriasParameters.PageSize);

            var result = await categoriasOrdenadas.ToPagedListAsync(categoriasParameters.PageNumber, categoriasParameters.PageSize);

            return result;
        }

        public async Task<IPagedList<Categoria>> GetCategoriasFiltroNomeAsync(CategoriasFiltroNome categoriasFiltroParameters)
        {
            var categorias = await GetAllAsync();

            if (!string.IsNullOrEmpty(categoriasFiltroParameters.Nome))
            {
                categorias = categorias.Where(c => c.Nome.Contains(categoriasFiltroParameters.Nome));
            }

            //var categoriasFiltradas = PagedList<Categoria>.toPagedList(categorias.AsQueryable(), categoriasFiltroParameters.PageNumber, categoriasFiltroParameters.PageSize);

            var categoriasFiltradas = await categorias.ToPagedListAsync(categoriasFiltroParameters.PageNumber, categoriasFiltroParameters.PageSize);


            return categoriasFiltradas;

        }   
    }
}

