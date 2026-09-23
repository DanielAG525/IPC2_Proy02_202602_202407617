using System.Text;
using IPC2_Proy02_202602_202407617.Models;

namespace IPC2_Proy02_202602_202407617.Estructuras
{
    public class ArbolCategorias
    {
        public NodoCategoria Raiz { get; private set; }

        public ArbolCategorias()
        {
            Raiz = null;
        }

        public bool Insertar(
            string nombre,
            string nombrePadre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                return false;
            }

            if (Buscar(nombre) != null)
            {
                return false;
            }

            NodoCategoria nuevo =
                new NodoCategoria(
                    new Categoria(nombre));

            if (string.IsNullOrWhiteSpace(nombrePadre))
            {
                if (Raiz == null)
                {
                    Raiz = nuevo;
                    return true;
                }

                Raiz = InsertarOrdenadoComoHermano(
                    Raiz,
                    nuevo);

                return true;
            }

            NodoCategoria padre = Buscar(nombrePadre);

            if (padre == null)
            {
                return false;
            }

            padre.PrimerHijo = InsertarOrdenadoComoHermano(
                padre.PrimerHijo,
                nuevo);

            return true;
        }

        private NodoCategoria InsertarOrdenadoComoHermano(
            NodoCategoria inicio,
            NodoCategoria nuevo)
        {
            if (inicio == null)
            {
                return nuevo;
            }

            if (string.Compare(
                    nuevo.Categoria.Nombre,
                    inicio.Categoria.Nombre,
                    true) < 0)
            {
                nuevo.SiguienteHermano = inicio;
                return nuevo;
            }

            NodoCategoria actual = inicio;

            while (actual.SiguienteHermano != null &&
                   string.Compare(
                       nuevo.Categoria.Nombre,
                       actual.SiguienteHermano.Categoria.Nombre,
                       true) > 0)
            {
                actual = actual.SiguienteHermano;
            }

            nuevo.SiguienteHermano =
                actual.SiguienteHermano;

            actual.SiguienteHermano = nuevo;

            return inicio;
        }

        public NodoCategoria Buscar(string nombre)
        {
            return BuscarRecursivo(Raiz, nombre);
        }

        private NodoCategoria BuscarRecursivo(
            NodoCategoria nodo,
            string nombre)
        {
            NodoCategoria actual = nodo;

            while (actual != null)
            {
                if (string.Equals(
                    actual.Categoria.Nombre,
                    nombre,
                    System.StringComparison.OrdinalIgnoreCase))
                {
                    return actual;
                }

                NodoCategoria encontrado =
                    BuscarRecursivo(
                        actual.PrimerHijo,
                        nombre);

                if (encontrado != null)
                {
                    return encontrado;
                }

                actual = actual.SiguienteHermano;
            }

            return null;
        }

        public string ObtenerEstructura()
        {
            StringBuilder resultado =
                new StringBuilder();

            ObtenerEstructuraRecursiva(
                Raiz,
                0,
                resultado);

            return resultado.ToString();
        }

        private void ObtenerEstructuraRecursiva(
            NodoCategoria nodo,
            int nivel,
            StringBuilder resultado)
        {
            NodoCategoria actual = nodo;

            while (actual != null)
            {
                for (int i = 0; i < nivel; i++)
                {
                    resultado.Append("    ");
                }

                resultado.AppendLine(
                    actual.Categoria.Nombre);

                ObtenerEstructuraRecursiva(
                    actual.PrimerHijo,
                    nivel + 1,
                    resultado);

                actual = actual.SiguienteHermano;
            }
        }

        public string GenerarDot()
        {
            StringBuilder dot = new StringBuilder();

            dot.AppendLine("digraph Categorias {");
            dot.AppendLine("rankdir=TB;");
            dot.AppendLine("node [shape=box];");

            GenerarDotRecursivo(Raiz, dot);

            dot.AppendLine("}");

            return dot.ToString();
        }

        private void GenerarDotRecursivo(
            NodoCategoria nodo,
            StringBuilder dot)
        {
            NodoCategoria actual = nodo;

            while (actual != null)
            {
                string idPadre =
                    ObtenerId(actual.Categoria.Nombre);

                string nombre =
                    Escapar(actual.Categoria.Nombre);

                dot.AppendLine(
                    idPadre +
                    " [label=\"" +
                    nombre +
                    "\"];");

                NodoCategoria hijo =
                    actual.PrimerHijo;

                while (hijo != null)
                {
                    string idHijo =
                        ObtenerId(
                            hijo.Categoria.Nombre);

                    dot.AppendLine(
                        idPadre +
                        " -> " +
                        idHijo +
                        ";");

                    hijo = hijo.SiguienteHermano;
                }

                GenerarDotRecursivo(
                    actual.PrimerHijo,
                    dot);

                actual = actual.SiguienteHermano;
            }
        }

        private string ObtenerId(string texto)
        {
            StringBuilder id = new StringBuilder("cat_");

            for (int i = 0; i < texto.Length; i++)
            {
                char c = texto[i];

                if (char.IsLetterOrDigit(c))
                {
                    id.Append(c);
                }
                else
                {
                    id.Append('_');
                }
            }

            return id.ToString();
        }

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