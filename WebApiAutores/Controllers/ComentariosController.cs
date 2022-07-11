using AutoMapper;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using WebApiAutores.DTOs;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers {
    [ApiController]
    [Route("api/libros/{libroId:int}/comentarios")]
    public class ComentariosController: ControllerBase {
        private readonly AplicationDbContext context;
        private readonly IMapper mapper;

        public ComentariosController(AplicationDbContext context, IMapper mapper) {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ComentarioDTO>>> Get(int libroId) {
            var existeLibro = await this.context.Libros.AnyAsync(libroDB => libroDB.Id == libroId);
            if(!existeLibro) {
                return NotFound("No existe el libro");
            }
            var comentarios = await context.Comentarios.Where(comentariosDB => comentariosDB.LibroId == libroId).ToListAsync();
            // return Ok(comentarios);
            return mapper.Map<List<ComentarioDTO>>(comentarios);
        }

        [HttpPost]
        public async Task<ActionResult> Post(int libroId, ComentarioCreacionDTO comentarioCreacionDTO) {
            var existeLibro = await this.context.Libros.AnyAsync(libroDB => libroDB.Id == libroId);
            if(!existeLibro) {
                return NotFound("No existe el libro");
            }
            var comentario = mapper.Map<Comentario>(comentarioCreacionDTO);
            comentario.LibroId = libroId;
            this.context.Add(comentario);
            await this.context.SaveChangesAsync();
            return Ok("Comentario guardado");
        }
    }
}
