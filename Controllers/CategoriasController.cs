using ApiCatalogo.Context;
using ApiCatalogo.Models;
using ApiCatalogo.Repositorys.Categorias;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace APICatalogo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriasRepository _repository;
    private readonly ILogger<CategoriasController> _logger;

    public CategoriasController(ILogger<CategoriasController> logger, ICategoriasRepository repository)
    {
        _repository = repository;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var cateogorias = _repository.GetCategorias();  

        return Ok(cateogorias);
    }

    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public IActionResult Get(int id)
    {
        var categoria = _repository.GetCategoria(id);

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

       var categoriaCriada = _repository.Create(categoria);

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

        var categoriaAtualizada = _repository.Update(categoria);

        return Ok(categoriaAtualizada);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    { 

        var categoriaExcluida = _repository.Delete(id);

        return Ok($"Categoria com id = {id} deletada com sucesso");
    }
}