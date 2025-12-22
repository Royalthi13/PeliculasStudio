# 🎬 PeliculasStudio

**Gestor de Tráilers y Catálogo de Cine desarrollado en WPF y .NET**

Este proyecto es una aplicación de escritorio para gestionar una colección de películas y reproducir sus tráilers. Utiliza tecnologías modernas como **SQLite Code-First** y el motor de reproducción **VLC** embebido.

---

## 🚀 Características Principales

* **📺 Reproductor de Vídeo:** Integración de `LibVLCSharp`.
* **💾 Base de Datos Automática:** No requiere instalación de servidores. La base de datos (`.db`) se crea y se autoconfigura sola al iniciar la app (Code First).
* **🔐 Sistema de Login:** Control de acceso de usuarios.
* **📂 Gestión de Contenido:** Catálogo visual de películas.
* **🎨 Interfaz Moderna:** Diseño limpio y aplicado a usabilidad.

---

## 🛠️ Tecnologías Utilizadas

* **Lenguaje:** C# (.NET 8.0)
* **Framework:** WPF (Windows Presentation Foundation)
* **Base de Datos:** SQLite (vía `sqlite-net-pcl` de Frank A. Krueger)
* **Reproductor:** LibVLCSharp (Wrapper de VLC para .NET)
* **IDE Recomendado:** Visual Studio 2022

---

## ⚙️ Instalación y Puesta en Marcha

Sigue estos pasos para ejecutar el proyecto en tu máquina local. ¡No necesitas instalar MySQL ni SQL Server!

1.  **Clonar el repositorio:**
    ```bash
    git clone [https://github.com/TU_USUARIO/PeliculasStudio.git](https://github.com/TU_USUARIO/PeliculasStudio.git)
    ```
2.  **Abrir el proyecto:**
    Abre el archivo `PeliculasStudio.sln` con Visual Studio.
3.  **Restaurar paquetes:**
    Al abrirlo, Visual Studio debería descargar automáticamente las dependencias NuGet (`sqlite-net-pcl`, `LibVLCSharp`, etc.). Si no, ve a *Herramientas -> Administrador de paquetes NuGet -> Restaurar*.
4.  **Ejecutar:**
    Dale al botón **Iniciar** (Play ▶️).
    


## 📂 Estructura del Proyecto

* **`/BaseDatos`**: Lógica de conexión y creación automática de tablas.
* **`/Modelos`**: Clases POCO que definen las tablas (Pelicula, Usuario...).
* **`/Vistas`**: Archivos XAML con la interfaz gráfica (Login, Catálogo, Detalle).
* **`/Assets`**: Recursos estáticos (Imágenes y Vídeos de los tráilers).

---

## 👥 Autores

Proyecto realizado por alumnos de 2º DAM:

* **Adrián Muñoz**
* **Oscar** 
* **Perdices** 

---

> 📝 **Nota para corrección:** La base de datos no se sube al repositorio para evitar conflictos binarios. Se genera localmente en cada equipo `.