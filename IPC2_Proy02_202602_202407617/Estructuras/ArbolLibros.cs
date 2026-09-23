using System.Text;
using IPC2_Proy02_202602_202407617.Models;

namespace IPC2_Proy02_202602_202407617.Estructuras
{
    public class ArbolLibros
    {
        public NodoLibro Raiz { get; private set; }

        public ArbolLibros()
        {
            Raiz = null;
        }

        // =========================
        // INSERTAR
        // =========================

        public bool Insertar(Libro libro)
        {
            if (libro == null)
            {
                return false;
            }

            if (Raiz == null)
            {
                Raiz = new NodoLibro(libro);
                return true;
            }

            NodoLibro actual = Raiz;

            while (actual != null)
            {
                if (libro.ISBN == actual.Libro.ISBN)
                {
                    return false;
                }

                if (libro.ISBN < actual.Libro.ISBN)
                {
                    if (actual.Izquierda == null)
                    {
                        actual.Izquierda = new NodoLibro(libro);
                        return true;
                    }

                    actual = actual.Izquierda;
                }
                else
                {
                    if (actual.Derecha == null)
                    {
                        actual.Derecha = new NodoLibro(libro);
                        return true;
                    }

                    actual = actual.Derecha;
                }
            }

            return false;
        }

        // =========================
        // BUSCAR
        // =========================

        public Libro Buscar(int isbn)
        {
            NodoLibro actual = Raiz;

            while (actual != null)
            {
                if (isbn == actual.Libro.ISBN)
                {
                    return actual.Libro;
                }

                if (isbn < actual.Libro.ISBN)
                {
                    actual = actual.Izquierda;
                }
                else
                {
                    actual = actual.Derecha;
                }
            }

            return null;
        }

        // =========================
        // MENOR ISBN
        // =========================

        public Libro ObtenerMenor()
        {
            if (Raiz == null)
            {
                return null;
            }

            NodoLibro actual = Raiz;

            while (actual.Izquierda != null)
            {
                actual = actual.Izquierda;
            }

            return actual.Libro;
        }

        // =========================
        // MAYOR ISBN
        // =========================

        public Libro ObtenerMayor()
        {
            if (Raiz == null)
            {
                return null;
            }

            NodoLibro actual = Raiz;

            while (actual.Derecha != null)
            {
                actual = actual.Derecha;
            }

            return actual.Libro;
        }

        // =========================
        // ELIMINAR
        // =========================

        public bool Eliminar(int isbn)
        {
            bool eliminado = false;

            Raiz = EliminarRecursivo(
                Raiz,
                isbn,
                ref eliminado);

            return eliminado;
        }

        private NodoLibro EliminarRecursivo(
            NodoLibro nodo,
            int isbn,
            ref bool eliminado)
        {
            if (nodo == null)
            {
                return null;
            }

            if (isbn < nodo.Libro.ISBN)
            {
                nodo.Izquierda =
                    EliminarRecursivo(
                        nodo.Izquierda,
                        isbn,
                        ref eliminado);
            }
            else if (isbn > nodo.Libro.ISBN)
            {
                nodo.Derecha =
                    EliminarRecursivo(
                        nodo.Derecha,
                        isbn,
                        ref eliminado);
            }
            else
            {
                eliminado = true;

                if (nodo.Izquierda == null)
                {
                    return nodo.Derecha;
                }

                if (nodo.Derecha == null)
                {
                    return nodo.Izquierda;
                }

                NodoLibro sucesor =
                    ObtenerNodoMenor(nodo.Derecha);

                nodo.Libro = sucesor.Libro;

                bool auxiliar = false;

                nodo.Derecha =
                    EliminarRecursivo(
                        nodo.Derecha,
                        sucesor.Libro.ISBN,
                        ref auxiliar);
            }

            return nodo;
        }

        private NodoLibro ObtenerNodoMenor(NodoLibro nodo)
        {
            NodoLibro actual = nodo;

            while (actual.Izquierda != null)
            {
                actual = actual.Izquierda;
            }

            return actual;
        }

        // =========================
        // RECORRIDO INORDEN
        // =========================

        public string ObtenerInOrden()
        {
            StringBuilder resultado =
                new StringBuilder();

            InOrdenRecursivo(
                Raiz,
                resultado);

            return resultado.ToString();
        }

        private void InOrdenRecursivo(
            NodoLibro nodo,
            StringBuilder resultado)
        {
            if (nodo == null)
            {
                return;
            }

            InOrdenRecursivo(
                nodo.Izquierda,
                resultado);

            resultado.AppendLine(
                nodo.Libro.ISBN + " | " +
                nodo.Libro.Titulo + " | " +
                nodo.Libro.Autor + " | " +
                nodo.Libro.Categoria);

            InOrdenRecursivo(
                nodo.Derecha,
                resultado);
        }

        // =========================
        // LIBROS PARA MOSTRAR
        // =========================

        public string ObtenerLibros()
        {
            StringBuilder resultado =
                new StringBuilder();

            ObtenerLibrosRecursivo(
                Raiz,
                resultado);

            return resultado.ToString();
        }

