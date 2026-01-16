using PeliculasStudio.BaseDatos;
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
using System.Windows.Shapes;

namespace PeliculasStudio.Vistas
{

    public partial class Registro : Window
    {
        public Registro()
        {
            InitializeComponent();        
            if (App.IsDarkMode)
            {
                btnTema.IsChecked = true;
                AplicarTemaOscuro();
            }
            else
            {
                btnTema.IsChecked = false;
                AplicarTemaClaro();
            }
        }
        /**
        * Metodo Registrar Click:
        * Gestiona el flujo completo de registro de un nuevo usuario.
        * 1. Recolecta y limpia los datos de la interfaz (Trim y ToLower).
        * 2. Valida visualmente campos vacíos pintando bordes de rojo.
        * 3. Verifica la robustez de la contraseña y la coincidencia entre campos.
        * 4. Intenta persistir el usuario en la base de datos y maneja la respuesta.
        * @param sender: El botón de registro que lanza el evento.
        * @param e: Argumentos del evento de click.
        **/
        private void btnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            string nombre = txtUsuario.Text.Trim();
            string correo = txtGmail.Text.Trim().ToLower();
            string pass = txtPassword.Password;
            string passRepeat = txtRepeatPassword.Password;
            SolidColorBrush colorError = Brushes.Red;
            SolidColorBrush colorNormal = App.IsDarkMode ?
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#555555")) :
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ABAdB3"));
            bool hayCamposVacios = false;
            if (string.IsNullOrWhiteSpace(nombre))
            {
                txtUsuario.BorderBrush = colorError;
                txtUsuario.BorderThickness = new Thickness(2);
                hayCamposVacios = true;
            }
            else
            {
                txtUsuario.BorderBrush = colorNormal;
                txtUsuario.BorderThickness = new Thickness(1);
            }
            if (string.IsNullOrWhiteSpace(correo))
            {
                txtGmail.BorderBrush = colorError;
                txtGmail.BorderThickness = new Thickness(2);
                hayCamposVacios = true;
            }
            else
            {
                txtGmail.BorderBrush = colorNormal;
                txtGmail.BorderThickness = new Thickness(1);
            }
            if (string.IsNullOrWhiteSpace(pass))
            {
                txtPassword.BorderBrush = colorError;
                txtPassword.BorderThickness = new Thickness(2);
                hayCamposVacios = true;
            }
            else
            {
                txtPassword.BorderBrush = colorNormal;
                txtPassword.BorderThickness = new Thickness(1);
            }
            if (hayCamposVacios)
            {
                MessageBox.Show("Por favor, rellena los campos en rojo.");
                return;
            }
            if (!ValidarSeguridadPassword(pass))
            {
                MessageBox.Show("La contraseña no es segura.");
                return;
            }
            if (pass != passRepeat)
            {
                MessageBox.Show("Las contraseñas no coinciden.");
                return;
            }           
            string resultado = DatabaseServicie.CrearUsuario(nombre, correo, pass);

