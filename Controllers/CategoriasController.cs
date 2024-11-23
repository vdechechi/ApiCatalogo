using ApiCatalogo.Context;
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
    public IActionResult Get()
    {
        var cateogorias = _uof.CategoriasRepository.GetAll();

        return Ok(cateogorias);
    }

    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public IActionResult Get(int id)
    {
        var categoria = _uof.CategoriasRepository.Get(c => c.Id == id);

        if (categoria == null)
        {
            _logger.LogWarning($"Categoria com id= {id} não encontrada...");
            return NotFound($"Categoria com id= {id} não encontrada...");
        }
        return Ok(categoria);
    }

    [HttpPost]
    public IActionResult Post(Categoria categoria)
    {
        if (categoria is null)
        {
            _logger.LogWarning($"Dados inválidos...");
            return BadRequest("Dados inválidos");
        }

       var categoriaCriada = _uof.CategoriasRepository.Create(categoria);

        return new CreatedAtRouteResult("ObterCategoria", new { id = categoriaCriada.Id }, categoriaCriada);
    }

    [HttpPut("{id:int}")]
    public IActionResult Put(int id, Categoria categoria)
    {
        if (id != categoria.Id)
        {
            _logger.LogWarning($"Dados inválidos...");
            return BadRequest("Dados inválidos");
        }

        var categoriaAtualizada = _uof.CategoriasRepository.Update(categoria);

        return Ok(categoriaAtualizada);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {

        var categoria = _uof.CategoriasRepository.Get(c => c.Id == id);
        if(categoria == null)
        {
            _logger.LogWarning("Categoria nao encontrada");
            return NotFound("Categoria não encontrada");
        }

        var categoriaExcluida = _uof.CategoriasRepository.Delete(categoria);
        return Ok($"Categoria com id = {id} deletada com sucesso");
    }
}