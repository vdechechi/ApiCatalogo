using ApiCatalogo.Context;
using ApiCatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ApiCatalogo.Controllers
{
    [Route("api/categorias")] 
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> GetCategorias()
        {
            var categorias = _context.Categorias.AsNoTracking().ToList();

            if (!categorias.Any())
            {
                return NotFound("Nenhuma categoria encontrada.");
            }

            return categorias;
        }

        [HttpGet]
        [Route("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            var categorias = _context.Categorias.AsNoTracking().Include(p=> p.Produtos).ToList();

            if (!categorias.Any())
            {
                return NotFound("Nenhuma categoria encontrada.");
            }

            return categorias;
        }


        [HttpGet]
        [Route("{id:int}", Name = "GetCategoriaById")]
        public ActionResult<Categoria> GetById(int id)
        {
            var categoria = _context.Categorias.FirstOrDefault(c => c.Id == id);

            if (categoria == null)
            {
                return NotFound("Categoria não encontrada.");
            }

            return Ok(categoria);
        }

        [HttpPost]
        public ActionResult<Categoria> Post([FromBody] Categoria categoria)
        {
            if (categoria == null) return BadRequest();

            _context.Categorias.Add(categoria);

            _context.SaveChanges();

            return new CreatedAtRouteResult("GetCategoriaById",
                new { id = categoria.Id }, categoria);
        }

        [HttpPut]
        [Route("{id:int}")]
        public ActionResult<Categoria> Put([FromBody] Categoria categoria, [FromRoute] int id)
        {
            if (categoria == null) { return NotFound(); }
            else
            {
                if (categoria.Id != id)
                {
                    return BadRequest("Categoria nao encontrado");
                }

                _context.Entry(categoria).State = EntityState.Modified;
                _context.SaveChanges();

                return Ok(categoria);
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public ActionResult Delete(int id)
        {
            var categoria = _context.Categorias.FirstOrDefault(x => x.Id == id);

            if (categoria == null)
            {
                return NotFound("Produto não encontrado");
            }

            _context.Categorias.Remove(categoria);

            _context.SaveChanges();

            return Ok("Categoria Deletado com sucesso");

        }
    }
}