            if (resultado.StartsWith("ERROR"))
            {
                MessageBox.Show(resultado, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show(resultado, "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                btnVolver_Click(null, null);
            }
        }
        /**
        * Metodo Validar Seguridad Password:
        * Realiza una comprobación lógica de los criterios de seguridad requeridos.
        * @param password: La cadena de texto con la contraseña a evaluar.
        * @returns: Devuelve true solo si cumple simultáneamente con longitud mínima, 
        * mayúscula, número y carácter especial; de lo contrario, devuelve false.
        **/
        private bool ValidarSeguridadPassword(string password)
        {
            if (string.IsNullOrEmpty(password)) return false;

            bool tieneLongitud = password.Length >= 8;
            bool tieneMayuscula = password.Any(char.IsUpper);
            bool tieneNumero = password.Any(char.IsDigit);
            bool tieneEspecial = password.Any(ch => !char.IsLetterOrDigit(ch));

           
            return tieneLongitud && tieneMayuscula && tieneNumero && tieneEspecial;
        }


        //-------------------------------------------------------------------------------
        /**
        * Metodo Limpiar Borde TextChanged:
        * Restaura el diseño original del control cuando el usuario empieza a escribir.
        * Se utiliza para eliminar el resaltado de error (borde rojo) de forma dinámica.
        * @param sender: El control que disparó el evento (TextBox o PasswordBox).
        * @param e: Argumentos del evento de cambio de texto.
        **/
        private void LimpiarBorde_TextChanged(object sender, TextChangedEventArgs e)
        {
            var control = sender as Control;
            if (control != null)
            {
                // Al escribir, devolvemos el borde al color normal del tema
                control.BorderBrush = App.IsDarkMode ?
                    new SolidColorBrush((Color)ColorConverter.ConvertFromString("#555555")) :
                    new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ABAdB3"));
                control.BorderThickness = new Thickness(1);
            }
        }
        /**
        * Metodo Repeat Password Changed:
        * Compara en tiempo real la contraseña principal con la repetición.
        * Actualiza dinámicamente el color del borde, el icono y el texto de ayuda.
        * @param sender: El control PasswordBox de repetir contraseña.
        * @param e: Argumentos del evento de cambio de contraseña.
        **/
        private void txtRepeatPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            string pass = txtPassword.Password;
            string repeatPass = txtRepeatPassword.Password;

            SolidColorBrush verde = Brushes.Green;
            SolidColorBrush rojo = App.IsDarkMode ?
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF4C4C")) :
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D32F2F"));

            
            if (string.IsNullOrEmpty(repeatPass))
            {
                txtRepeatPassword.BorderBrush = App.IsDarkMode ?
                    new SolidColorBrush((Color)ColorConverter.ConvertFromString("#555555")) :
                    new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ABAdB3"));

                txtRepeatPassword.BorderThickness = new Thickness(1);
                iconCoincide.Text = "✖";
                lblCoincide.Text = "Las contraseñas no coinciden"; 
                iconCoincide.Foreground = rojo;
                lblCoincide.Foreground = rojo;
                return;
            }

            // Comprobación de coincidencia
            if (pass == repeatPass)
            {
                // --- COINCIDEN ---
                txtRepeatPassword.BorderBrush = verde;
                txtRepeatPassword.BorderThickness = new Thickness(2.5);

                iconCoincide.Text = "✔";
                lblCoincide.Text = "Las contraseñas coinciden"; 
                iconCoincide.Foreground = verde;
                lblCoincide.Foreground = verde;
            }
            else
            {
                // --- NO COINCIDEN ---
                txtRepeatPassword.BorderBrush = Brushes.Red;
                txtRepeatPassword.BorderThickness = new Thickness(2.5);

                iconCoincide.Text = "✖";
                lblCoincide.Text = "Las contraseñas no coinciden";
                iconCoincide.Foreground = rojo;
                lblCoincide.Foreground = rojo;
            }
        }
        /**
        * Metodo Password Changed:
        * Evalúa en tiempo real la robustez de la contraseña mientras el usuario escribe.
        * Comprueba longitud, mayúsculas, números y caracteres especiales.
        * @param sender: El control PasswordBox de la contraseña principal.
        * @param e: Argumentos del evento de cambio de contraseña.
        **/
        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            string pass = txtPassword.Password;       
            SolidColorBrush colorRojo = App.IsDarkMode ?
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF4C4C")) :
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D32F2F"));

            SolidColorBrush colorVerde = Brushes.Green;          
            ActualizarEstadoRequisito(pass.Length >= 8, iconLongitud, lblLongitud, colorRojo, colorVerde);         
            ActualizarEstadoRequisito(pass.Any(char.IsUpper), iconMayuscula, lblMayuscula, colorRojo, colorVerde);        
            ActualizarEstadoRequisito(pass.Any(char.IsDigit), iconNumero, lblNumero, colorRojo, colorVerde);
            bool tieneEspecial = pass.Any(ch => !char.IsLetterOrDigit(ch));
            ActualizarEstadoRequisito(tieneEspecial, iconEspecial, lblEspecial, colorRojo, colorVerde);
            txtRepeatPassword_PasswordChanged(null, null);
        }

