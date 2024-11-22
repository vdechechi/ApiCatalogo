
using ApiCatalogo.Context;
using ApiCatalogo.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Repositorys.Produtos
{
    public class ProdutosRepository : IProdutosRepository
    {
        private readonly AppDbContext _context;
        public ProdutosRepository(AppDbContext context)
        {
            _context = context;
        }
        public Produto Create(Produto produto)
        {
            if (produto == null)
            {
                throw new ArgumentNullException(nameof(produto));
            }

            _context.Produtos.Add(produto);
            _context.SaveChanges();

            return produto;
            }

            public Produto Delete(int id)
        {
            var produto = _context.Produtos.Find(id);

            if(produto == null)
            {
                throw new ArgumentNullException(nameof(produto));
            }

            _context.Produtos.Remove(produto);  
            _context.SaveChanges();
            return produto;

        }

        public Produto GetProduto(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(c => c.Id == id);
            return produto;
        }

        public ICollection<Produto> GetProdutos()
        {
            var produtos = _context.Produtos.ToList();

            return produtos;
        }

        public Produto Update(Produto produto)
        {
            if (produto == null)
            {
                throw new ArgumentNullException(nameof(produto));
            }
            _context.Produtos.Update(produto);
            _context.SaveChanges();
            return produto; 
        }
    }
}
