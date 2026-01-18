using PeliculasStudio.BaseDatos;
using PeliculasStudio.Modelos;
using PeliculasStudio.Utilidades;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
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


            this.Loaded += (s, e) =>
            {
                try
                {

                    btnTema.IsChecked = App.IsDarkMode;

                    GestordeTemas.AplicarTema(App.IsDarkMode);
                }
                catch (Exception ex)
                {
               
                    System.Diagnostics.Debug.WriteLine("Error en Loaded: " + ex.Message);
                }
            };
        }
        /**
        * Metodo Iniciar Sesion Click:
        * Valida las credenciales contra la base de datos y gestiona el acceso al sistema.
        * 1. Si los datos son correctos: Instancia la MainWindow, carga el UC_Inicio con el nombre del usuario y cierra el Login.
        * 2. Si son incorrectos: Resalta los campos en rojo y muestra un mensaje de error.
        * @param sender: El botón de inicio de sesión.
        * @param e: Argumentos del evento de click.
        **/
        private void btnIniciarSesion_Click(object sender, RoutedEventArgs e)
        {
            string nombre = txtUsuario.Text.Trim();
            string pass = txtPassword.Password;

           
            var brushError = (Brush)Application.Current.FindResource("BrushMensajeError");
            var brushNormal = (Brush)Application.Current.FindResource("BrushBordeControl");

            var usuarioEncontrado = DatabaseServicie.Login(nombre, pass);

            var main = Application.Current.MainWindow as MainWindow;
            if (usuarioEncontrado != null)
            {
            
                if (usuarioEncontrado.Rol == TipoRol.Admin)
                {
                    // Si es ADMIN, lo mandamos a la vista de administración
                    main?.Navegar(new UC_PanelAdmin(usuarioEncontrado));
                }
                else
                {
                    // Si es USUARIO normal, lo mandamos al inicio normal
                    main?.Navegar(new UC_Inicio(usuarioEncontrado));
                }
            }
            else
            {
         
                txtUsuario.BorderBrush = brushError;
                txtUsuario.BorderThickness = new Thickness(2);

                txtPassword.BorderBrush = brushError;
                txtPassword.BorderThickness = new Thickness(2);

                MessageBox.Show("Usuario o contraseña incorrectos.", "Error de acceso", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /**
        * Metodo Ir A Registro Click:
        * Gestiona la transición desde la pantalla de Login hacia la pantalla de Registro.
        * 1. Sincroniza el estado del tema actual para que la nueva ventana se abra con el mismo color.
        * 2. Instancia y muestra la ventana de Registro.
        * 3. Localiza y cierra la ventana de Login actual para optimizar el uso de memoria.
        * @param sender: El botón o enlace que solicita ir al registro.
        * @param e: Argumentos del evento de click.
        **/
        private void btnIrARegistro_Click(object sender, RoutedEventArgs e)
        {

            var main = Application.Current.MainWindow as MainWindow;

            main?.Navegar(new Registro());
        }

        /**
        * Metodo Tema Click (Login):
        * Gestiona el cambio de apariencia de la ventana de inicio de sesión.
        * Sincroniza el estado del botón con la configuración global de la aplicación.
        * @param sender: El ToggleButton (interruptor) de cambio de tema.
        * @param e: Argumentos del evento de click.
        **/
        private void btnTema_Click(object sender, RoutedEventArgs e)
        {
            App.IsDarkMode = btnTema.IsChecked ?? false;


            GestordeTemas.AplicarTema(App.IsDarkMode);

            try
            {
                
                var anim = GridPrincipal.Resources["AnimacionFondo"] as Storyboard;

                if (anim != null)
                {                 
                    anim.Stop();
                    anim.Begin();
                }
            }
            catch (Exception ex)
            {        
                System.Diagnostics.Debug.WriteLine("La animación no pudo iniciar: " + ex.Message);
            }
        }

    }
}