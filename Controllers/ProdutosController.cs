using ApiCatalogo.Context;
using ApiCatalogo.DTO;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using ApiCatalogo.Repositorys.Generico;
using ApiCatalogo.Repositorys.Produtos;
using ApiCatalogo.Repositorys.UnitOfWork;
using APICatalogo.Controllers;
using AutoMapper;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ApiCatalogo.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase 
    
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        public ProdutosController(IUnitOfWork uof,IMapper mapper)
        {
            _uof = uof;
            _mapper = mapper;
        }

        [HttpGet("Pagination")]
        public ActionResult<IEnumerable<ProdutoDto>> Get([FromQuery] ProdutosParameters produtosParameters)
        {

            var produtos = _uof.ProdutosRepository.GetProdutos(produtosParameters);

            var metadata = new
            {
                produtos.TotalCount,
                produtos.PageSize,
                produtos.CurrentPage,
                produtos.TotalPages,
                produtos.HasNext,
                produtos.HasPrevious
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata)); 

            var produtosDto = _mapper.Map<IEnumerable<ProdutoDto>>(produtos);

            return Ok(produtosDto);

        }
        [HttpGet]
        public ActionResult<IEnumerable<ProdutoDto>> GetProdutos()
        {
            var produtos = _uof.ProdutosRepository.GetAll();

            if(produtos == null)
            {
                return NotFound("Lista de produtos vazia");
            }
            //var destino = _mapper.map<Destino>(origem)

            var produtosDto = _mapper.Map<IEnumerable<ProdutoDto>>(produtos);
            return Ok(produtosDto); 
        }

        [HttpGet("Categoria/{id:int}")]

        public ActionResult<IEnumerable<ProdutoDto>> GetProdutosPorCategoria(int id)
        {
            var produtos = _uof.ProdutosRepository.GetProdutosPorCategoria(id);

            if (produtos == null) return NotFound();

            var produtosDto = _mapper.Map<IEnumerable<ProdutoDto>>(produtos);
            return Ok(produtosDto);
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetProdutoById")]
            public ActionResult<ProdutoDto> GetProdutoById([FromRoute] int id)
        {
            var produto = _uof.ProdutosRepository.Get(p => p.Id == id);

            if (produto == null)
            {
                return NotFound("Produto não encontrado");
            }
            var produtoDto = _mapper.Map<ProdutoDto>(produto);

            return Ok(produtoDto);
        }

        [HttpPost]
        public ActionResult<ProdutoDto> Post([FromBody] ProdutoDto produtoDto)
        {
            if (produtoDto == null) return BadRequest();

            var produto = _mapper.Map<Produto>(produtoDto); ;

            var novoProduto = _uof.ProdutosRepository.Create(produto);
            _uof.Commit();

            var novoProdutoDto = _mapper.Map<ProdutoDto>(novoProduto);


            return new CreatedAtRouteResult("GetProdutoById",
                new { id = novoProdutoDto.Id }, novoProdutoDto);
        }

        [HttpPatch("{id:int}")]
        public ActionResult<ProdutoDtoUpdateResponse> Patch(int id,
                                               JsonPatchDocument<ProdutoDtoUpdateRequest> patchProdutoDto)
        {
            if (patchProdutoDto == null || id <= 0) { return BadRequest(); }

            var produto = _uof.ProdutosRepository.Get(p => p.Id == id);

            if (produto is null)
            {
                return NotFound();
            }

            var produtoUpdateRequest = _mapper.Map<ProdutoDtoUpdateRequest>(produto);

            patchProdutoDto.ApplyTo(produtoUpdateRequest, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(produtoUpdateRequest, produto);

            _uof.ProdutosRepository.Update(produto);
            _uof.Commit();

            return Ok(_mapper.Map<ProdutoDtoUpdateResponse>(produto));
        }



        [HttpPut]
        [Route("{id:int}")]
        public ActionResult<ProdutoDto> Put([FromBody] ProdutoDto produtoDto, [FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (produtoDto.Id != id) {
                
                return BadRequest();
            }

            var produto = _mapper.Map<Produto>(produtoDto);

            var produtoAtualizado = _uof.ProdutosRepository.Update(produto);
            _uof.Commit();

            var produtoAtualizadoDto = _mapper.Map<ProdutoDto>(produtoAtualizado);
                  
            return Ok(produtoAtualizadoDto);
           
        }

        [HttpDelete]
        [Route("{id:int}")]
        public ActionResult<ProdutoDto> Delete(int id)
        {
            var produto = _uof.ProdutosRepository.Get(p => p.Id == id);

            if(produto == null)
            {
                return NotFound("Categoria não encontrada");
            }
            var produtoDeletado = _uof.ProdutosRepository.Delete(produto);
            _uof.Commit();

            var produtoDeletadoDto = _mapper.Map<ProdutoDto>(produtoDeletado);

            return Ok(produtoDeletadoDto);


        }






    }
}
