using PeliculasStudio.BaseDatos;
using PeliculasStudio.Modelos;
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
                
                    if (App.IsDarkMode)
                    {
                        btnTema.IsChecked = true;
                        CambiarInterfazTema(true);
                    }

              
                    if (GridPrincipal?.Resources != null && GridPrincipal.Resources.Contains("AnimacionFondo"))
                    {
                        Storyboard anim = (Storyboard)GridPrincipal.Resources["AnimacionFondo"];
                        anim.Begin();
                    }
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

            if (usuarioEncontrado != null)
            {
                MainWindow ventanaPrincipal = new MainWindow();

                
                UC_Inicio inicio = new UC_Inicio(usuarioEncontrado.Nombreusuario);
                ventanaPrincipal.Content = inicio;
                ventanaPrincipal.Show();

               
                Window.GetWindow(this)?.Close();
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
            
            App.IsDarkMode = btnTema.IsChecked ?? false;

            Registro ventanaRegistro = new Registro();
            ventanaRegistro.Show();

            Window ventanaActual = Window.GetWindow(this);
            if (ventanaActual != null)
            {
                ventanaActual.Close();
            }
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
            var boton = sender as System.Windows.Controls.Primitives.ToggleButton;
            if (boton == null) return;

            bool modoOscuro = boton.IsChecked ?? false;
            App.IsDarkMode = modoOscuro;

            
            CambiarInterfazTema(modoOscuro);

          
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

        /**
          * Metodo CambiarInterfazTema:
          * Gestiona la apariencia global de la aplicación mediante la conmutación de diccionarios de recursos.
          * * 1. Desvincula el tema visual anterior del árbol de recursos de la aplicación.
          * 2. Carga dinámicamente el diccionario XAML correspondiente (Claro u Oscuro).
          * 3. Notifica al motor de WPF para que actualice automáticamente todos los controles 
          * vinculados mediante 'DynamicResource'.
          * * Este enfoque elimina el acoplamiento entre la lógica de C# y los valores hexadecimales de diseño.
          * * @param modoOscuro: Determina si se carga 'Tema.Oscuro.xaml' (true) o 'Tema.Claro.xaml' (false).
          **/

        private void CambiarInterfazTema(bool modoOscuro)
        {
           
            Application.Current.Resources.MergedDictionaries.Clear();

         
            ResourceDictionary nuevoTema = new ResourceDictionary();

           
            string ruta = modoOscuro ? "Temas/Tema.Oscuro.xaml" : "Temas/Tema.Claro.xaml";

            try
            {
                nuevoTema.Source = new Uri(ruta, UriKind.Relative);
                Application.Current.Resources.MergedDictionaries.Add(nuevoTema);
            }
            catch (Exception ex)
            {
               
                Debug.WriteLine("Error cargando tema: " + ex.Message);
            }

        }
    }
}