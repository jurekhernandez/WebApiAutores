using AutoMapper;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<IdentityUser> userManager;

        public ComentariosController(AplicationDbContext context, IMapper mapper, UserManager<IdentityUser> userManager) {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
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

        [HttpGet("id:int", Name = "ObtenerComentario")]
        public async Task<ActionResult<ComentarioDTO>> GetById(int Id) {
            var comentario = await context.Comentarios.FirstOrDefaultAsync(comentarioDb => comentarioDb.Id.Equals(Id));

            if(comentario == null) {
                return NotFound("No existe");
            }
            return mapper.Map<ComentarioDTO>(comentario);             
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post(int libroId, ComentarioCreacionDTO comentarioCreacionDTO) {
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;
            var usuario = await userManager.FindByEmailAsync(email);
            var usuarioId = usuario.Id;

            var existeLibro = await this.context.Libros.AnyAsync(libroDB => libroDB.Id == libroId);
            if(!existeLibro) {
                return NotFound("No existe el libro");
            }
            var comentario = mapper.Map<Comentario>(comentarioCreacionDTO);
            comentario.LibroId = libroId;
            comentario.UsuarioId = usuarioId;
            this.context.Add(comentario);
            await this.context.SaveChangesAsync();
            var comentarioDTO = mapper.Map<ComentarioDTO>(comentario);
            return CreatedAtRoute("ObtenerComentario", new {id = comentario.Id, libroId = libroId }, comentarioDTO);
        }
    }
}
