using ApiCatalogo.Context;
using ApiCatalogo.Models;
using ApiCatalogo.Repositorys.Produtos;
using APICatalogo.Controllers;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly ILogger<ProdutosController> _logger;
        private readonly IProdutosRepository _repostitory;
        public ProdutosController(IProdutosRepository repostitory, ILogger<ProdutosController> logger)
        {
            _logger = logger;
            _repostitory = repostitory;
        }

        [HttpGet]
        public IActionResult GetProdutos()
        {
            var produtos = _repostitory.GetProdutos();

            if(produtos == null)
            {
                return NotFound("Lista de produtos vazia");
            }

            return Ok(produtos); 
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetProdutoById")]
            public IActionResult GetProdutoById([FromRoute] int id)
        {
            var produto = _repostitory.GetProduto(id);

            if (produto == null)
            {
                return NotFound("Produto não encontrado");
            }

            return Ok(produto);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Produto produto)
        {
            if (produto == null) return BadRequest();

            var produtoCriado = _repostitory.Create(produto);

            return new CreatedAtRouteResult("GetProdutoById",
                new { id = produtoCriado.Id }, produtoCriado);
        }

        [HttpPut]
        [Route("{id:int}")]
        public IActionResult Put([FromBody] Produto produto, [FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (produto.Id != id) {
                
                return BadRequest();
            }

            var ProdutoAtualizado = _repostitory.Update(produto);

            return Ok(ProdutoAtualizado);
           
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult Delete(int id)
        { 

            var produtoDeleteado = _repostitory.Delete(id); 

            return Ok($"Produto com id = {id} Deletado com sucesso");


        }






    }
}
