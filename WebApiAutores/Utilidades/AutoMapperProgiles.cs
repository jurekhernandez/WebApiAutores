using AutoMapper;

using WebApiAutores.DTOs;
using WebApiAutores.Entidades;

namespace WebApiAutores.Utilidades {
    public class AutoMapperProgiles: Profile {

        public AutoMapperProgiles() {
            CreateMap<AutorCreacionDTO, Autor>();
        }
    }
}
