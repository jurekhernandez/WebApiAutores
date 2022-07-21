using AutoMapper;

using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using WebApiAutores.DTOs;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosController : ControllerBase
    {
        private readonly AplicationDbContext context;
        private readonly IMapper mapper;

        public LibrosController(AplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("{id:int}", Name ="obtenerLibro")]
        public async Task<ActionResult<LibroDTOConAutores>> Get(int id)
        {
            var libro = await context.Libros
                .Include(librosDB => librosDB.Comentarios)
                .Include(libroDb => libroDb.AutorLibro)
                .ThenInclude(autorLibroDb => autorLibroDb.autor)
                .FirstOrDefaultAsync(librosDB => librosDB.Id == id);

            if(libro == null) {
                return NotFound();
            }

            libro.AutorLibro = libro.AutorLibro.OrderBy(x => x.Orden).ToList();
            return mapper.Map<LibroDTOConAutores>(libro);
        }

        [HttpPost]
        public async Task<ActionResult> Post(LibroCreacionDTO libroCreacionDTO) {
            if(libroCreacionDTO.AutoresIds== null) {
                return BadRequest("No se puede crear libros sin autores");
            }

            var autoresIds = await context.Autores.Where(AutoresDb => libroCreacionDTO.AutoresIds.Contains(AutoresDb.Id)).Select(autorDB => autorDB.Id).ToListAsync();

            if(autoresIds.Count != libroCreacionDTO.AutoresIds.Count) {
                return BadRequest("Uno o varios autores enviados no existen en nuestro sistema");
            }

            var libro = mapper.Map<Libro>(libroCreacionDTO);
            this.AsignarOrdenAutores(libro);
            context.Add(libro);
            await context.SaveChangesAsync();

            var libroDTO = mapper.Map<LibroDTO>(libro);
            return CreatedAtRoute("obtenerLibro", new {id = libro.Id }, libroDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, LibroCreacionDTO libroCreacionDTO){
            var libroDB = await context.Libros.Include(x => x.AutorLibro).FirstOrDefaultAsync(y => y.Id == id);
            if(null == libroDB) { 
                return NotFound();
            }
            //libroDB = mapper.Map<Libro>(libroCreacionDTO);
            libroDB = mapper.Map(libroCreacionDTO, libroDB);
            AsignarOrdenAutores(libroDB);
            await context.SaveChangesAsync();
            return NoContent();
            
        }

        private void AsignarOrdenAutores(Libro libro) {
            if(libro.AutorLibro != null) {
                for(int i = 0; i < libro.AutorLibro.Count; i++) {
                    libro.AutorLibro[i].Orden = i;
                }
            }
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<LibroPatchDTO> patchDocument) {
            if(patchDocument == null) {
                return BadRequest("algo mal");
            }
            var libroDB = await context.Libros.FirstOrDefaultAsync(x => x.Id == id);
            if(libroDB == null) {
                return NotFound("no existe");
            }
            var libroDTO = mapper.Map<LibroPatchDTO>(libroDB);
            patchDocument.ApplyTo(libroDTO, ModelState);

            var esValido = TryValidateModel(libroDTO);
            if(!esValido) {
                return BadRequest(ModelState);
            }

            mapper.Map(libroDTO, libroDB);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id) {
            var existe = await context.Libros.AnyAsync(x => x.Id == id);
            if(!existe) {
                return NotFound();
            }
            context.Remove(new Libro() { Id = id });
            await context.SaveChangesAsync();
            return Ok();
        }

    }
}
