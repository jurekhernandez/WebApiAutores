using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiAutores.Validaciones;

namespace WebApiAutores.Entidades
{
    public class Autor: IValidatableObject{
        public int id { get; set; }
       
        [Required(ErrorMessage ="El campo {0} es requerido")]
        [StringLength(maximumLength:120, ErrorMessage ="el campo {0} no puede tener mas de {1} carácteres")]
        [PrimeraLetraMayusculaAttribute]
        public string nombre { get; set; }
       
       // [Range(18, 120)]
       // [NotMapped]
       // public int edad { get; set; }
       // 
       // [CreditCard]
       // [NotMapped]
       // public string credit { get; set; }
       // 
       // [Url]
       // [NotMapped]
       // public string url { get; set; }

      //  public int menor { get; set; }
      //  public int mayor { get; set; }
        public List<Libro> libros { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(nombre)) {
                var primeraLetra = nombre[0].ToString();
                if (primeraLetra != primeraLetra.ToUpper()) {
                    yield return new ValidationResult("La primera letra debe ser mayuscula", new string[] { nameof(nombre) });
                }
            }
          //  if (menor > mayor) {
          //      yield return new ValidationResult("Este valor no puede ser mayor que el campo mayor ", new string[] { nameof(menor)});
          //  }

        }
    }
}
