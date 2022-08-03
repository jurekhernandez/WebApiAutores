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
        private readonly IAuthorizationService authorizationService;

        public AutoresController(AplicationDbContext context, IMapper mapper, IAuthorizationService authorizationService) {
            this.context = context;
            this.mapper = mapper;
            this.authorizationService = authorizationService;
        }

        [HttpGet(Name ="obtenerAutores")] // api/autores
        [AllowAnonymous]
        public async Task<ActionResult<AutorDTO>> Get([FromQuery]bool incluirHATEOAS =true) {
            var autores = await context.Autores.ToListAsync();
            var dtos= mapper.Map<List<AutorDTO>>(autores);
            if(incluirHATEOAS) {
                var esAdmin = await authorizationService.AuthorizeAsync(User, "esAdmin");
                dtos.ForEach(dto => GenerarEnlaces(dto, esAdmin.Succeeded));

                var resultado = new ColeccionDeRecursos<AutorDTO> { Valores = dtos };
                resultado.Enlaces.Add(new DatoHATEOAS(enlace: Url.Link("obtenerAutores", new{ }), descripcion:"self", metodo:"GET"));
                if(esAdmin.Succeeded) { 
                    resultado.Enlaces.Add(new DatoHATEOAS(enlace: Url.Link("crearAutor", new { }), descripcion: "crear-autor", metodo: "POST"));
                }
                return Ok(resultado);
            }
            return Ok(dtos);

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
            var dto= mapper.Map<AutorDTOConLibro>(autor);
            var esAdmin = await authorizationService.AuthorizeAsync(User, "esAdmin");
            GenerarEnlaces(dto, esAdmin.Succeeded);
            return dto;
        }

        private void GenerarEnlaces(AutorDTO autorDTO, bool esAdmin) {
            autorDTO.Enlaces.Add(new DatoHATEOAS(enlace: Url.Link("obtenerAutor", new { id=autorDTO.Id }), descripcion:"self", metodo:"GET"));
            if(esAdmin) {
                autorDTO.Enlaces.Add(new DatoHATEOAS(enlace: Url.Link("actualizarAutor", new { id = autorDTO.Id }), descripcion: "autor-actualizar", metodo: "PUT"));
                autorDTO.Enlaces.Add(new DatoHATEOAS(enlace: Url.Link("borrarAutor", new { id = autorDTO.Id }), descripcion: "autor-borrar", metodo: "DELETE"));
            }
        }

        [HttpGet("{nombre}",Name ="obtenerAutorPorNombre")]
        public async Task<ActionResult<List<AutorDTO>>> Buscando([FromRoute] string nombre){
            var autores = await context.Autores.Where(autorDB => autorDB.Nombre.Contains(nombre)).ToListAsync();
            return mapper.Map<List<AutorDTO>>(autores);
        }

        [HttpPost(Name ="crearAutor")]
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

        [HttpPut("{id:int}", Name ="actualizarAutor")]
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

        [HttpDelete("{id:int}",Name ="borrarAutor")]
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
