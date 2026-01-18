using PeliculasStudio.BaseDatos;
using PeliculasStudio.Modelos;
using PeliculasStudio.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PeliculasStudio.Vistas
{
    /// <summary>
    /// Lógica de interacción para UC_PanelAdmin.xaml
    /// </summary>
    public partial class UC_PanelAdmin : UserControl
    {
        private Usuario _adminLogueado;
        public UC_PanelAdmin(Usuario admin)
        {
            InitializeComponent();
            _adminLogueado = admin;
            CargarTablas();
        }
        private void CargarTablas()
        {
            var conexion = DatabaseServicie.GetConexion();
            if (conexion != null)
            {
             
                var listaUsuarios = conexion.Table<Usuario>().ToList();

               
                txtTotalUsuarios.Text = listaUsuarios.Count.ToString();
                txtTotalAdmins.Text = listaUsuarios.Count(u => u.Rol == TipoRol.Admin).ToString();

              
                dgUsuarios.ItemsSource = null;
                dgUsuarios.ItemsSource = listaUsuarios;

            
                dgPeliculas.ItemsSource = null;
                dgPeliculas.ItemsSource = conexion.Table<Pelicula>().ToList();
            }
        }

       

        // MÉTODO PARA ELIMINAR USUARIO
        private void btnEliminarUsuario_Click(object sender, RoutedEventArgs e)
        {
            var usuario = (sender as Button)?.DataContext as Usuario;

            if (usuario != null)
            {
                var resultado = MessageBox.Show($"¿Seguro que quieres borrar a {usuario.Nombreusuario}?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (resultado == MessageBoxResult.Yes)
                {
                    DatabaseServicie.BorrarUsuario(usuario.Id);
                    CargarTablas();
                }
            }
        }
        private void btnCambiarRol_Click(object sender, RoutedEventArgs e)
        {
            var usuarioAEditar = (sender as Button)?.DataContext as Usuario;

            // Verificamos que existan tanto el usuario a editar como la sesión del admin
            if (usuarioAEditar != null && _adminLogueado != null)
            {
                string nuevoRol = (usuarioAEditar.Rol == TipoRol.Admin) ? "Usuario" : "Administrador";

                // 1. Confirmación de intención
                if (MessageBox.Show($"¿Deseas cambiar el rol de {usuarioAEditar.Nombreusuario} a {nuevoRol}?",
                    "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No) return;

                // 2. Validación de seguridad: No dejar el sistema sin administradores
                if (usuarioAEditar.Rol == TipoRol.Admin)
                {
                    var numAdmins = DatabaseServicie.GetConexion()?.Table<Usuario>().Count(u => u.Rol == TipoRol.Admin) ?? 0;
                    if (numAdmins <= 1)
                    {
                        MessageBox.Show("Acción denegada: No se puede eliminar al último administrador del sistema.",
                            "Error de Seguridad", MessageBoxButton.OK, MessageBoxImage.Stop);
                        return;
                    }
                }

                // 3. Ventana de Password personalizada
                var ventanaPass = new VentanaPassword();
                if (ventanaPass.ShowDialog() == true)
                {
                    string passIntroducida = ventanaPass.Password;

                    // 4. Verificación SHA256 contra el admin que tiene la sesión abierta
                    if (Cifrado.VerifyPassword(passIntroducida, _adminLogueado.Contrasenia))
                    {
                        usuarioAEditar.Rol = (usuarioAEditar.Rol == TipoRol.Admin) ? TipoRol.Usuario : TipoRol.Admin;
                        DatabaseServicie.GetConexion()?.Update(usuarioAEditar);

                        // Limpiar y Recargar
                        dgUsuarios.ItemsSource = null;
                        CargarTablas();

                        MessageBox.Show($"¡Éxito! {usuarioAEditar.Nombreusuario} ahora es {nuevoRol}.", "Actualizado", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("La contraseña introducida es incorrecta.", "Error de Autenticación", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        // MÉTODO PARA ELIMINAR PELÍCULA
        private void btnEliminarPelicula_Click(object sender, RoutedEventArgs e)
        {
            var pelicula = (sender as Button)?.DataContext as Pelicula;

            if (pelicula != null)
            {
                var resultado = MessageBox.Show($"¿Borrar {pelicula.Titulo}?", "Confirmar", MessageBoxButton.YesNo);
                if (resultado == MessageBoxResult.Yes)
                {
                    DatabaseServicie.BorrarPelicula(TipoRol.Admin, pelicula.Id);
                    CargarTablas();
                }
            }
        }
 
        private void btnFiltroAdmins_Click(object sender, RoutedEventArgs e)
        {
            var conexion = DatabaseServicie.GetConexion();
            if (conexion != null)
            {
               
                var soloAdmins = conexion.Table<Usuario>()
                                         .Where(u => u.Rol == TipoRol.Admin)
                                         .ToList();

           
                dgUsuarios.ItemsSource = null;
                dgUsuarios.ItemsSource = soloAdmins;

               
                btnFiltroAdmins.Opacity = 0.7;
            }
        }

       
        private void btnVerTodos_Click(object sender, RoutedEventArgs e)
        {
          
            btnFiltroAdmins.Opacity = 1.0;
            CargarTablas();
        }
    }
}
