using ApiCatalogo.Context;
using ApiCatalogo.DTO;
using ApiCatalogo.DTO.Mappings;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using ApiCatalogo.Repositorys.Categorias;
using ApiCatalogo.Repositorys.Generico;
using ApiCatalogo.Repositorys.UnitOfWork;
using APICatalogo.DTOs.Mappings;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using X.PagedList;

namespace APICatalogo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;
    public CategoriasController(IMapper mapper, IUnitOfWork uof)
    {
        _mapper = mapper;
        _uof = uof;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoriaDto>>> Get()
    {
        var categorias = await _uof.CategoriasRepository.GetAllAsync();

        if(categorias is null)
        {
            return NotFound("Não existem categorias");
        }

        var categoriasDto = _mapper.Map<IEnumerable<CategoriaDto>>(categorias);

        return Ok(categoriasDto);
    }

    [HttpGet("pagitanion")]
    public async Task<ActionResult<IEnumerable<CategoriaDto>>> GetCategorias([FromQuery] CategoriasParameters categoriasParameters)
    {
        var categorias = await _uof.CategoriasRepository.GetCategoriasAsync(categoriasParameters);

        if (categorias is null)
        {
            return NotFound("Não existem categorias");
        }

        return ObterCategorias(categorias);
    }

    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public async Task<ActionResult<CategoriaDto>> Get(int id)
    {
        var categoria = await _uof.CategoriasRepository.GetAsync(c => c.Id == id);

        if (categoria == null)
        {
            return NotFound($"Categoria com id= {id} não encontrada...");
        }

        var categoriaDto = _mapper.Map<CategoriaDto>(categoria);

        return Ok(categoriaDto);
    }

    [HttpPost]
    public async Task<ActionResult<CategoriaDto>> Post(CategoriaDto categoriaDto)
    {
        if (categoriaDto is null)
        {
            return BadRequest("Dados inválidos");
        }

        var categoria = _mapper.Map<Categoria>(categoriaDto);

        var novaCategoria = _uof.CategoriasRepository.Create(categoria);
        await _uof.CommitAsync();

        var novaCategoriaDto = _mapper.Map<CategoriaDto>(novaCategoria);

        return new CreatedAtRouteResult("ObterCategoria", new { id = novaCategoriaDto.Id }, novaCategoriaDto);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CategoriaDto>> Put(int id, CategoriaDto categoriaDto)
    {
        if (categoriaDto is null)
        {
            return BadRequest("Dados inválidos");
        }

        var categoria = _mapper.Map<Categoria>(categoriaDto);

        var categoriaAtualizada = _uof.CategoriasRepository.Update(categoria);
        await _uof.CommitAsync();

        var novaCategoriaDto = _mapper.Map<CategoriaDto>(categoriaAtualizada);

        return Ok(categoriaAtualizada);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<CategoriaDto>> Delete(int id)
    {

        var categoria = await _uof.CategoriasRepository.GetAsync(c => c.Id == id);
        if(categoria == null)
        {
            return NotFound("Categoria não encontrada");
        }

        var categoriaExcluida = _uof.CategoriasRepository.Delete(categoria);
        await _uof.CommitAsync();

        var categoriaExcluidaDto = _mapper.Map<CategoriaDto>(categoriaExcluida);

        return Ok(categoriaExcluidaDto);
    }

    [HttpGet("filter/nome/pagination")]

    public async Task<ActionResult<IEnumerable<CategoriaDto>>> GetCategoriasFiltradas( [FromQuery] CategoriasFiltroNome categoriasFiltro)
    {
        var categoriasFiltradas = await _uof.CategoriasRepository
                                     .GetCategoriasFiltroNomeAsync(categoriasFiltro);

        return ObterCategorias(categoriasFiltradas);

    }

    private ActionResult<IEnumerable<CategoriaDto>> ObterCategorias(IPagedList<Categoria> categorias)
    {
        var metadata = new
        {
            categorias.Count,
            categorias.PageSize,
            categorias.PageCount,
            categorias.TotalItemCount,
            categorias.HasNextPage,
            categorias.HasPreviousPage
        };

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
        var categoriasDto = categorias.ToCategoriaDTOList();
        return Ok(categoriasDto);
    }
}