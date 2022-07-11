﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiAutores.Validaciones;

namespace WebApiAutores.Entidades
{
    public class Autor{
        public int Id { get; set; }       
        [Required(ErrorMessage ="El campo {0} es requerido")]
        [StringLength(maximumLength:120, ErrorMessage ="el campo {0} no puede tener mas de {1} carácteres")]
        [PrimeraLetraMayusculaAttribute]
        public string Nombre { get; set; }
        public List<AutorLibro> AutorLibro { get; set; }


    }
}
