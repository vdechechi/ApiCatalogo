using ApiCatalogo.Context;
using ApiCatalogo.Models;
using ApiCatalogo.Repositorys.Generico;
using ApiCatalogo.Repositorys.Produtos;
using ApiCatalogo.Repositorys.UnitOfWork;
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
        private readonly IUnitOfWork _uof;
        public ProdutosController(IUnitOfWork uof, ILogger<ProdutosController> logger)
        {
            _logger = logger;
            _uof = uof;
        }

        [HttpGet]
        public IActionResult GetProdutos()
        {
            var produtos = _uof.ProdutosRepository.GetAll();

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
            var produto = _uof.ProdutosRepository.Get(p => p.Id == id);

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

            var produtoCriado = _uof.ProdutosRepository.Create(produto);

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

            var ProdutoAtualizado = _uof.ProdutosRepository.Update(produto);

            return Ok(ProdutoAtualizado);
           
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult Delete(int id)
        {
            var produto = _uof.ProdutosRepository.Get(p => p.Id == id);
            if(produto == null)
            {
                _logger.LogWarning("Categoria nao encontrada");
                return NotFound("Categoria não encontrada");
            }

            var produtoDeleteado = _uof.ProdutosRepository.Delete(produto); 

            return Ok($"Produto com id = {id} Deletado com sucesso");


        }






    }
}
