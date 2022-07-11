using AutoMapper;

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

        [HttpGet("{id:int}")]
        public async Task<ActionResult<LibroDTO>> Get(int id)
        {
            var libro = await context.Libros.Include(librosDB => librosDB.Comentarios).FirstOrDefaultAsync(librosDB => librosDB.Id == id);
            return mapper.Map<LibroDTO>(libro);
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

            if(libro.AutorLibro != null) {
                for(int i = 0; i < autoresIds.Count; i++) {
                    libro.AutorLibro[i].Orden = i;
                }
            }
            context.Add(libro);
            await context.SaveChangesAsync();
            return Ok();
        }


    }
}
