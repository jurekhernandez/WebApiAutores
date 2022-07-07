﻿using System.ComponentModel.DataAnnotations;

using WebApiAutores.Validaciones;

namespace WebApiAutores.DTOs {
    public class AutorCreacionDTO {

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 120, ErrorMessage = "el campo {0} no puede tener mas de {1} carácteres")]
        [PrimeraLetraMayusculaAttribute]
        public string nombre { get; set; }

    }
}