        /**
          * Metodo Actualizar Estado Requisito:
          * Cambia la apariencia visual de un requisito de validación (longitud, mayúsculas, etc.).
          * Actualiza el icono y el color del texto dependiendo de si se cumple o no la condición.
          * @param cumple: Booleano que indica si el requisito de seguridad se ha alcanzado.
          * @param icono: El TextBlock que contiene el símbolo (✔ o ✖).
          * @param texto: El TextBlock que contiene la descripción del requisito.
          * @param rojo: Color para indicar que el requisito falta.
          * @param verde: Color para indicar que el requisito está completado.
          **/
        private void ActualizarEstadoRequisito(bool cumple, TextBlock icono, TextBlock texto, Brush rojo, Brush verde)
        {
            if (cumple)
            {
                icono.Text = "✔";
                icono.Foreground = verde;
                texto.Foreground = verde;
            }
            else
            {
                icono.Text = "✖";
                icono.Foreground = rojo;
                texto.Foreground = rojo;
            }
        }

        /**
        * Metodo Volver Click:
        * Gestiona el retorno a la pantalla principal desde la ventana de registro.
        * Crea una nueva instancia de MainWindow y cierra la ventana actual para liberar recursos.
        * @param sender: El botón "Volver" que dispara el evento.
        * @param e: Argumentos del evento de click.
        **/
        private void btnVolver_Click(object sender, RoutedEventArgs e)
        {
        
            MainWindow principal = new MainWindow();
            principal.Show();

            this.Close();
        }
        /**
        * Metodo Tema Click:
        * Gestiona el cambio dinámico entre el modo claro y el modo oscuro.
        * Actualiza la propiedad global de la aplicación y dispara los cambios visuales.
        * @param sender: El ToggleButton que activa o desactiva el modo oscuro.
        * @param e: Argumentos del evento de click.
        **/
        private void btnTema_Click(object sender, RoutedEventArgs e)
        {
           
            App.IsDarkMode = btnTema.IsChecked ?? false;

            if (App.IsDarkMode) AplicarTemaOscuro();
            else AplicarTemaClaro();
        }
        /**
        * Metodo Aplicar Tema Oscuro:
        * Modifica los recursos visuales de la interfaz para adaptarse al modo noche.
        * Cambia los fondos de los contenedores y establece los colores de fuente a tonos claros 
        * para garantizar un alto contraste y descanso visual.
        **/
        private void AplicarTemaOscuro()
        {
            GridPrincipal.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E1E1E"));
            BorderFormulario.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2D2D2D"));
            BorderRequisitos.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#252526"));

            // Ponemos todos los textos en Blanco Puro
            txtTitulo.Foreground = Brushes.White;
            lblUsuario.Foreground = Brushes.White;
            lblGmail.Foreground = Brushes.White;
            lblPassword.Foreground = Brushes.White;
            lblSeguridadTitulo.Foreground = Brushes.White;
            lblInstruccion.Foreground = Brushes.LightGray;
            lblNota.Foreground = Brushes.LightGray;
          
        }
        /**
        * Metodo Aplicar Tema Claro:
        * Restablece la paleta de colores original de la interfaz (Modo Día).
        * Configura fondos claros y suaves junto con tipografías en grises oscuros
        * para mantener la estética estándar y una legibilidad óptima.
        **/
        private void AplicarTemaClaro()
        {
            GridPrincipal.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F0F0F0"));
            BorderFormulario.Background = Brushes.White;
            BorderRequisitos.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F9F9F9"));

            // Volvemos a los grises oscuros
            txtTitulo.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#333333"));
            lblUsuario.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#666666"));
            lblGmail.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#666666"));
            lblPassword.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#666666"));
            lblSeguridadTitulo.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#333333"));
            lblInstruccion.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#777777"));
            lblNota.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#888888"));
          
        }
    }
}

