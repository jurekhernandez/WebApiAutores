using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using WebApiAutores.DTOs;
using WebApiAutores.Entidades;
using WebApiAutores.Filtros;


namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/autores")]
    // [Authorize] // de esta forma cuido todas las rutas del controlador
    public class AutoresController : ControllerBase {
        private readonly AplicationDbContext context;
        private readonly IMapper mapper;

        public AutoresController(AplicationDbContext context, IMapper mapper) {
            this.context = context;
            this.mapper = mapper;
        }
     
        [HttpGet] // api/autores
        public async Task<ActionResult<List<AutorDTO>>> Get() {
           var autores = await context.Autores.ToListAsync();

        return mapper.Map<List<AutorDTO>>(autores);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AutorDTO>> Buscando(int id) { 
            Autor autor = await context.Autores.FirstOrDefaultAsync(autorDB => autorDB.Id==id);
            if (autor == null) {
                return NotFound();
            }

            return mapper.Map<AutorDTO>(autor);
        }

        [HttpGet("{nombre}")]
        public async Task<ActionResult<List<AutorDTO>>> Buscando([FromRoute] string nombre)
        {
            var autores = await context.Autores.Where(autorDB => autorDB.Nombre.Contains(nombre)).ToListAsync();

            return mapper.Map<List<AutorDTO>>(autores);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AutorCreacionDTO autorCreacionDto) {

            var existeAutor = await context.Autores.AnyAsync(x => x.Nombre == autorCreacionDto.nombre);
            if (existeAutor) {
                return BadRequest($"Ya existe este autor {autorCreacionDto.nombre}");
            }

            Autor autor= mapper.Map<Autor>(autorCreacionDto);
            context.Add(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Autor autor, int id) {
            if (autor.Id != id) {
                return BadRequest("El id del autor no coincide con el id de la url");
            }

            var existe = await context.Autores.AnyAsync(x => x.Id == id);
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
            var existe =await context.Autores.AnyAsync(x => x.Id == id);
            if (!existe){
                return NotFound();
            }
            context.Remove(new Autor() { Id = id });
            await context.SaveChangesAsync();
            return Ok();
        }


    }
}
