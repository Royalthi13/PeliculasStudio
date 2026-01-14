using SQLite;

namespace PeliculasStudio.Modelos
{
    public class Usuario
    {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull, Unique]
        public string Nombreusuario { get; set; }

        [NotNull]
        public string Contrasenia { get; set; }

        [NotNull]
        public string Rol { get; set; }
    }
}
