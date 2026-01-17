using PeliculasStudio.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace PeliculasStudio.BaseDatos
{
    public static class DatosIniciales
    {

        public static List<Pelicula> ObtenerListado()
        {
            return new List<Pelicula>
            {
                 new Pelicula {
                        Titulo = "Blade Runner 2049",
                        Anio = 2017,
                        Genero = "Ciencia Ficcion",
                      Resumen = @"En el año 2049, un nuevo blade runner...
                                    descubre un secreto oculto que podría
                                    cambiar el mundo.",
                        TrailerPath = "Blade Runner 2049.mp4",
                        PortadaPath = "Blade_Runner.jpg"
                    },
                    new Pelicula {
                        Titulo = "La Odisea",
                        Anio = 2026,
                        Genero = "Aventura",
                        Resumen = @"Una epopeya mitológica que sigue la historia de Odiseo
                                    y su largo viaje a casa, de 10 años de duración, tras la guerra de Troya.",
                        TrailerPath = "La Odisea.mp4",
                        PortadaPath = "Odisea.jpg"
                    },
                    new Pelicula {
                Titulo = "Expediente Warren: El último rito",
                Anio = 2021,
                Genero = "Terror",
                Resumen = @"Los investigadores de lo paranormal Ed y Lorraine Warren
                                se enfrentan a un último caso aterrador
                        en el que están implicadas entidades misteriosas a las que deben enfrentarse.",
                TrailerPath = "Expediente-Warren-El-ultimo-rito.mp4",
                PortadaPath = "Expediente_Warrem_El_Ultimo_Rito.jpg"
            },
                    new Pelicula {
                Titulo = "El Diario de Noa",
                Anio = 2004,
                Genero = "Romance",
                Resumen = @"En una residencia de ancianos, un hombre (James Garner) lee a una mujer (Gena Rowlands)
                            una historia de amor escrita en su viejo cuaderno de notas.
                        Es la historia de Noah Calhoun (Ryan Gosling) y Allie Hamilton (Rachel McAdams)
                    , dos jóvenes adolescentes de Carolina del Norte que, 
                    a pesar de vivir en dos ambientes sociales muy diferentes,
                    se enamoraron profundamente y pasaron juntos un verano inolvidable, antes de ser separados,
                    primero por sus padres, y más tarde por la guerra.",
                TrailerPath = "El Diario de Noa.mp4",
                PortadaPath = "EL_Diario_De_Noa.jpg"
            },

        };
        }
    }
}
