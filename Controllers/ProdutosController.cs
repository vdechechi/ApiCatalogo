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
using X.PagedList;

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
        public async Task<ActionResult<IEnumerable<ProdutoDto>>> Get([FromQuery] ProdutosParameters produtosParameters)
        {

            var produtos = await _uof.ProdutosRepository.GetProdutosAsync(produtosParameters);

            return ObterProdutos(produtos);

        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoDto>>> GetProdutos()
        {
            var produtos = await _uof.ProdutosRepository.GetAllAsync();

            if(produtos == null)
            {
                return NotFound("Lista de produtos vazia");
            }

            var produtosDto = _mapper.Map<IEnumerable<ProdutoDto>>(produtos);
            return Ok(produtosDto); 
        }

        [HttpGet("Categoria/{id:int}")]

        public async Task<ActionResult<IEnumerable<ProdutoDto>>> GetProdutosPorCategoria(int id)
        {
            var produtos = await _uof.ProdutosRepository.GetProdutosPorCategoriaAsync(id);

            if (produtos == null) return NotFound();

            var produtosDto = _mapper.Map<IEnumerable<ProdutoDto>>(produtos);
            return Ok(produtosDto);
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetProdutoById")]
            public async Task<ActionResult<ProdutoDto>> GetProdutoById([FromRoute] int id)
        {
            var produto = await _uof.ProdutosRepository.GetAsync(p => p.Id == id);

            if (produto == null)
            {
                return NotFound("Produto não encontrado");
            }
            var produtoDto = _mapper.Map<ProdutoDto>(produto);

            return Ok(produtoDto);
        }

        [HttpPost]
        public async Task<ActionResult<ProdutoDto>> Post([FromBody] ProdutoDto produtoDto)
        {
            if (produtoDto == null) return BadRequest();

            var produto = _mapper.Map<Produto>(produtoDto); ;

            var novoProduto = _uof.ProdutosRepository.Create(produto);
            await _uof.CommitAsync();

            var novoProdutoDto = _mapper.Map<ProdutoDto>(novoProduto);


            return new CreatedAtRouteResult("GetProdutoById",
                new { id = novoProdutoDto.Id }, novoProdutoDto);
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<ProdutoDtoUpdateResponse>> Patch(int id,
                                               JsonPatchDocument<ProdutoDtoUpdateRequest> patchProdutoDto)
        {
            if (patchProdutoDto == null || id <= 0) { return BadRequest(); }

            var produto = await _uof.ProdutosRepository.GetAsync(p => p.Id == id);

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
            await _uof.CommitAsync();

            return Ok(_mapper.Map<ProdutoDtoUpdateResponse>(produto));
        }



        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<ProdutoDto>> Put([FromBody] ProdutoDto produtoDto, [FromRoute] int id)
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
            await _uof.CommitAsync();

            var produtoAtualizadoDto = _mapper.Map<ProdutoDto>(produtoAtualizado);
                  
            return Ok(produtoAtualizadoDto);
           
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<ProdutoDto>> Delete(int id)
        {
            var produto = await _uof.ProdutosRepository.GetAsync(p => p.Id == id);

            if(produto == null)
            {
                return NotFound("Categoria não encontrada");
            }
            var produtoDeletado = _uof.ProdutosRepository.Delete(produto);
            await _uof.CommitAsync();

            var produtoDeletadoDto = _mapper.Map<ProdutoDto>(produtoDeletado);

            return Ok(produtoDeletadoDto);
        }

        [HttpGet("filter/preco/pagination")]

        public async Task<ActionResult<IEnumerable<ProdutoDto>>> GetProdutoFilterPreco ([FromQuery] ProdutosFiltroPreco produtosFiltroPrecoParameters)
        {
            var produtos = await _uof.ProdutosRepository.GetProdutosFiltroPrecoAsync(produtosFiltroPrecoParameters);

            return ObterProdutos(produtos);

        }

        private ActionResult<IEnumerable<ProdutoDto>> ObterProdutos(IPagedList<Produto> produtos)
        {
            var metadata = new
            {
                produtos.Count,
                produtos.PageSize,
                produtos.PageCount,
                produtos.TotalItemCount,
                produtos.HasNextPage,
                produtos.HasPreviousPage
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

            var produtosDto = _mapper.Map<IEnumerable<ProdutoDto>>(produtos);

            return Ok(produtosDto);
        }
    }
}
