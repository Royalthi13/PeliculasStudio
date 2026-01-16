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
    public partial class UC_Login : UserControl
    {
        public UC_Login()
        {
            InitializeComponent();

            // Al cargar el control, verificamos si el modo oscuro ya estaba activo
            // Usamos Loaded para asegurarnos de que el XAML esté listo
            this.Loaded += (s, e) => {
                if (App.IsDarkMode)
                {
                    btnTema.IsChecked = true;
                    CambiarInterfazTema(true);
                }
            };
        }
        private void btnIniciarSesion_Click(object sender, RoutedEventArgs e)
        {
            string nombre = txtUsuario.Text.Trim();
            string pass = txtPassword.Password;

            var usuarioEncontrado = DatabaseServicie.ValidarLogin(nombre, pass);

            if (usuarioEncontrado != null)
            {
                // 1. Creamos la ventana principal
                MainWindow ventanaPrincipal = new MainWindow();

                // 2. Creamos el control de inicio con el nombre del usuario
                UC_Inicio inicio = new UC_Inicio(usuarioEncontrado.Nombreusuario);

                // 3. Metemos el UC_Inicio dentro del contenido de la MainWindow
                // Asumo que tu MainWindow tiene un Grid llamado "GridPrincipal" o similar
                ventanaPrincipal.Content = inicio;

                ventanaPrincipal.Show();

                // 4. Cerramos el login
                Window.GetWindow(this).Close();
            }
            else
            {
                // --- ERROR ---
                // Pintamos los bordes de rojo
                txtUsuario.BorderBrush = Brushes.Red;
                txtUsuario.BorderThickness = new Thickness(2);

                txtPassword.BorderBrush = Brushes.Red;
                txtPassword.BorderThickness = new Thickness(2);

                MessageBox.Show("Usuario o contraseña incorrectos.", "Error de acceso", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnIrARegistro_Click(object sender, RoutedEventArgs e)
        {
            // Antes de abrir la ventana de registro, nos aseguramos de que el estado sea el actual
            App.IsDarkMode = btnTema.IsChecked ?? false;

            Registro ventanaRegistro = new Registro();
            ventanaRegistro.Show();

            Window ventanaActual = Window.GetWindow(this);
            if (ventanaActual != null)
            {
                ventanaActual.Close();
            }
        }

        private void btnTema_Click(object sender, RoutedEventArgs e)
        {
            var boton = sender as System.Windows.Controls.Primitives.ToggleButton;
            bool modoOscuro = boton.IsChecked ?? false;

            // ACTUALIZAMOS LA VARIABLE GLOBAL
            App.IsDarkMode = modoOscuro;

            CambiarInterfazTema(modoOscuro);
        }

        // He extraído la lógica a un método para poder llamarlo desde el constructor o el click
        private void CambiarInterfazTema(bool modoOscuro)
        {
            if (GridPrincipal == null || BorderCentral == null) return;

            if (modoOscuro)
            {
                // --- MODO OSCURO ---
                GridPrincipal.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E1E1E"));
                BorderCentral.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2D2D2D"));

                txtTitulo.Foreground = Brushes.White;
                lblUsuario.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CCCCCC"));
                lblPass.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CCCCCC"));

                txtUsuario.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3F3F46"));
                txtUsuario.Foreground = Brushes.White;
                txtUsuario.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#555555"));
            }
            else
            {
                // --- MODO CLARO ---
                GridPrincipal.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F0F0F0"));
                BorderCentral.Background = Brushes.White;

                txtTitulo.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#333333"));
                lblUsuario.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#555555"));
                lblPass.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#555555"));

                txtUsuario.Background = Brushes.White;
                txtUsuario.Foreground = Brushes.Black;
                txtUsuario.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ABAdB3"));
            }
        }
    }
}