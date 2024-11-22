using ApiCatalogo.Context;
using ApiCatalogo.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace ApiCatalogo.Repositorys.Categorias
{
    public class CategoriasRepository : ICategoriasRepository
    {
        private readonly AppDbContext _context;

        public CategoriasRepository(AppDbContext context)
        {
            _context = context;

        }

        public Categoria Create(Categoria categoria)
        {
            if (categoria == null)
            {
                throw new ArgumentNullException(nameof(categoria));
            }

            _context.Categorias.Add(categoria);
            _context.SaveChanges();

            return categoria;
        }

        public Categoria Delete(int id)
        {
            var categoria = _context.Categorias.Find(id);

            if (categoria == null)
            {
                throw new ArgumentNullException(nameof(categoria));
            }

            _context.Categorias.Remove(categoria);

            _context.SaveChanges();

            return categoria;
        }

        public Categoria GetCategoria(int id)
        {
            return _context.Categorias.FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<Categoria> GetCategorias()
        {
            return _context.Categorias.ToList(); ;

        }

        public Categoria Update(Categoria categoria)
        {
            if (categoria == null)
            {
                throw new ArgumentNullException(nameof(categoria));

            }

            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();

            return categoria;
        }
    }
}
