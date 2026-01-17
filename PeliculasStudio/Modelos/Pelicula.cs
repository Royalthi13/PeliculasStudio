using SQLite;

namespace PeliculasStudio.Modelos
{
   public  class Pelicula
    {
        [PrimaryKey , AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public string Titulo { get; set; }

        [NotNull]
        public string Resumen { get; set; }

        [NotNull]
        public string Genero { get; set; }

        [NotNull]

        public  int Anio { get; set; }

        [NotNull]
        public string TrailerPath { get; set; }

        [NotNull]
        public string PortadaPath { get; set; }

        [NotNull]
        public int CantVisualizaciones { get; set; } = 0;



    }
}
