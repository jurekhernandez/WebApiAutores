using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosController : ControllerBase
    {
        private readonly AplicationDbContext context;

        public LibrosController(AplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Libro>> Get(int id)
        {
            return await context.Libros.Include(x => x.Autor).FirstOrDefaultAsync(x => x.id == id);

        }

        [HttpPost]
        public async Task<ActionResult> Post(Libro libro){
            var existeAutor = await context.Autores.AnyAsync(x => x.id == libro.autorid);

            if (!existeAutor) {
                return BadRequest($"no existe el wn {libro.autorid }");
            }
            context.Add(libro);
            await context.SaveChangesAsync();
            return Ok();
        }


    }
}
