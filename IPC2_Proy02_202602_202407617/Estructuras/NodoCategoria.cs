using IPC2_Proy02_202602_202407617.Models;

namespace IPC2_Proy02_202602_202407617.Estructuras
{
    public class NodoCategoria
    {
        public Categoria Categoria { get; set; }

        public NodoCategoria PrimerHijo { get; set; }

        public NodoCategoria SiguienteHermano { get; set; }

        public NodoCategoria(Categoria categoria)
        {
            Categoria = categoria;
            PrimerHijo = null;
            SiguienteHermano = null;
        }
    }
}