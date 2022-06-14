using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/autores")]
    public class AutoresController : ControllerBase {
        private readonly AplicationDbContext context;
        /* 
         * 
         * Los datos pueden venir desde distintos lugares como 
         * FromBody = formulario,  
         * FromRoute = desde url   api/autores/Jurek
         * FromQuery = parametro de url   api/autores?nombre=Jurek&otro=algo
         * */
        public AutoresController(AplicationDbContext context) {
            this.context = context;
        } 

        [HttpGet] // api/autores
        [HttpGet("listado")] // api/autores/listado
        [HttpGet("/listado")] // /listado
        public async Task<ActionResult<List<Autor>>> Get() {
            return await context.Autores.Include( autor => autor.libros).ToListAsync();
        }

        [HttpGet("primero")]
        public async Task<ActionResult<Autor>> PrimerAutor() { 
            return await context.Autores.FirstOrDefaultAsync();
        }

        [HttpGet("{id:int}/{variable1=valor}/{variable2?}")]
        public async Task<ActionResult<Autor>> Buscando(int id, string variable1, string variable2) { 
            Autor autor = await context.Autores.FirstOrDefaultAsync(x => x.id==id);
            if (autor == null) {
                return NotFound();
            }
            return Ok(autor);
        }

        [HttpGet("{nombre}")]
        public async Task<ActionResult<Autor>> Buscando([FromRoute] string nombre)
        {
            Autor autor = await context.Autores.FirstOrDefaultAsync(x => x.nombre.Contains(nombre));
            if (autor == null)
            {
                return NotFound();
            }
            return Ok(autor);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Autor autor) {

            var existeAutor = await context.Autores.AnyAsync(x => x.nombre == autor.nombre);
            if (existeAutor) {
                return BadRequest($"Ya existe este autor {autor.nombre}");
            }
            context.Add(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Autor autor, int id) {
            if (autor.id != id) {
                return BadRequest("El id del autor no coincide con el id de la url");
            }

            var existe = await context.Autores.AnyAsync(x => x.id == id);
            if (!existe)
            {
                return NotFound();
            }


            context.Update(autor);
            await context.SaveChangesAsync();
            return Ok();

        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id) { 
            var existe =await context.Autores.AnyAsync(x => x.id == id);
            if (!existe){
                return NotFound();
            }
            context.Remove(new Autor() { id = id });
            await context.SaveChangesAsync();
            return Ok();
        }


    }
}
