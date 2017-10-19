using System;
using System.IO;

namespace FileNameMap
{
    public class MainProgram
    {
        private static string mainDirectory;
        private static string outputFile;
        private static string logFile;

        public static void Main(string[] args)
        {
            string searchPath;

            // Initial settings
            mainDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            outputFile = mainDirectory + "\\filesMapping.txt";
            logFile = mainDirectory + "\\log.txt";

            // Read 'search directory' from Console
            Console.WriteLine("Archivo de salida en '" + outputFile + "'");
            Console.WriteLine("Logs en '" + logFile + "'");

            Console.Write(Environment.NewLine + "Ingrese el directorio raiz en donde buscar: ");
            searchPath = Console.ReadLine();

            // Directory validation
            if (!Directory.Exists(searchPath))
                Console.WriteLine(Environment.NewLine + "  ERROR. El directorio especificado no existe.");
            else
            {
                Console.WriteLine(Environment.NewLine + "Leyendo los nombres de los archivos...");

                if (!IntoFolders(searchPath))
                    Console.WriteLine(Environment.NewLine + "  ERROR. Favor de referirse al log para mayor información.");
                else
                    Console.WriteLine(Environment.NewLine + "Todos los archivos en '" + searchPath + "' han sido mapeados.");
            }
        }

        private static bool IntoFolders(string path)
        {
            try
            {
                string[] folders = Directory.GetDirectories(path);
                string[] files = Directory.GetFiles(path);

                foreach (string element in folders)
                    IntoFolders(element);

                return WriteFiles(files);
            }
            catch (Exception ex)
            {
                LogEx(ex.Message, ex.ToString());
                return false;
            }
        }

        private static bool WriteFiles(string[] listF)
        {
            try
            {
                StreamWriter sWriter = new StreamWriter(outputFile, true);

                foreach (string element in listF)
                    sWriter.WriteLine(element);

                //sWriter.Flush();
                sWriter.Close();
            }
            catch (Exception ex)
            {
                LogEx(ex.Message, ex.ToString());
            }

            return true;
        }

        private static void LogEx(string message, string ex)
        {
            StreamWriter oFile = new StreamWriter(logFile, true);

            oFile.WriteLine(ex);
            //oFile.Flush();
            oFile.Close();
        }
    }
}
