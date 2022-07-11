﻿using AutoMapper;

using WebApiAutores.DTOs;
using WebApiAutores.Entidades;

namespace WebApiAutores.Utilidades {
    public class AutoMapperProgiles: Profile {

        public AutoMapperProgiles() {
            CreateMap<AutorCreacionDTO, Autor>();
            CreateMap<Autor, AutorDTO>();

            CreateMap<LibroCreacionDTO, Libro>().ForMember(libro => libro.AutorLibro, opciones => opciones.MapFrom(MapAutoresLibros));
            CreateMap<Libro, LibroDTO>();

            CreateMap<ComentarioCreacionDTO, Comentario>();
            CreateMap<Comentario, ComentarioDTO>();
           
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

