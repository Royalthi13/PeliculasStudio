using PeliculasStudio.BaseDatos;
using PeliculasStudio.Modelos;
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
        public UC_PanelAdmin(string nombreAdmin)
        {
            InitializeComponent();
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
       
        private void btnCambiarRol_Click(object sender, RoutedEventArgs e)
        {
            var usuario = (sender as Button)?.DataContext as Usuario;

            if (usuario != null)
            {
            
                if (usuario.Rol == TipoRol.Admin)
                {
                  
                    var numAdmins = DatabaseServicie.GetConexion()?.Table<Usuario>()
                                    .Count(u => u.Rol == TipoRol.Admin) ?? 0;

               
                    if (numAdmins <= 1)
                    {
                        MessageBox.Show("No se puede cambiar el rango. Debe existir al menos un administrador en el sistema.",
                                        "Acción denegada", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return; 
                    }
                }

           
                usuario.Rol = (usuario.Rol == TipoRol.Admin) ? TipoRol.Usuario : TipoRol.Admin;

                DatabaseServicie.GetConexion()?.Update(usuario);

 
                dgUsuarios.ItemsSource = null;
                CargarTablas();
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
