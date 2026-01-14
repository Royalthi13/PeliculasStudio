using SQLite;
using System;
using System.IO;
using System.Collections.Generic;
using PeliculasStudio.Modelos;
namespace PeliculasStudio.BaseDatos
{

    public static class DatabaseServicie
    {
        private static SQLiteConnection db;

        public static void Inicializar()
        {
            if (db != null) return;

            // Esta ruta  es la mejor manera para que funcione en Windows, Linux y Mac sustituye a db = new SQLiteConnection("PeliculasStudio.db");
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PeliculasStudio.db");
            db = new SQLiteConnection(dbPath);

           
            db.CreateTable<Pelicula>();
            db.CreateTable<Usuario>();

            IntegrarDatos();
        }
        private static void IntegrarDatos()
        {
            if (db.Table<Usuario>().Count() == 0)
            {
                db.Insert(new Usuario
                {
                    Nombreusuario = "admin",
                    Contrasenia = "123",
                    Rol = "Admin"
                });
            }

            if (db.Table<Pelicula>().Count() == 0)
            {
                var iniciales = new List<Pelicula>
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
                db.InsertAll(iniciales);
            }

     

    }

        public static SQLiteConnection GetConexion()
        {
            return db;
        }

    }
}
