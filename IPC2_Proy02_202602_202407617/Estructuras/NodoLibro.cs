using IPC2_Proy02_202602_202407617.Models;

namespace IPC2_Proy02_202602_202407617.Estructuras
{
    public class NodoLibro
    {
        public Libro Libro { get; set; }

        public NodoLibro Izquierda { get; set; }

        public NodoLibro Derecha { get; set; }

        public NodoLibro(Libro libro)
        {
            Libro = libro;
            Izquierda = null;
            Derecha = null;
        }
    }
}