using ApiCatalogo.Context;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public ActionResult<IEnumerable<Produto>> GetProdutos()
        {
            var produtos = _context.Produtos.ToList();

            if(produtos == null)
            {
                return NotFound("Lista de produtos vazia");
            }

            return produtos; 
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetProdutoById")]
        public ActionResult<Produto> GetProdutoById([FromRoute] int id)
        {
            var produto = _context.Produtos.FirstOrDefault(x => x.Id == id);

            if (produto == null)
            {
                return NotFound("Produto não encontrado");
            }

            return produto;
        }

        [HttpPost]
        public ActionResult<Produto> Post([FromBody] Produto produto)
        {
            if(produto == null) return BadRequest();   

            _context.Produtos.Add(produto);

            _context.SaveChanges();

            return new CreatedAtRouteResult("GetProdutoById",
                new { id = produto.Id }, produto);
        }

        [HttpPut]
        [Route("{id:int}")]
        public ActionResult<Produto> Put([FromBody] Produto produto, [FromRoute] int id)
        {
            if (produto == null) { return NotFound(); }
            else
            {
                if (produto.Id != id)
                {
                    return BadRequest("Produto nao encontrado");
                }

                _context.Entry(produto).State = EntityState.Modified;
                _context.SaveChanges();

                return Ok(produto);
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(x => x.Id == id);

            if (produto == null)
            {
                return NotFound("Produto não encontrado");
            }

            _context.Produtos.Remove(produto);

            _context.SaveChanges();

            return Ok("Produto Deletado com sucesso");


        }






    }
}
