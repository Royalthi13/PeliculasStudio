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

 
            this.Loaded += (s, e) => {
                if (App.IsDarkMode)
                {
                    btnTema.IsChecked = true;
                    CambiarInterfazTema(true);
                }
            };
            Storyboard anim = (Storyboard)this.GridPrincipal.Resources["AnimacionFondo"];
            anim.Begin();
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

            
            var usuarioEncontrado = DatabaseServicie.Login(nombre, pass);

            if (usuarioEncontrado != null)
            {
                MainWindow ventanaPrincipal = new MainWindow();

           
                UC_Inicio inicio = new UC_Inicio(usuarioEncontrado.Nombreusuario);
                ventanaPrincipal.Content = inicio;
                ventanaPrincipal.Show();

           
                Window.GetWindow(this).Close();
            }
            else
            {
         
                txtUsuario.BorderBrush = Brushes.Red;
                txtUsuario.BorderThickness = new Thickness(2);

                txtPassword.BorderBrush = Brushes.Red;
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
            bool modoOscuro = boton.IsChecked ?? false;
            Storyboard anim = (Storyboard)this.GridPrincipal.Resources["AnimacionFondo"];
            anim.Begin();
            // ACTUALIZAMOS LA VARIABLE GLOBAL
            App.IsDarkMode = modoOscuro;
            CambiarInterfazTema(modoOscuro);
        }

        /**
        * Metodo Cambiar Interfaz Tema:
        * Centraliza la lógica de estilos visuales para la pantalla de Login.
        * Ajusta colores de fondo, etiquetas y cajas de texto de forma simultánea.
        * Mantiene la animación del degradado actualizando sus colores.
        * @param modoOscuro: Booleano que determina si se aplica el tema noche (true) o día (false).
        **/
     
        private void CambiarInterfazTema(bool modoOscuro)
        {
            if (GridPrincipal == null || BorderCentral == null) return;

            var fondoDegradado = GridPrincipal.Background as RadialGradientBrush;

            if (modoOscuro)
            {
                // --- MODO OSCURO CLÁSICO (Gris muy oscuro / Negro) ---
                if (fondoDegradado != null)
                {
                    // Centro gris oscuro y bordes negros para que el foco resalte al moverse
                    fondoDegradado.GradientStops[0].Color = (Color)ColorConverter.ConvertFromString("#2D2D2D");
                    fondoDegradado.GradientStops[1].Color = (Color)ColorConverter.ConvertFromString("#121212");

                    fondoDegradado.RadiusX = 1.2;
                    fondoDegradado.RadiusY = 1.2;
                }

                // Fondo del panel en gris grafito sólido
                BorderCentral.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E1E1E"));

                // Titulo Blanco con el resplandor original
                txtTitulo.Foreground = Brushes.White;
                if (txtTitulo.Effect is DropShadowEffect shadow)
                {
                    shadow.Color = (Color)ColorConverter.ConvertFromString("#B0E0E6");
                    shadow.BlurRadius = 15;
                    shadow.Opacity = 0.8;
                }

                // Colores de texto y cajas del modo oscuro original
                lblUsuario.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CCCCCC"));
                lblPass.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CCCCCC"));

                txtUsuario.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3F3F46"));
                txtUsuario.Foreground = Brushes.White;
                txtUsuario.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#555555"));
            }
            else
            {
            
                    // --- MODO CLARO LLAMATIVO (Blanco/Azul) ---
                    if (fondoDegradado != null)
                    {
                        fondoDegradado.GradientStops[0].Color = Colors.White;
                        fondoDegradado.GradientStops[1].Color = (Color)ColorConverter.ConvertFromString("#C8DCEF"); // Azul claro
                        fondoDegradado.RadiusX = 1.8; // Foco más abierto
                        fondoDegradado.RadiusY = 1.8;
                    }

                    BorderCentral.Background = Brushes.White;
                    txtTitulo.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#005A9E"));

                    // Ajuste de resplandor para modo claro (más sutil)
                    if (txtTitulo.Effect is DropShadowEffect shadow)
                    {
                        shadow.Color = (Color)ColorConverter.ConvertFromString("#B0E0E6");
                        shadow.Opacity = 0.4;
                        shadow.BlurRadius = 8;
                    }

                    lblUsuario.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#444444"));
                    lblPass.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#444444"));
                    txtUsuario.Background = Brushes.White;
                    txtUsuario.Foreground = Brushes.Black;
                    txtUsuario.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC"));
                
            }
        }
    }
}