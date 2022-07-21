using System.ComponentModel.DataAnnotations;

using WebApiAutores.Validaciones;

namespace WebApiAutores.DTOs {
    public class AutorCreacionDTO {

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(120, ErrorMessage = "el campo {0} no puede tener mas de {1} carácteres")]
        [MinLength(5, ErrorMessage = "El campo {0}  no puede tener menos de {1} carácteres")]
        [PrimeraLetraMayusculaAttribute]
        public string nombre { get; set; }

    }
}
