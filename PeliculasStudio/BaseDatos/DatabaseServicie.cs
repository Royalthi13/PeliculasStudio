using PeliculasStudio.Modelos;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
namespace PeliculasStudio.BaseDatos
{
    /**
     * Clase DatabaseServicie:
     * Actúa como la capa de acceso a datos (DAL) de la aplicación.
     * Se encarga de la gestión del ciclo de vida de la base de datos SQLite, 
     * incluyendo la inicialización de tablas, la inserción de datos semilla, 
     * y la ejecución de operaciones CRUD (Crear, Leer, Eliminar) para las 
     * entidades de Películas y Usuarios.
     **/

    public static class DatabaseServicie
    {
        private static SQLiteConnection db;
        /**
         * Metodo para Inicializar la aplicacion con la BBDD
         * BBDD Peliculas: Contendra la clase Pelicula (Titulo,Año,Genero,Resumen,RutaVideo,RutaLogo
         * BBDD Usuario: Nombre, Contraseña,Rol (Proponer meter gmail, y una ID para cada usuario, asi permitir nombres iguales)
         * **/
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

        /**
         * Metodo que Ingresa los datos Iniciales, para arrancar con algo de chicha
         * **/
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


        /**
         * Metodo de crear Usuario
         * Funcion del metodo, recoger las variables de la Clase Usuario e introduccirlos en la BBDD Usuario
         * Recoge: nombre, contraseña y rol
         ***/
        public static string CrearUsuario(string nombre, string password, string rol = "Usuario")
        {
            try
            {
                if (db == null) Inicializar();
                var usuarioExistente = db.Table<Usuario>()
                                         .FirstOrDefault(u => u.Nombreusuario.ToLower() == nombre.ToLower());

                if (usuarioExistente != null)
                {
                    return "El nombre de usuario ya existe.";
                }       
                var nuevoUsuario = new Usuario
                {
                    Nombreusuario = nombre,
                    Contrasenia = password,
                    Rol = rol
                };            
                db.Insert(nuevoUsuario);
                return 
                       $"Usuario: {nuevoUsuario.Nombreusuario}\n" +
                       $"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}";
            }
            catch (Exception ex)
            {
                return "Error al guardar: " + ex.Message;
            }
        }

        /**
        * Metodo Registrar Usuario:
        * Valida los datos de entrada e inserta un nuevo usuario con ID autogenerado.
        * @param nombre: Nickname del usuario.
        * @param gmail: Correo electrónico (se valida formato).
        * @param password: Clave de acceso.
        * @param rol: "Admin" o "Usuario" (por defecto "Usuario").
        * @return: Envia un Mensaje de como ha sido.
        **/
        /*
        public static string CrearUsuario(string nombre, string gmail, string password, string rol = "Usuario")
        {
            try
            {
                if (db == null) Inicializar();

                // 1. Validar formato de Gmail
                string patronEmail = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(gmail, patronEmail))
                {
                    return "ERROR: El formato del correo electrónico no es válido.";
                }

                // 2. Verificar duplicados
                var existente = db.Table<Usuario>()
                                  .FirstOrDefault(u => u.Nombreusuario.ToLower() == nombre.ToLower()
                                                    || u.Gmail.ToLower() == gmail.ToLower());

                if (existente != null)
                {
                    return "ERROR: El nombre de usuario o el correo ya están registrados.";
                }

                // 3. Crear el objeto
                var nuevoUsuario = new Usuario
                {
                    Nombreusuario = nombre,
                    Gmail = gmail,
                    Contrasenia = password,
                    Rol = rol
                };

              
                db.Insert(nuevoUsuario);

                // 5. Devolver mensaje estructurado con los datos (usando interpolación de strings $)
                return 
                       $"Usuario: {nuevoUsuario.Nombreusuario}\n" +
                       $"Email: {nuevoUsuario.Gmail}\n" +
                       $"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}"+
                       Gracias por contar con nosotros;
            }
            catch (Exception ex)
            {
                return "Error en la base de datos: " + ex.Message;
            }
        }
        */

        /**
         * Metodo Borrar Usuario:
         * Elimina un registro de la base de datos utilizando su ID.
         * @param id: El identificador numérico del usuario a eliminar.
         * @return: "OK" si se eliminó, o un mensaje de error si no se encontró o falló.
         **/
        public static string BorrarUsuario(int id)
        {
            try
            {
                if (db == null) Inicializar();

              
                var usuario = db.Table<Usuario>().FirstOrDefault(u => u.Id == id);

                if (usuario == null)
                {
                    return "El usuario no existe en la base de datos.";
                }

                db.Delete(usuario);

                return "Cuenta eliminada correctamente. Muchas gracias por haber pertenecido al equipo.";
            }
            catch (Exception ex)
            {
                return "Error al eliminar: " + ex.Message;
            }
        }


    }
}
