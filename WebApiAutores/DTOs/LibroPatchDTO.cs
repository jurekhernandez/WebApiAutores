using System.ComponentModel.DataAnnotations;

using WebApiAutores.Validaciones;

namespace WebApiAutores.DTOs {
    public class LibroPatchDTO {
        [PrimeraLetraMayusculaAttribute]
        [StringLength(maximumLength: 250)]
        [Required]
        public string Titulo { get; set; }
        public DateTime FechaPublicacion { get; set; }
    }
}
