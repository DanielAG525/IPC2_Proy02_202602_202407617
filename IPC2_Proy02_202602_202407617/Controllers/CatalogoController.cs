using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using IPC2_Proy02_202602_202407617.Services;
using IPC2_Proy02_202602_202407617.Carga;
using IPC2_Proy02_202602_202407617.Graphviz;

namespace IPC2_Proy02_202602_202407617.Controllers
{
    public class CatalogoController : Controller
    {
        private readonly CatalogoService servicio;

        public CatalogoController()
        {
            servicio = CatalogoService.ObtenerInstancia();
        }

        // =========================
        // INICIO
        // =========================

        public ActionResult Index()
        {
            return View();
        }

        // =========================
        // LIBROS
        // =========================

        public ActionResult Libros()
        {
            ViewBag.Libros =
                servicio.Libros.ObtenerLibros();

            return View();
        }

        // =========================
        // CATEGORIAS
        // =========================

        public ActionResult Categorias()
        {
            ViewBag.Categorias =
                servicio.Categorias.ObtenerEstructura();

            return View();
        }

        // =========================
        // CARGAR XML
        // =========================

        [HttpGet]
        public ActionResult Cargar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Cargar(
            HttpPostedFileBase archivo)
        {
            if (archivo == null ||
                archivo.ContentLength == 0)
            {
                ViewBag.Mensaje =
                    "Seleccione un archivo XML.";

                return View();
            }

            try
            {
                string extension =
                    Path.GetExtension(
                        archivo.FileName);

                if (!extension.Equals(
                    ".xml",
                    StringComparison.OrdinalIgnoreCase))
                {
                    ViewBag.Mensaje =
                        "El archivo debe tener extensión XML.";

                    return View();
                }

                string nombreTemporal =
                    Guid.NewGuid().ToString() +
                    ".xml";

                string ruta =
                    Path.Combine(
                        Server.MapPath(
                            "~/App_Data"),
                        nombreTemporal);

                archivo.SaveAs(ruta);

                CargadorXML cargador =
                    new CargadorXML();

                string resultado =
                    cargador.Cargar(ruta);

                ViewBag.Mensaje =
                    resultado;

                if (System.IO.File.Exists(ruta))
                {
                    System.IO.File.Delete(ruta);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje =
                    "Error al procesar archivo: " +
                    ex.Message;
            }

            return View();
        }

        // =========================
        // BUSCAR LIBRO
        // =========================

        [HttpGet]
        public ActionResult Buscar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Buscar(int isbn)
        {
            ViewBag.BusquedaRealizada =
                true;

            ViewBag.Libro =
                servicio.BuscarLibro(isbn);

            return View();
        }

        // =========================
        // AGREGAR CATEGORIA
        // =========================

        [HttpGet]
        public ActionResult AgregarCategoria()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AgregarCategoria(
            string nombre,
            string padre)
        {
            bool resultado =
                servicio.AgregarCategoria(
                    nombre,
                    padre);

            if (resultado)
            {
                ViewBag.Mensaje =
                    "Categoría agregada correctamente.";
            }
            else
            {
                ViewBag.Mensaje =
                    "No se pudo agregar la categoría. " +
                    "Verifique que no exista y que " +
                    "la categoría padre sea válida.";
            }

            return View();
        }

        // =========================
        // AGREGAR LIBRO
        // =========================

        [HttpGet]
        public ActionResult AgregarLibro()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AgregarLibro(
            int isbn,
            string titulo,
            string autor,
            string categoria)
        {
            bool resultado =
                servicio.AgregarLibro(
                    isbn,
                    titulo,
                    autor,
                    categoria);

            if (resultado)
            {
                ViewBag.Mensaje =
                    "Libro agregado correctamente.";
            }
            else
            {
                ViewBag.Mensaje =
                    "No se pudo agregar el libro. " +
                    "Verifique el ISBN y la categoría.";
            }

            return View();
        }

        // =========================
        // ELIMINAR LIBRO
        // =========================

        [HttpGet]
        public ActionResult EliminarLibro()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EliminarLibro(
            int isbn)
        {
            bool resultado =
                servicio.EliminarLibro(isbn);

            if (resultado)
            {
                ViewBag.Mensaje =
                    "Libro eliminado correctamente.";
            }
            else
            {
                ViewBag.Mensaje =
                    "No se encontró un libro con ese ISBN.";
            }

            return View();
        }

        // =========================
        // MENOR Y MAYOR ISBN
        // =========================

        public ActionResult ExtremosISBN()
        {
            ViewBag.Menor =
                servicio.ObtenerMenorISBN();

            ViewBag.Mayor =
                servicio.ObtenerMayorISBN();

            return View();
        }

        // =========================
        // LIBROS POR CATEGORIA
        // =========================

        [HttpGet]
        public ActionResult LibrosPorCategoria()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LibrosPorCategoria(
            string categoria)
        {
            ViewBag.BusquedaRealizada =
                true;

            ViewBag.Categoria =
                categoria;

            ViewBag.Libros =
                servicio.ObtenerLibrosPorCategoria(
                    categoria);

            if (!string.IsNullOrWhiteSpace(categoria) &&
                servicio.Categorias.Buscar(categoria) != null)
            {
                string carpeta =
                    Server.MapPath(
                        "~/Content/Reportes/");

                if (!Directory.Exists(carpeta))
                {
                    Directory.CreateDirectory(
                        carpeta);
                }

                string rutaDot =
                    Path.Combine(
                        carpeta,
                        "categoria_libros.dot");

                string rutaImagen =
                    Path.Combine(
                        carpeta,
                        "categoria_libros.png");

                string contenidoDot =
                    servicio.GenerarDotPorCategoria(
                        categoria);

                GeneradorGraphviz generador =
                    new GeneradorGraphviz();

                bool generado =
                    generador.GenerarImagen(
                        contenidoDot,
                        rutaDot,
                        rutaImagen);

                if (generado)
                {
                    ViewBag.Grafica =
                        Url.Content(
                            "~/Content/Reportes/" +
                            "categoria_libros.png?" +
                            DateTime.Now.Ticks);
                }
            }

            return View();
        }

        // =========================
        // REPORTES
        // =========================

        public ActionResult Reporte()
        {
            string carpeta =
                Server.MapPath(
                    "~/Content/Reportes/");

            if (!Directory.Exists(carpeta))
            {
                Directory.CreateDirectory(
                    carpeta);
            }

            string rutaDotLibros =
                Path.Combine(
                    carpeta,
                    "libros.dot");

            string rutaImagenLibros =
                Path.Combine(
                    carpeta,
                    "libros.png");

            string rutaDotCategorias =
                Path.Combine(
                    carpeta,
                    "categorias.dot");

            string rutaImagenCategorias =
                Path.Combine(
                    carpeta,
                    "categorias.png");

            GeneradorGraphviz generador =
                new GeneradorGraphviz();

            bool librosGenerados =
                generador.GenerarImagen(
                    servicio.Libros.GenerarDot(),
                    rutaDotLibros,
                    rutaImagenLibros);

            bool categoriasGeneradas =
                generador.GenerarImagen(
                    servicio.Categorias.GenerarDot(),
                    rutaDotCategorias,
                    rutaImagenCategorias);

            if (librosGenerados)
            {
                ViewBag.GraficaLibros =
                    Url.Content(
                        "~/Content/Reportes/" +
                        "libros.png?" +
                        DateTime.Now.Ticks);
            }

            if (categoriasGeneradas)
            {
                ViewBag.GraficaCategorias =
                    Url.Content(
                        "~/Content/Reportes/" +
                        "categorias.png?" +
                        DateTime.Now.Ticks);
            }

            return View();
        }

        // =========================
        // AYUDA
        // =========================

        public ActionResult Ayuda()
        {
            return View();
        }
    }
}