using System;
using System.Diagnostics;
using System.IO;

namespace IPC2_Proy02_202602_202407617.Graphviz
{
    public class GeneradorGraphviz
    {
        public bool GenerarImagen(
            string contenidoDot,
            string rutaDot,
            string rutaImagen)
        {
            try
            {
                File.WriteAllText(
                    rutaDot,
                    contenidoDot);

                ProcessStartInfo info =
                    new ProcessStartInfo();

                info.FileName = "dot";

                info.Arguments =
                    "-Tpng \"" +
                    rutaDot +
                    "\" -o \"" +
                    rutaImagen +
                    "\"";

                info.UseShellExecute = false;
                info.CreateNoWindow = true;

                using (Process proceso =
                    Process.Start(info))
                {
                    proceso.WaitForExit();

                    return proceso.ExitCode == 0;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}