using AutoMapper;

using Microsoft.AspNetCore.Authentication.JwtBearer;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    // [Authorize] // de esta forma cuido todas las rutas del controlador
    public class AutoresController : ControllerBase {
        private readonly AplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public AutoresController(AplicationDbContext context, IMapper mapper, IConfiguration configuration) {
            this.context = context;
            this.mapper = mapper;
            this.configuration = configuration;
        }
       // [HttpGet("configuracion")]
       // public ActionResult<string> ObtenerConfiguracion() {
       //     return this.configuration["apellido"];
       // 
       // }
     
        [HttpGet] // api/autores
        [AllowAnonymous]
        public async Task<ActionResult<List<AutorDTO>>> Get() {
            var autores = await context.Autores.ToListAsync();
            return mapper.Map<List<AutorDTO>>(autores);
        }


        [HttpGet("{id:int}", Name ="obtenerAutor")]
        public async Task<ActionResult<AutorDTOConLibro>> Buscando(int id) { 
            Autor autor = await context.Autores
                .Include(autorDb => autorDb.AutorLibro)
                .ThenInclude(autorLibroDb => autorLibroDb.libro)                
                .FirstOrDefaultAsync(autorDB => autorDB.Id==id);
            if (autor == null) {
                return NotFound();
            }
            return mapper.Map<AutorDTOConLibro>(autor);
        }

        [HttpGet("{nombre}")]
        public async Task<ActionResult<List<AutorDTO>>> Buscando([FromRoute] string nombre){
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

            var autorDTO = mapper.Map<AutorDTO>(autor);
            return CreatedAtRoute("obtenerAutor", new { id = autor.Id }, autorDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(AutorCreacionDTO autorCreacionDTO, int id) {


            var existe = await context.Autores.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound();
            }

            var autor = mapper.Map<Autor>(autorCreacionDTO);
            autor.Id = id;
            context.Update(autor);
            await context.SaveChangesAsync();
            return NoContent();

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
