namespace UltimoParcialDesarolloWebChristianB.Models
{
    public class Pelicula
    {
        public int PeliculaID { get; set; }
        public string Titulo { get; set; }
        public string Sinopsis { get; set; }
        public int CineID { get; set; }
        public int GeneroID { get; set; }
        public DateTime FechaEstreno { get; set; }
    }
}
