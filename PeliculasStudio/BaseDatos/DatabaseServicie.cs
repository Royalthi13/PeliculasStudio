using PeliculasStudio.Modelos;
using PeliculasStudio.Utilidades;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
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
     * 
     **/
   

    public static class DatabaseServicie
    {
 
       

        private static SQLiteConnection? db;
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
            if (db!.Table<Usuario>().Count() == 0)
            {
                db.Insert(new Usuario
                {
                    Nombreusuario = "admin",
                    Gmail="admin@studio.com",
                    Contrasenia = Cifrado.HashPassword("123"),

                    Rol = TipoRol.Admin
                });
            }

            if (db.Table<Pelicula>().Count() == 0)
            {
                db.InsertAll(DatosIniciales.ObtenerListado());
            }
        }

        /**
         * Metodo GetConexion:
         * Proporciona acceso a la instancia activa de la conexion SQLite.
         * Se utiliza para realizar consultas personalizadas o LINQ desde otras clases
         * garantizando que toda la aplicacion comparta el mismo tunel de datos.
         * @return: Retorna el objeto SQLiteConnection inicializado.
         **/

        public static SQLiteConnection? GetConexion()
        {
            return db;
        }





        /**
        * Metodo Registrar Usuario:
        * Valida los datos de entrada e inserta un nuevo usuario con ID autogenerado.
        * @param nombre: Nickname del usuario.
        * @param gmail: Correo electrónico (se valida formato).
        * @param password: Clave de acceso.
        * @param rol: "Admin" o "Usuario" (por defecto "Usuario").
        * @return: Envia un Mensaje de como ha sido, si no lo deseas pue' no lo muestres.
        **/

        public static string CrearUsuario(string nombre, string gmail, string password, TipoRol rol = TipoRol.Usuario)
        {
            try
            {
                if (db == null) Inicializar();

                // 1. Validar formato de email
                string patronEmail = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(gmail, patronEmail))
                {
                    return "ERROR: El formato del correo electrónico no es válido.";
                }

                // 2. Comprobar si ya existe (Case Insensitive) - INTEGRADO DE MAIN
                Usuario? existente = db!.Table<Usuario>()
                                  .FirstOrDefault(u => u.Nombreusuario.ToLower() == nombre.ToLower()
                                                    || u.Gmail.ToLower() == gmail.ToLower());

                if (existente != null)
                {
                    return "ERROR: El nombre de usuario o el correo ya están registrados.";
                }

                // 3. Crear nuevo usuario con el Cifrado de tu compañero
                var nuevoUsuario = new Usuario
                {
                    Nombreusuario = nombre,
                    Gmail = gmail,
                    Contrasenia = Cifrado.HashPassword(password), // Cifrado integrado
                    Rol = rol
                };

                db.Insert(nuevoUsuario);

                return $"USUARIO REGISTRADO CON ÉXITO\n" +
                       $"---------------------------\n" +
                       $"Nombre: {nuevoUsuario.Nombreusuario}\n" +
                       $"Email: {nuevoUsuario.Gmail}\n" +
                       $"Rol: {nuevoUsuario.Rol}\n" +
                       $"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}\n" +
                       $"---------------------------\n" +
                       $"Gracias por contar con nosotros.";
            }
            catch (Exception ex)
            {
                return "Error en la base de datos: " + ex.Message;
            }
        }



        /**
         * Metodo Login Usuario:
         * .
         * Verifica las credenciales de acceso comparando el usuario y el hash de la contraseña.
         * @return:Devuelve el objeto usuario si el login es correcto devuelve null si el usuario no existe o la contraseña es incorrecta
         **/

        public static Usuario? Login(string usuario, string paswd)
        {
            if (db == null) Inicializar();

        
            Usuario? user = db!.Table<Usuario>()
                               .FirstOrDefault(u => u.Nombreusuario == usuario);

            if (user == null) return null;

            if (user.Contrasenia == Cifrado.HashPassword(paswd))
            {
                return user;
            }

            return null;
        }


        /**
         * Metodo Borrar Usuario:
         * Elimina un registro de la base de datos utilizando su ID.
         * @param id: El identificador numérico del usuario a eliminar.
         * @return: string de: si se eliminó, o un mensaje de error si no se encontró o falló.
         **/
        public static string BorrarUsuario(int id)
        {
            try
            {
                if (db == null) Inicializar();
                
                var usuario = db!.Table<Usuario>().FirstOrDefault(u => u.Id == id);

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

        /**
         * Metodo Añadir Pelicula (Solo Admin):
         * Verifica que el usuario tenga rol de "Admin" antes de insertar.
         * @param rolUsuario: El rol de quien ejecuta la accion.
         * @return: Mensaje de exito o error de permisos.
         **/

        public static string AniadirPelicula(TipoRol rolUsuario, string titulo, int anio, string genero, string resumen, string trailer, string portada)
        {
            try
            {
             
                if (rolUsuario != TipoRol.Admin)
                {
                    return "ACCESO DENEGADO: No tienes permisos para añadir películas.";
                }

                if (db == null) Inicializar();

            
                if (string.IsNullOrWhiteSpace(titulo) || anio < 1888)
                {
                    return "ERROR: Datos de la película inválidos (Título vacío o año incorrecto).";
                }

               
                var existente = db!.Table<Pelicula>()
                                  .FirstOrDefault(p => p.Titulo.ToLower() == titulo.ToLower() && p.Anio == anio);

                if (existente != null)
                {
                    return $"ERROR: La película '{titulo}' ({anio}) ya existe en la base de datos.";
                }

               
                var nuevaPelicula = new Pelicula
                {
                    Titulo = titulo,
                    Anio = anio,
                    Genero = genero,
                    Resumen = resumen,
                    TrailerPath = trailer,
                    PortadaPath = portada
                };

              
                db.Insert(nuevaPelicula);

                return $"ÉXITO: La película '{titulo}' ha sido añadida correctamente por el Administrador.";
            }
            catch (Exception ex)
            {
                return "Error crítico en la base de datos: " + ex.Message;
            }
        }
        /**
         * Metodo Borrar Pelicula:
         * Elimina una película de la base de datos por su ID.
         * Requiere permisos de Administrador.
         * @param rolUsuario: El rol de quien intenta borrar.
         * @param idPelicula: El ID único de la película.
         * @return: Mensaje con el resultado de la operación.
         **/
        public static string BorrarPelicula(TipoRol rolUsuario, int idPelicula)
        {
            try
            {
                
                if (rolUsuario != TipoRol.Admin)
                {
                    return "ACCESO DENEGADO: Solo los administradores pueden eliminar contenido.";
                }

                if (db == null) Inicializar();

               
                var pelicula = db!.Table<Pelicula>().FirstOrDefault(p => p.Id == idPelicula);

                if (pelicula == null)
                {
                    return "ERROR: La película no existe en la base de datos.";
                }

          
                string tituloBorrado = pelicula.Titulo;
                db.Delete(pelicula);

                return $"ÉXITO: La película '{tituloBorrado}' ha sido eliminada del catálogo.";
            }
            catch (Exception ex)
            {
                return "Error al intentar eliminar la película: " + ex.Message;
            }
        }


    }
}
