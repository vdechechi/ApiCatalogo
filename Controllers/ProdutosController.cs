using ApiCatalogo.Context;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogo.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {

        private readonly AppDbContext _context;
        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = _context.Produtos.ToList();

            if(produtos == null)
            {
                return NotFound("Lista de produtos vazia");
            }

            return produtos; 
        }

        [HttpGet]
        [Route("{id:int}")]
        public ActionResult<Produto> GetById([FromRoute] int id)
        {
            var produto = _context.Produtos.FirstOrDefault(x => x.Id == id);

            if (produto == null)
            {
                return NotFound("Produto não encontrado");
            }

            return produto;
        }





    }
}
