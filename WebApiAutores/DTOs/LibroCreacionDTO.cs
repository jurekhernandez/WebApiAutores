using System.ComponentModel.DataAnnotations;

using WebApiAutores.Validaciones;

namespace WebApiAutores.DTOs {
    public class LibroCreacionDTO {
        [PrimeraLetraMayusculaAttribute]
        [StringLength(maximumLength: 250)]
        public string Titulo { get; set; }
        public List<int> AutoresIds { get; set; }
    }
}
