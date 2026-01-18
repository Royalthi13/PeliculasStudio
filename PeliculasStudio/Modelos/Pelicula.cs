using SQLite;
using System.IO;

namespace PeliculasStudio.Modelos
{
    public enum GeneroPelicula
    {
        Accion,
        Aventura,
        CienciaFiccion,
        Drama,
        Terror,
        Romance,
        Western,
        Comedia,
        Thriller,
        Fantasía, 
        Crimen,
        Animacion
    }
    public  class Pelicula
    {
        [PrimaryKey , AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public string Titulo { get; set; }

        [NotNull]
        public string Resumen { get; set; }

        [NotNull]
        public GeneroPelicula Genero { get; set; }

        [NotNull]

        public  int Anio { get; set; }

        [NotNull]
        public string TrailerPath { get; set; }

        [NotNull]
        public string PortadaPath { get; set; }

        [NotNull]
        public int CantVisualizaciones { get; set; } = 0;


        [Ignore] // Le decimos a la base de datos que ignore esto, es solo visual
        public string RutaImagenCompleta
        {
            get
            {
                if (string.IsNullOrEmpty(PortadaPath)) return null;
                // Combina la carpeta donde está el .exe + Assets + Imagenes + NombreArchivo
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Imagenes", PortadaPath);
            }
        }
    }
}
