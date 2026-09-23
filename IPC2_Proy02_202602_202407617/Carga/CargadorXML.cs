using System;
using System.Xml;
using IPC2_Proy02_202602_202407617.Services;

namespace IPC2_Proy02_202602_202407617.Carga
{
    public class CargadorXML
    {
        private readonly CatalogoService catalogo;

        public CargadorXML()
        {
            catalogo =
                CatalogoService.ObtenerInstancia();
        }

        public string Cargar(string ruta)
        {
            try
            {
                XmlDocument documento =
                    new XmlDocument();

                documento.Load(ruta);

                XmlNode raiz =
                    documento.SelectSingleNode("/config");

                if (raiz == null)
                {
                    return "El archivo XML no contiene el nodo config.";
                }

                XmlNode listaCategorias =
                    raiz.SelectSingleNode("listaCategorias");

                if (listaCategorias != null)
                {
                    CargarCategorias(listaCategorias);
                }

                XmlNode listaLibros =
                    raiz.SelectSingleNode("listaLibros");

                if (listaLibros != null)
                {
                    CargarLibros(listaLibros);
                }

                return "Archivo cargado correctamente.";
            }
            catch (Exception ex)
            {
                return "Error al cargar XML: " + ex.Message;
            }
        }

        private void CargarCategorias(
            XmlNode listaCategorias)
        {
            foreach (XmlNode nodo in listaCategorias.ChildNodes)
            {
                if (nodo.Name != "categoria")
                {
                    continue;
                }

                string nombre =
                    nodo.InnerText.Trim();

                string padre = "";

                if (nodo.Attributes != null &&
                    nodo.Attributes["padre"] != null)
                {
                    padre =
                        nodo.Attributes["padre"]
                        .Value
                        .Trim();
                }

                catalogo.AgregarCategoria(
                    nombre,
                    padre);
            }
        }

        private void CargarLibros(
            XmlNode listaLibros)
        {
            foreach (XmlNode nodo in listaLibros.ChildNodes)
            {
                if (nodo.Name != "libro")
                {
                    continue;
                }

                XmlNode nodoISBN =
                    nodo.SelectSingleNode("ISBN");

                XmlNode nodoTitulo =
                    nodo.SelectSingleNode("titulo");

                XmlNode nodoAutor =
                    nodo.SelectSingleNode("autor");

                XmlNode nodoCategoria =
                    nodo.SelectSingleNode("categoria");

                if (nodoISBN == null ||
                    nodoTitulo == null ||
                    nodoAutor == null ||
                    nodoCategoria == null)
                {
                    continue;
                }

                int isbn;

                if (!int.TryParse(
                    nodoISBN.InnerText.Trim(),
                    out isbn))
                {
                    continue;
                }

                string titulo =
                    nodoTitulo.InnerText.Trim();

                string autor =
                    nodoAutor.InnerText.Trim();

                string categoria =
                    nodoCategoria.InnerText.Trim();

                catalogo.AgregarLibro(
                    isbn,
                    titulo,
                    autor,
                    categoria);
            }
        }
    }
}