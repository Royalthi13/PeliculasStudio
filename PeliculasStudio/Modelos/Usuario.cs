using SQLite;
using static PeliculasStudio.BaseDatos.DatabaseServicie;

namespace PeliculasStudio.Modelos
{
    /**
     * Clase Usuario:
     * Representa la entidad de usuario dentro de la base de datos.
     * Define la estructura de la tabla 'Usuario' con sus restricciones y campos principales.
     **/
    public class Usuario
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull, Unique]
        public string Nombreusuario { get; set; }

        [NotNull, Unique]
        public string Gmail { get; set; } 

        [NotNull]
        public string Contrasenia { get; set; }

        [NotNull]
        public TipoRol Rol { get; set; }
    }
}
