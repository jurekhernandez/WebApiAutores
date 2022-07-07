using System.ComponentModel.DataAnnotations;

using WebApiAutores.Validaciones;

namespace WebApiAutores.Entidades
{
    public class Libro
    {
        public int id { get; set; }
        [PrimeraLetraMayusculaAttribute]
        [StringLength(maximumLength:250)]
        public string titulo { get; set; }
    }
}
