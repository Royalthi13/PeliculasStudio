using SQLite;


namespace PeliculasStudio.Modelos

{

    /**
     * Clase Usuario:
     * Representa la entidad de usuario dentro de la base de datos.
     * Define la estructura de la tabla 'Usuario' con sus restricciones y campos principales.
     **/



    public enum TipoRol
    {
        /// <summary>
        /// Rol estándar: Permite visualizar contenido pero no modificar la base de datos.
        /// Internamente, SQLite lo guardará como el valor entero 0.
        /// </summary>
        Usuario=0,

        /// <summary>
        /// Rol con privilegios: Permite añadir, editar y eliminar películas o usuarios.
        /// Internamente, SQLite lo guardará como el valor entero 1.
        /// </summary>
        Admin=1
    }
    public class Usuario
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull, Unique]
        public  string Nombreusuario { get; set; } 

        [NotNull, Unique]
        public  string Gmail { get; set; } 

        [NotNull]
        public  string Contrasenia { get; set; }

        [NotNull]
        public TipoRol Rol { get; set; }
        [NotNull]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}
