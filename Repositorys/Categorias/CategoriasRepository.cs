using ApiCatalogo.Context;
using ApiCatalogo.Models;
using ApiCatalogo.Repositorys.Generico;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace ApiCatalogo.Repositorys.Categorias
{
    public class CategoriasRepository : Repository<Categoria>, ICategoriasRepository
    {
        public CategoriasRepository(AppDbContext context) :base(context)
        { 
        }
    }
}
