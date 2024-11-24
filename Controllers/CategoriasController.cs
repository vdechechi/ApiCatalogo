using ApiCatalogo.Context;
using ApiCatalogo.DTO;
using ApiCatalogo.DTO.Mappings;
using ApiCatalogo.Models;
using ApiCatalogo.Repositorys.Categorias;
using ApiCatalogo.Repositorys.Generico;
using ApiCatalogo.Repositorys.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace APICatalogo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly ILogger<CategoriasController> _logger;

    public CategoriasController(ILogger<CategoriasController> logger, IUnitOfWork uof)
    {
        _logger = logger;
        _uof = uof;
    }

    [HttpGet]
    public ActionResult<IEnumerable<CategoriaDto>> Get()
    {
        var categorias = _uof.CategoriasRepository.GetAll();

        if(categorias is null)
        {
            return NotFound("Não existem categorias");
        }

        var categoriasDto = categorias.toCategoriaDtoList();

        return Ok(categoriasDto);
    }

    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public ActionResult<CategoriaDto> Get(int id)
    {
        var categoria = _uof.CategoriasRepository.Get(c => c.Id == id);

        if (categoria == null)
        {
            _logger.LogWarning($"Categoria com id= {id} não encontrada...");
            return NotFound($"Categoria com id= {id} não encontrada...");
        }
            
       var categoriaDto = categoria.ToCategoriaDto();

        return Ok(categoriaDto);
    }

    [HttpPost]
    public ActionResult<CategoriaDto> Post(CategoriaDto categoriaDto)
    {
        if (categoriaDto is null)
        {
            _logger.LogWarning($"Dados inválidos...");
            return BadRequest("Dados inválidos");
        }

        var categoria = categoriaDto.ToCategoria();

        var categoriaCriada = _uof.CategoriasRepository.Create(categoria);
        _uof.Commit();

        var novaCategoriaDto = categoriaCriada.ToCategoriaDto();

        return new CreatedAtRouteResult("ObterCategoria", new { id = novaCategoriaDto.Id }, novaCategoriaDto);
    }

    [HttpPut("{id:int}")]
    public ActionResult<CategoriaDto> Put(int id, CategoriaDto categoriaDto)
    {
        if (id != categoriaDto.Id)
        {
            _logger.LogWarning($"Dados inválidos...");
            return BadRequest("Dados inválidos");
        }
        var categoria = categoriaDto.ToCategoria();

        var categoriaAtualizada = _uof.CategoriasRepository.Update(categoria);
        _uof.Commit();

        var categoriaAtualizadaDto = categoriaAtualizada.ToCategoriaDto();

        return Ok(categoriaAtualizadaDto);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<CategoriaDto> Delete(int id)
    {

        var categoria = _uof.CategoriasRepository.Get(c => c.Id == id);
        if(categoria == null)
        {
            _logger.LogWarning("Categoria nao encontrada");
            return NotFound("Categoria não encontrada");
        }

        var categoriaExcluida = _uof.CategoriasRepository.Delete(categoria);

        var categoriaExcluidaDto = categoriaExcluida.ToCategoriaDto();

        return Ok(categoriaExcluidaDto);
    }
}