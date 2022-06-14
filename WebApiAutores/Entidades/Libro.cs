using WebApiAutores.Validaciones;

namespace WebApiAutores.Entidades
{
    public class Libro
    {
        public int id { get; set; }
        [PrimeraLetraMayusculaAttribute]
        public string titulo { get; set; }
        public int autorid { get; set; }
        public Autor Autor { get; set; } 
    }
}