        private void ObtenerLibrosRecursivo(
            NodoLibro nodo,
            StringBuilder resultado)
        {
            if (nodo == null)
            {
                return;
            }

            ObtenerLibrosRecursivo(
                nodo.Izquierda,
                resultado);

            resultado.AppendLine(
                nodo.Libro.ISBN + "|" +
                nodo.Libro.Titulo + "|" +
                nodo.Libro.Autor + "|" +
                nodo.Libro.Categoria);

            ObtenerLibrosRecursivo(
                nodo.Derecha,
                resultado);
        }

        // =========================
        // LIBROS POR CATEGORIA
        // =========================

        public string ObtenerLibrosPorCategoria(
            string categoria)
        {
            StringBuilder resultado =
                new StringBuilder();

            ObtenerLibrosPorCategoriaRecursivo(
                Raiz,
                categoria,
                resultado);

            return resultado.ToString();
        }

        private void ObtenerLibrosPorCategoriaRecursivo(
            NodoLibro nodo,
            string categoria,
            StringBuilder resultado)
        {
            if (nodo == null)
            {
                return;
            }

            ObtenerLibrosPorCategoriaRecursivo(
                nodo.Izquierda,
                categoria,
                resultado);

            if (string.Equals(
                nodo.Libro.Categoria,
                categoria,
                System.StringComparison.OrdinalIgnoreCase))
            {
                resultado.AppendLine(
                    nodo.Libro.ISBN + " | " +
                    nodo.Libro.Titulo + " | " +
                    nodo.Libro.Autor + " | " +
                    nodo.Libro.Categoria);
            }

            ObtenerLibrosPorCategoriaRecursivo(
                nodo.Derecha,
                categoria,
                resultado);
        }

        // =========================
        // GRAPHVIZ ARBOL COMPLETO
        // =========================

        public string GenerarDot()
        {
            StringBuilder dot =
                new StringBuilder();

            dot.AppendLine(
                "digraph ArbolLibros {");

            dot.AppendLine(
                "rankdir=TB;");

            dot.AppendLine(
                "node [shape=record];");

            GenerarDotRecursivo(
                Raiz,
                dot);

            dot.AppendLine("}");

            return dot.ToString();
        }

        private void GenerarDotRecursivo(
            NodoLibro nodo,
            StringBuilder dot)
        {
            if (nodo == null)
            {
                return;
            }

            string titulo =
                Escapar(nodo.Libro.Titulo);

            string autor =
                Escapar(nodo.Libro.Autor);

            dot.AppendLine(
                "n" + nodo.Libro.ISBN +
                " [label=\"{" +
                nodo.Libro.ISBN +
                "|" + titulo +
                "|" + autor +
                "}\"];");

            if (nodo.Izquierda != null)
            {
                dot.AppendLine(
                    "n" + nodo.Libro.ISBN +
                    " -> n" +
                    nodo.Izquierda.Libro.ISBN +
                    ";");
            }

            if (nodo.Derecha != null)
            {
                dot.AppendLine(
                    "n" + nodo.Libro.ISBN +
                    " -> n" +
                    nodo.Derecha.Libro.ISBN +
                    ";");
            }

            GenerarDotRecursivo(
                nodo.Izquierda,
                dot);

            GenerarDotRecursivo(
                nodo.Derecha,
                dot);
        }

        // =========================
        // GRAPHVIZ POR CATEGORIA
        // =========================

        public string GenerarDotPorCategoria(
            string categoria)
        {
            StringBuilder dot =
                new StringBuilder();

            dot.AppendLine(
                "digraph LibrosCategoria {");

            dot.AppendLine(
                "rankdir=TB;");

            dot.AppendLine(
                "node [shape=record];");

            int isbnAnterior = -1;

            GenerarDotCategoriaRecursivo(
                Raiz,
                categoria,
                dot,
                ref isbnAnterior);

            dot.AppendLine("}");

            return dot.ToString();
        }

        private void GenerarDotCategoriaRecursivo(
            NodoLibro nodo,
            string categoria,
            StringBuilder dot,
            ref int isbnAnterior)
        {
            if (nodo == null)
            {
                return;
            }

            GenerarDotCategoriaRecursivo(
                nodo.Izquierda,
                categoria,
                dot,
                ref isbnAnterior);

            if (string.Equals(
                nodo.Libro.Categoria,
                categoria,
                System.StringComparison.OrdinalIgnoreCase))
            {
                string titulo =
                    Escapar(nodo.Libro.Titulo);

                string autor =
                    Escapar(nodo.Libro.Autor);

                dot.AppendLine(
                    "n" + nodo.Libro.ISBN +
                    " [label=\"{" +
                    nodo.Libro.ISBN +
                    "|" + titulo +
                    "|" + autor +
                    "}\"];");

                if (isbnAnterior != -1)
                {
                    dot.AppendLine(
                        "n" + isbnAnterior +
                        " -> n" +
                        nodo.Libro.ISBN +
                        ";");
                }

                isbnAnterior =
                    nodo.Libro.ISBN;
            }

            GenerarDotCategoriaRecursivo(
                nodo.Derecha,
                categoria,
                dot,
                ref isbnAnterior);
        }

        // =========================
        // ESCAPAR TEXTO GRAPHVIZ
        // =========================

        private string Escapar(string texto)
        {
            if (texto == null)
            {
                return "";
            }

            return texto
                .Replace("\\", "\\\\")
                .Replace("\"", "\\\"");
        }
    }
}