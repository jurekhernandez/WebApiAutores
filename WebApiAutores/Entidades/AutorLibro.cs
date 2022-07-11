namespace WebApiAutores.Entidades {
    public class AutorLibro {

        public int LibroId { set; get; }
        public int AutorId { set; get; }
        public int Orden { set; get; }
        public Libro libro { set; get; }
        public Autor autor { set; get; }
    }
}
