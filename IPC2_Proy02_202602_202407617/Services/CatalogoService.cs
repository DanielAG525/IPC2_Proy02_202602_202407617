using IPC2_Proy02_202602_202407617.Estructuras;
using IPC2_Proy02_202602_202407617.Models;

namespace IPC2_Proy02_202602_202407617.Services
{
    public class CatalogoService
    {
        private static CatalogoService instancia;

        public ArbolLibros Libros { get; private set; }

        public ArbolCategorias Categorias { get; private set; }

        private CatalogoService()
        {
            Inicializar();
        }

        // =========================
        // INSTANCIA
        // =========================

        public static CatalogoService ObtenerInstancia()
        {
            if (instancia == null)
            {
                instancia = new CatalogoService();
            }

            return instancia;
        }

        // =========================
        // INICIALIZAR
        // =========================

        public void Inicializar()
        {
            Libros = new ArbolLibros();
            Categorias = new ArbolCategorias();
        }

        // =========================
        // AGREGAR CATEGORIA
        // =========================

        public bool AgregarCategoria(
            string nombre,
            string padre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                return false;
            }

            return Categorias.Insertar(
                nombre,
                padre);
        }

        // =========================
        // AGREGAR LIBRO
        // =========================

        public bool AgregarLibro(
            int isbn,
            string titulo,
            string autor,
            string categoria)
        {
            if (isbn <= 0)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(titulo) ||
                string.IsNullOrWhiteSpace(autor) ||
                string.IsNullOrWhiteSpace(categoria))
            {
                return false;
            }

            if (Categorias.Buscar(categoria) == null)
            {
                return false;
            }

            Libro nuevo =
                new Libro(
                    isbn,
                    titulo,
                    autor,
                    categoria);

            return Libros.Insertar(nuevo);
        }

        // =========================
        // BUSCAR LIBRO
        // =========================

        public Libro BuscarLibro(int isbn)
        {
            return Libros.Buscar(isbn);
        }

        // =========================
        // ELIMINAR LIBRO
        // =========================

        public bool EliminarLibro(int isbn)
        {
            return Libros.Eliminar(isbn);
        }

        // =========================
        // MENOR ISBN
        // =========================

        public Libro ObtenerMenorISBN()
        {
            return Libros.ObtenerMenor();
        }

        // =========================
        // MAYOR ISBN
        // =========================

        public Libro ObtenerMayorISBN()
        {
            return Libros.ObtenerMayor();
        }

        // =========================
        // LIBROS POR CATEGORIA
        // =========================

        public string ObtenerLibrosPorCategoria(
            string categoria)
        {
            if (string.IsNullOrWhiteSpace(categoria))
            {
                return "";
            }

            if (Categorias.Buscar(categoria) == null)
            {
                return "";
            }

            return Libros.ObtenerLibrosPorCategoria(
                categoria);
        }

        // =========================
        // GRAPHVIZ POR CATEGORIA
        // =========================

        public string GenerarDotPorCategoria(
            string categoria)
        {
            if (string.IsNullOrWhiteSpace(categoria))
            {
                return "";
            }

            if (Categorias.Buscar(categoria) == null)
            {
                return "";
            }

            return Libros.GenerarDotPorCategoria(
                categoria);
        }
    }
}