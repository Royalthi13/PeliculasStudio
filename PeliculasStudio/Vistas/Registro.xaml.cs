using PeliculasStudio.BaseDatos;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PeliculasStudio.Vistas
{

    public partial class Registro : UserControl
    {
        public Registro()
        {
            InitializeComponent();

        }
        /**
        * Metodo Registrar Click: Usuario minimo 5 letras sin digitos rarosos
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
            btnRegistrar.IsEnabled = false;

            try
            {
                string nombre = txtUsuario.Text.Trim();
                string correo = txtGmail.Text.Trim().ToLower();
                string pass = txtPassword.Password;
                string passRepeat = txtRepeatPassword.Password;

                var brushError = (Brush)Application.Current.FindResource("BrushMensajeError");
                var brushNormal = (Brush)Application.Current.FindResource("BrushBordeControl");

               
                txtUsuario.BorderBrush = txtGmail.BorderBrush = txtPassword.BorderBrush = txtRepeatPassword.BorderBrush = brushNormal;
                txtUsuario.BorderThickness = txtGmail.BorderThickness = txtPassword.BorderThickness = txtRepeatPassword.BorderThickness = new Thickness(1);

               
                if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(pass))
                {
                    if (string.IsNullOrWhiteSpace(nombre)) txtUsuario.BorderBrush = brushError;
                    if (string.IsNullOrWhiteSpace(correo)) txtGmail.BorderBrush = brushError;
                    if (string.IsNullOrWhiteSpace(pass)) txtPassword.BorderBrush = brushError;

                    MessageBox.Show("Por favor, rellena los campos obligatorios.", "Campos vacíos", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

               
                if (nombre.Length < 5 || !System.Text.RegularExpressions.Regex.IsMatch(nombre, @"^[a-zA-Z0-9_]+$"))
                {
                    txtUsuario.BorderBrush = brushError;
                    txtUsuario.BorderThickness = new Thickness(2);
                    MessageBox.Show("El nombre de usuario debe tener al menos 5 caracteres y no contener símbolos.", "Usuario no válido", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

               
                if (!ValidarSeguridadPassword(pass))
                {
                    txtPassword.BorderBrush = brushError;
                    MessageBox.Show("La contraseña no cumple los requisitos de seguridad.");
                    return;
                }

          
                if (pass != passRepeat)
                {
                    txtRepeatPassword.BorderBrush = brushError;
                    MessageBox.Show("Las contraseñas no coinciden.");
                    return;
                }

                string resultado = DatabaseServicie.CrearUsuario(nombre, correo, pass);

                if (resultado.StartsWith("ERROR"))
                {
                    string resMin = resultado.ToLower();

                    // Detección exclusiva basada en los nuevos mensajes de la BD
                    bool tieneUsuario = resMin.Contains("usuario");
                    bool tieneCorreo = resMin.Contains("correo") || resMin.Contains("gmail") || resMin.Contains("email");

                    if (tieneUsuario && !tieneCorreo)
                    {
                        txtUsuario.BorderBrush = brushError;
                        txtUsuario.BorderThickness = new Thickness(2);
                    }
                    else if (tieneCorreo && !tieneUsuario)
                    {
                        txtGmail.BorderBrush = brushError;
                        txtGmail.BorderThickness = new Thickness(2);
                    }
                    else
                    {
                        // Si por alguna razón fallan ambos o es otro error
                        txtUsuario.BorderBrush = txtGmail.BorderBrush = brushError;
                        txtUsuario.BorderThickness = txtGmail.BorderThickness = new Thickness(2);
                    }

                    MessageBox.Show(resultado, "Error de registro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show(resultado, "¡Bienvenido!", MessageBoxButton.OK, MessageBoxImage.Information);
                    BtnVolver_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error inesperado: " + ex.Message);
            }
            finally
            {
                btnRegistrar.IsEnabled = true;
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
                control.BorderBrush = (Brush)Application.Current.FindResource("BrushBordeControl");

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

            // Extraemos los pinceles directamente del diccionario activo
            // Esto garantiza que el rojo/verde sea el exacto que definiste para cada tema
            var brushExito = (Brush)Application.Current.FindResource("BrushMensajeExito");
            var brushError = (Brush)Application.Current.FindResource("BrushMensajeError");
            var brushBordeNormal = (Brush)Application.Current.FindResource("BrushBordeControl");

            // Caso: Campo vacío
            if (string.IsNullOrEmpty(repeatPass))
            {
                txtRepeatPassword.BorderBrush = brushBordeNormal;
                txtRepeatPassword.BorderThickness = new Thickness(1);

                iconCoincide.Text = "✖";
                lblCoincide.Text = "Las contraseñas no coinciden";
                iconCoincide.Foreground = brushError;
                lblCoincide.Foreground = brushError;
                return;
            }

            // Comprobación de coincidencia
            if (pass == repeatPass)
            {
                // --- COINCIDEN ---
                txtRepeatPassword.BorderBrush = brushExito;
                txtRepeatPassword.BorderThickness = new Thickness(2);

                iconCoincide.Text = "✔";
                lblCoincide.Text = "Las contraseñas coinciden";
                iconCoincide.Foreground = brushExito;
                lblCoincide.Foreground = brushExito;
            }
            else
            {
                // --- NO COINCIDEN ---
                txtRepeatPassword.BorderBrush = brushError;
                txtRepeatPassword.BorderThickness = new Thickness(2);

                iconCoincide.Text = "✖";
                lblCoincide.Text = "Las contraseñas no coinciden";
                iconCoincide.Foreground = brushError;
                lblCoincide.Foreground = brushError;
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

            
            var brushError = (Brush)Application.Current.FindResource("BrushMensajeError");
            var brushExito = (Brush)Application.Current.FindResource("BrushMensajeExito");

          
            ActualizarEstadoRequisito(pass.Length >= 8, iconLongitud, lblLongitud, brushError, brushExito);
            ActualizarEstadoRequisito(pass.Any(char.IsUpper), iconMayuscula, lblMayuscula, brushError, brushExito);
            ActualizarEstadoRequisito(pass.Any(char.IsDigit), iconNumero, lblNumero, brushError, brushExito);

            bool tieneEspecial = pass.Any(ch => !char.IsLetterOrDigit(ch));
            ActualizarEstadoRequisito(tieneEspecial, iconEspecial, lblEspecial, brushError, brushExito);

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
        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {

            var main = Application.Current.MainWindow as MainWindow;
            main?.Navegar(new UC_Login());
        }

       
    }
}

