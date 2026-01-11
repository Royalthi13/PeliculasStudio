# 🎬 PeliculasStudio

**Gestor de Tráilers y Catálogo de Cine desarrollado en WPF y .NET**

Este proyecto es una aplicación de escritorio para gestionar una colección de películas y reproducir sus tráilers. Utiliza tecnologías modernas como una **base de datos embebida** y el motor de reproducción **VLC** embebido.

---

## 🚀 Características Principales

* **📺 Reproductor de Vídeo:** Integración de `LibVLCSharp`.
* **💾 Persistencia Local: Base de datos SQLite con generación automática de esquemas al iniciar la aplicación (sin necesidad de scripts SQL externos).
* **🔐 Sistema de Login:** Control de acceso de usuarios.
* **📂 Gestión de Contenido:** Catálogo visual de películas.
* **🎨 Interfaz Moderna:** Interfaz moderna y minimalista centrada en la usabilidad.
---

## 🛠️ Tecnologías Utilizadas

* **Lenguaje:** C# (.NET 8.0)
* **Framework:** WPF (Windows Presentation Foundation)
* **Base de Datos:** SQLite (vía `sqlite-net-pcl`)
* **Reproductor:** LibVLCSharp (Wrapper de VLC para .NET)
* **IDE Recomendado:** Visual Studio 2022

---

## ⚙️ Instalación y Puesta en Marcha

Sigue estos pasos para ejecutar el proyecto en tu equipo local. ¡No necesitas instalar MySQL ni SQL Server!

1.  **Clonar el repositorio:**
    ```bash
    git clone [https://github.com/TU_USUARIO/PeliculasStudio.git](https://github.com/TU_USUARIO/PeliculasStudio.git)
    ```
2.  **Abrir el proyecto:**
    Abre el archivo `PeliculasStudio.sln` con Visual Studio.
3.  **Restaurar paquetes:**
    Al abrirlo, Visual Studio debería descargar automáticamente las dependencias NuGet (`sqlite-net-pcl`, `LibVLCSharp`, etc.). Si no, ve a *Herramientas -> Administrador de paquetes NuGet -> Restaurar*.
4.  **Ejecutar:**
    Dale al botón **Iniciar**.
    


## 📂 Estructura del Proyecto

* **`/BaseDatos`**: Lógica de conexión y creación automática de tablas.
* **`/Modelos`**: Clases POCO que definen las tablas (Pelicula, Usuario...).
* **`/Vistas`**: Archivos XAML con la interfaz gráfica (Login, Catálogo, Detalle).
* **`/Assets`**: Recursos estáticos (Imágenes y Vídeos de los tráilers).

## ⚠️ Notas Importantes

* **Base de Datos**: El archivo `.db` se genera localmente para evitar conflictos binarios. La aplicación lo crea automáticamente en cada equipo al iniciarse.
* **Credenciales por defecto**: Para acceder al sistema sin registrarse, se puede utilizar el usuario `admin` y la contraseña `admin`.
* **Reproducción**: Se recomienda que los vídeos alojados en la carpeta `/Assets` utilizar el formato `.mp4` para garantizar la compatibilidad con  VLC.

---

## 👥 Autores

Proyecto realizado por alumnos de 2º DAM:

* **Adrián Muñoz**
* **Oscar** 
* **Perdices** 

---

