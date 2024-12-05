using ApiCatalogo.Context;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using ApiCatalogo.Repositorys.Generico;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ApiCatalogo.Repositorys.Categorias
{
    public class CategoriasRepository : Repository<Categoria>, ICategoriasRepository
    {
        public CategoriasRepository(AppDbContext context) : base(context)
        {
        }

        public PagedList<Categoria> GetCategorias(CategoriasParameters categoriasParameters)
        {
            var categorias = GetAll().OrderBy(c => c.Id).AsQueryable();

            var categoriasOrdenadas = PagedList<Categoria>.toPagedList(categorias, categoriasParameters.PageNumber, categoriasParameters.PageSize);

            return categoriasOrdenadas;
        }

        public PagedList<Categoria> GetCategoriasFiltroNome(CategoriasFiltroNome categoriasFiltroParameters)
        {
            var categorias = GetAll().AsQueryable();

            if (!string.IsNullOrEmpty(categoriasFiltroParameters.Nome))
            {
                categorias = categorias.Where(c => c.Nome.Contains(categoriasFiltroParameters.Nome));
            }

            var categoriasFiltradas = PagedList<Categoria>.toPagedList(categorias, categoriasFiltroParameters.PageNumber, categoriasFiltroParameters.PageSize);
            return categoriasFiltradas;

        }
    }
}

