using ApiCatalogo.Context;
using Microsoft.AspNetCore.Mvc;

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





    }
}
