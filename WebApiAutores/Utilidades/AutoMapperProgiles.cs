using AutoMapper;

using WebApiAutores.DTOs;
using WebApiAutores.Entidades;

namespace WebApiAutores.Utilidades {
    public class AutoMapperProgiles:Profile {

        public AutoMapperProgiles() {
            CreateMap<AutorCreacionDTO, Autor>();
            CreateMap<Autor, AutorDTO>();
            CreateMap<Autor, AutorDTOConLibro>().ForMember(autorDTO => autorDTO.Libros, opciones => opciones.MapFrom(MapAutorDTOLibros));

            CreateMap<LibroCreacionDTO, Libro>().ForMember(libro => libro.AutorLibro, opciones => opciones.MapFrom(MapAutoresLibros));
            CreateMap<Libro, LibroDTO>();
            CreateMap<Libro, LibroDTOConAutores>().ForMember(libroDTO => libroDTO.Autores, opciones => opciones.MapFrom(MapLibroDTOAutores));
            CreateMap<LibroPatchDTO, Libro>().ReverseMap();

            CreateMap<ComentarioCreacionDTO, Comentario>();
            CreateMap<Comentario, ComentarioDTO>();

        }

        private List<LibroDTO> MapAutorDTOLibros(Autor autor, AutorDTO autorDTO) {
            var resultado = new List<LibroDTO>();

            if(autor.AutorLibro == null) {
                return resultado;
            }
            foreach(var autorLibro in autor.AutorLibro) {
                resultado.Add(new LibroDTO() { 
                    Id = autorLibro.libro.Id,
                    Titulo = autorLibro.libro.Titulo
                });
            }

            return resultado;
        }

        private List<AutorDTO> MapLibroDTOAutores(Libro libro, LibroDTOConAutores libroDTO) {
            var resultado = new List<AutorDTO>();

            if(libro.AutorLibro == null) { return resultado; }

            foreach(var autorLibro in libro.AutorLibro) {
                resultado.Add(new AutorDTO(){
                    Id = autorLibro.AutorId,
                    Nombre = autorLibro.autor.Nombre
                });
            }
            return resultado;
        }

        private List<AutorLibro> MapAutoresLibros(LibroCreacionDTO libroCreacionDTO, Libro libro) {
            var resultado = new List<AutorLibro>();
            if(libroCreacionDTO.AutoresIds == null) {
                return resultado;
            }

            foreach(var autorId in libroCreacionDTO.AutoresIds) {
                resultado.Add(new AutorLibro() {
                    AutorId = autorId
                });
            }
            return resultado;

        }



    }
}

