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
        private List<GeneroPelicula> _generosSeleccionados = new List<GeneroPelicula>();
        private bool _ordenDescendente = true; 
        private bool _cargandoFiltros = true;
        private string _criterioOrden = "VistasDesc"; 
        public UC_PanelAdmin(Usuario admin)
        {
            InitializeComponent();
            icGeneros.ItemsSource = Enum.GetValues(typeof(GeneroPelicula));
            _adminLogueado = admin;
            _cargandoFiltros = false;
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


        //_________________________________________ZONA DE USUARIOS__________________________________________________________________________
        /**
         * Metodo Eliminar Usuario Click:
         * Gestiona la eliminación permanente de un usuario de la base de datos con doble factor de seguridad.
         * 1. Validación de Identidad: Evita que el administrador se elimine a sí mismo por error.
         * 2. Confirmación y Advertencia: Muestra una ventana personalizada con un aviso de acción irreversible.
         * 3. Verificación de Seguridad: Solicita la contraseña del administrador actual mediante un PasswordBox.
         * 4. Ejecución: Si la clave es correcta, borra el registro, limpia el DataGrid y refresca la lista.
         * @param sender: El botón de eliminar dentro de la fila del DataGrid.
         * @param e: Argumentos del evento de click.
         **/
        private void btnEliminarUsuario_Click(object sender, RoutedEventArgs e)
        {
            var usuarioAEliminar = (sender as Button)?.DataContext as Usuario;

            if (usuarioAEliminar != null && _adminLogueado != null)
            {
                // 1. Evitar que el admin se borre a sí mismo
                if (usuarioAEliminar.Id == _adminLogueado.Id)
                {
                    MessageBox.Show("No puedes eliminar tu propia cuenta mientras estás en el panel.", "Acción denegada", MessageBoxButton.OK, MessageBoxImage.Stop);
                    return;
                }

                // 2. NUEVA VALIDACIÓN: Evitar borrar al último administrador
                if (usuarioAEliminar.Rol == TipoRol.Admin)
                {
                    var numAdmins = DatabaseServicie.GetConexion()?.Table<Usuario>().Count(u => u.Rol == TipoRol.Admin) ?? 0;

                    if (numAdmins <= 1)
                    {
                        MessageBox.Show("Acción denegada: No se puede eliminar al único administrador del sistema.",
                            "Error de Seguridad", MessageBoxButton.OK, MessageBoxImage.Stop);
                        return;
                    }
                }

                // 3. Confirmación con contraseña
                string aviso = $"¡PELIGRO! Vas a ELIMINAR permanentemente al usuario '{usuarioAEliminar.Nombreusuario}'. Esta acción no se puede deshacer.";
                var ventanaPass = new VentanaPassword(aviso);

                if (ventanaPass.ShowDialog() == true)
                {
                    if (Cifrado.VerifyPassword(ventanaPass.Password, _adminLogueado.Contrasenia))
                    {
                        // Borrado físico en la base de datos
                        DatabaseServicie.BorrarUsuario(usuarioAEliminar.Id);

                        // Actualización de la UI en tiempo real
                        CargarTablas();

                        MessageBox.Show($"El usuario '{usuarioAEliminar.Nombreusuario}' ha sido eliminado correctamente.", "Usuario Borrado");
                    }
                    else
                    {
                        MessageBox.Show("Contraseña incorrecta. El usuario no ha sido eliminado.", "Error de Seguridad", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        /**
         * Metodo Cambiar Rol Click:
         * Modifica el rango de un usuario (Admin/Usuario) garantizando la integridad del sistema.
         * 1. Regla de Negocio: Verifica que no se elimine el permiso de administrador al último usuario con dicho rango.
         * 2. Preparación: Define dinámicamente el nuevo rol y genera el mensaje informativo para el diálogo.
         * 3. Seguridad: Instancia VentanaPassword para solicitar la clave del administrador que opera la sesión.
         * 4. Actualización: Tras validar el SHA256, actualiza el registro en la base de datos y refresca el DataGrid.
         * @param sender: El botón "Rango" dentro de la fila del DataGrid.
         * @param e: Argumentos del evento de click.
         **/
        private void btnCambiarRol_Click(object sender, RoutedEventArgs e)
        {
            
            var usuarioAEditar = (sender as Button)?.DataContext as Usuario;

      
            if (usuarioAEditar != null && _adminLogueado != null)
            {
                string nuevoRol = (usuarioAEditar.Rol == TipoRol.Admin) ? "Usuario" : "Administrador";

                if (usuarioAEditar.Rol == TipoRol.Admin)
                {
                    var numAdmins = DatabaseServicie.GetConexion()?.Table<Usuario>().Count(u => u.Rol == TipoRol.Admin) ?? 0;
                    if (numAdmins <= 1)
                    {
                        MessageBox.Show("Acción denegada: No se puede quitar al último administrador del sistema.",
                            "Error de Seguridad", MessageBoxButton.OK, MessageBoxImage.Stop);
                        return;
                    }
                }

                
                string aviso = $"ATENCIÓN: Se va a cambiar el rol de '{usuarioAEditar.Nombreusuario}' a {nuevoRol}.";

             
                var ventanaPass = new VentanaPassword(aviso);

                if (ventanaPass.ShowDialog() == true)
                {
                    string passIntroducida = ventanaPass.Password;

                
                    if (Cifrado.VerifyPassword(passIntroducida, _adminLogueado.Contrasenia))
                    {
                    
                        usuarioAEditar.Rol = (usuarioAEditar.Rol == TipoRol.Admin) ? TipoRol.Usuario : TipoRol.Admin;
                        DatabaseServicie.GetConexion()?.Update(usuarioAEditar);


                        dgUsuarios.Items.Refresh(); ;
                        CargarTablas();

                        MessageBox.Show($"¡Éxito! {usuarioAEditar.Nombreusuario} ahora es {nuevoRol}.", "Actualizado");
                    }
                    else
                    {
                        MessageBox.Show("Contraseña incorrecta. Acción cancelada.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        /**
         * Metodo Filtro Administradores Click:
         * Filtra la vista del DataGrid para mostrar exclusivamente a los usuarios con privilegios de administrador.
         * 1. Consulta: Accede a la base de datos y utiliza una expresión Lambda (.Where) para filtrar por TipoRol.Admin.
         * 2. Actualización de Interfaz: Reinicia el ItemsSource del DataGrid para forzar el refresco con la lista filtrada.
         * 3. Feedback Visual: Reduce la opacidad del botón para indicar de forma intuitiva que el filtro está activo.
         * @param sender: El botón de filtro de administradores.
         * @param e: Argumentos del evento de click.
         **/
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
        /**
         * Metodo Ver Todos Click:
         * Restablece la vista completa de la tabla de usuarios eliminando cualquier filtro activo.
         * 1. Restauración Visual: Devuelve la opacidad original (1.0) al botón de filtro para indicar que no hay restricciones.
         * 2. Recarga de Datos: Llama al método CargarTablas() para realizar una nueva consulta total a la base de datos.
         * 3. Sincronización: Actualiza el DataGrid y los contadores superiores con la información completa de usuarios.
         * @param sender: El botón "Ver Todos".
         * @param e: Argumentos del evento de click.
         **/
        private void btnVerTodos_Click(object sender, RoutedEventArgs e)
        {

            btnFiltroAdmins.Opacity = 1.0;
            CargarTablas();
        }
        //_________________________________________ZONA DE PELICULAS__________________________________________________________________________

        /**
         * Metodo Eliminar Pelicula Click:
         * Gestiona la eliminación de un título del catálogo mediante validación de credenciales.
         * 1. Preparación: Identifica la película seleccionada y genera un mensaje de advertencia.
         * 2. Seguridad: Despliega la ventana de PasswordBox para interceptar la clave del administrador actual.
         * 3. Verificación: Valida el hash SHA256 antes de proceder con la llamada al servicio de datos.
         * 4. Actualización: Ejecuta el borrado, limpia la selección del DataGrid y refresca la lista de películas.
         * @param sender: El botón de eliminar situado en la fila de la película.
         * @param e: Argumentos del evento de click.
         **/
        private void btnEliminarPelicula_Click(object sender, RoutedEventArgs e)
        {
            var peliculaAEliminar = (sender as Button)?.DataContext as Pelicula;

        
            if (peliculaAEliminar != null && _adminLogueado != null)
            {
           
                string aviso = $"¿Estás seguro de que deseas eliminar permanentemente la película '{peliculaAEliminar.Titulo}'?";

              
                var ventanaPass = new VentanaPassword(aviso);

              
                if (ventanaPass.ShowDialog() == true)
                {
                    string passIntroducida = ventanaPass.Password;

                    if (Cifrado.VerifyPassword(passIntroducida, _adminLogueado.Contrasenia))
                    {
                     
                        DatabaseServicie.BorrarPelicula(TipoRol.Admin, peliculaAEliminar.Id);
                        dgPeliculas.ItemsSource = null;
                        CargarTablas();

                        MessageBox.Show($"La película '{peliculaAEliminar.Titulo}' ha sido eliminada del catálogo.", "Operación Exitosa");
                    }
                    else
                    {
                        MessageBox.Show("Contraseña incorrecta. No se ha podido eliminar la película.", "Error de Autenticación", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        /**
         * Metodo Nueva Pelicula Click:
         * Gestiona la apertura del formulario de creación y la actualización de la interfaz tras el registro.
         * 1. Instanciación: Crea una nueva ventana de AniadirPeliculas_Panel pasando el rol del administrador.
         * 2. Interacción: Despliega la ventana en modo diálogo (bloqueante) para la entrada de datos.
         * 3. Validación: Evalúa si el resultado del diálogo fue exitoso (true) tras el guardado.
         * 4. Sincronización: Invoca a CargarTablas para refrescar el DataGrid con la nueva información.
         * @param sender: El objeto que dispara el evento (botón Nueva Película).
         * @param e: Argumentos del evento de interacción del usuario.
         **/
        private void btnNuevaPelicula_Click(object sender, RoutedEventArgs e)
        {
            
            var win = new AniadirPeliculas_Panel(_adminLogueado.Rol);

            if (win.ShowDialog() == true)
            {
             
                CargarTablas();
            }
        }
        /**
         * Metodo Cargar Peliculas Filtradas:
         * Gestiona la obtención, filtrado por géneros y ordenación dinámica del catálogo de películas.
         * 1. Conexión: Establece comunicación con el servicio de base de datos y valida la disponibilidad.
         * 2. Recuperación: Obtiene la colección completa de títulos almacenados en la tabla de Películas.
         * 3. Filtrado: Aplica restricciones por género si la opción "Todos" está desactivada y hay una selección activa.
         * 4. Ordenación: Clasifica la lista resultante según el criterio actual (ID o visualizaciones ascendentes/descendentes).
         * 5. Actualización: Asigna la colección final filtrada y ordenada al ItemsSource del DataGrid para su visualización.
         **/
        private void CargarPeliculasFiltradas()
        {
            var conexion = DatabaseServicie.GetConexion();
            if (conexion == null) return;

            var lista = conexion.Table<Pelicula>().ToList();

            // FILTRO DE GÉNEROS
            if (chkTodos.IsChecked == false && _generosSeleccionados.Count > 0)
            {
                lista = lista.Where(p => _generosSeleccionados.Contains(p.Genero)).ToList();
            }

            // LÓGICA DE ORDEN SEGÚN EL BOTÓN PULSADO
            switch (_criterioOrden)
            {
                case "ID":
                    lista = lista.OrderBy(p => p.Id).ToList();
                    break;
                case "VistasDesc":
                    lista = lista.OrderByDescending(p => p.CantVisualizaciones).ToList();
                    break;
                case "VistasAsc":
                    lista = lista.OrderBy(p => p.CantVisualizaciones).ToList();
                    break;
            }

            dgPeliculas.ItemsSource = lista;
        }

        // EVENTOS DE LOS BOTONES
        /**
         * Metodo Mas Vistas Click:
         * Configura el orden de visualización para priorizar los títulos con mayor número de reproducciones.
         * 1. Criterio: Actualiza la variable de control de orden al estado de visualizaciones descendentes.
         * 2. Refresco: Ejecuta la lógica de filtrado y ordenación para actualizar el DataGrid inmediatamente.
         * @param sender: El botón de ordenación por popularidad pulsado.
         * @param e: Argumentos del evento de click.
         **/
        private void btnMasVistas_Click(object sender, RoutedEventArgs e)
        {
            _criterioOrden = "VistasDesc";
            CargarPeliculasFiltradas();
        }
        /**
         * Metodo Menos Vistas Click:
         * Configura el orden de visualización para priorizar los títulos con menor número de reproducciones.
         * 1. Criterio: Actualiza la variable de control de orden al estado de visualizaciones ascendentes.
         * 2. Refresco: Ejecuta la lógica de filtrado y ordenación para actualizar el DataGrid inmediatamente.
         * @param sender: El botón de ordenación por menor popularidad pulsado.
         * @param e: Argumentos del evento de click.
         **/
        private void btnMenosVistas_Click(object sender, RoutedEventArgs e)
        {
            _criterioOrden = "VistasAsc";
            CargarPeliculasFiltradas();
        }
        /**
         * Metodo Orden ID Click:
         * Configura el orden de visualización para organizar los títulos según su identificador único.
         * 1. Criterio: Establece la variable de control de orden al estado por defecto basado en el ID.
         * 2. Refresco: Ejecuta la lógica de filtrado y ordenación para actualizar el DataGrid inmediatamente.
         * @param sender: El botón de ordenación por ID pulsado.
         * @param e: Argumentos del evento de click.
         **/
        private void btnOrdenID_Click(object sender, RoutedEventArgs e)
        {
            _criterioOrden = "ID";
            CargarPeliculasFiltradas();
        }
        /**
         * Metodo Checkbox Todos Click:
         * Gestiona la selección global del catálogo desmarcando los filtros individuales de género.
         * 1. Validación: Verifica si el estado del control es "Checked" para iniciar la limpieza.
         * 2. Control de Estado: Activa la bandera _cargandoFiltros para prevenir disparos de eventos en cascada.
         * 3. Limpieza de Datos: Vacía la colección de géneros seleccionados en la lógica de negocio.
         * 4. Interfaz Visual: Recorre el ItemsControl de géneros, localiza cada CheckBox mediante el árbol visual y fuerza su desmarcado.
         * 5. Sincronización: Restablece el control de eventos y refresca el DataGrid con el catálogo completo.
         * @param sender: El CheckBox "Todos" que dispara el evento.
         * @param e: Argumentos del evento de interacción.
         **/
        private void chkTodos_Click(object sender, RoutedEventArgs e)
        {
            // 1. Verificamos si se ha marcado el CheckBox
            if (chkTodos.IsChecked == true)
            {
                // 2. Bloqueamos temporalmente los eventos para evitar bucles infinitos
                _cargandoFiltros = true;

                // 3. Limpiamos la lista de géneros seleccionados
                _generosSeleccionados.Clear();

                // 4. Recorremos visualmente los CheckBoxes de los géneros para desmarcarlos
                foreach (var item in icGeneros.Items)
                {
                    // Buscamos el contenedor visual de cada género
                    var container = icGeneros.ItemContainerGenerator.ContainerFromItem(item) as ContentPresenter;
                    if (container != null)
                    {
                        // Buscamos el CheckBox dentro del DataTemplate
                        var checkbox = VisualTreeHelper.GetChild(container, 0) as CheckBox;
                        if (checkbox != null)
                        {
                            checkbox.IsChecked = false;
                        }
                    }
                }

                // 5. Desbloqueamos y refrescamos la tabla
                _cargandoFiltros = false;
                CargarPeliculasFiltradas();
            }
        }
        /**
         * Metodo Filtro Genero Changed:
         * Se dispara tanto al marcar (Checked) como al desmarcar (Unchecked) un genero.
         **/
        private void FiltroGenero_Changed(object sender, RoutedEventArgs e)
        {
            // 1. Evitamos que se ejecute mientras inicializamos los componentes
            if (_cargandoFiltros) return;

            var chk = sender as CheckBox;
            if (chk == null) return;

            // El Content del CheckBox contiene el valor del Enum (ej: Terror)
            var genero = (GeneroPelicula)chk.Content;

            if (chk.IsChecked == true)
            {
                // Si el usuario marca un genero especifico, quitamos "Ver Todas"
                chkTodos.IsChecked = false;

                // Añadimos el genero a la lista de filtros activos si no esta
                if (!_generosSeleccionados.Contains(genero))
                {
                    _generosSeleccionados.Add(genero);
                }
            }
            else
            {
                // Si desmarca el genero, lo quitamos de la lista
                _generosSeleccionados.Remove(genero);
            }

         
            if (_generosSeleccionados.Count == 0)
            {
                chkTodos.IsChecked = true;
            }
            CargarPeliculasFiltradas();
        }

        /**
         * Metodo Cerrar Sesion Click:
         * Finaliza la sesión del usuario actual y redirige la interfaz a la pantalla de autenticación.
         * 1. Localización: Obtiene una referencia a la ventana principal (MainWindow) que contiene el control actual.
         * 2. Navegación: Invoca el método de intercambio de vistas para cargar una nueva instancia de UC_Login.
         * 3. Seguridad: Al navegar fuera del panel de control, se invalidan los accesos a las funciones de administración.
         * @param sender: El botón o elemento de menú "Cerrar Sesión" que dispara el evento.
         * @param e: Argumentos del evento de click.
         **/

        private void CerrarSesion_Click(object sender, RoutedEventArgs e)
        {
           
            var main = Window.GetWindow(this) as MainWindow;
            main?.Navegar(new UC_Login());
        }
    }
}
