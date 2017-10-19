using System;
using System.IO;

namespace FileNameMap
{
    public class MainProgram
    {
        private static string _mainDirectory;
        private static string _outputFile;
        private static string _logFile;

        public static void Main(string[] args)
        {
            string searchPath;

            // Initial (default) settings
            _mainDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            _outputFile = _mainDirectory + "\\files_mapping.txt";
            _logFile = _mainDirectory + "\\fnm_log.txt";

            // Point out the location of the output files
            Console.WriteLine("Archivo de salida en '" + _outputFile + "'");
            Console.WriteLine("Logs en '" + _logFile + "'");

            // Read 'search directory' from Console
            Console.Write(Environment.NewLine + "Ingrese el directorio raiz en donde buscar: ");
            searchPath = Console.ReadLine();

            // Directory validation
            if (!Directory.Exists(searchPath))
                Console.WriteLine(Environment.NewLine + "  ERROR. El directorio especificado no existe.");
            else
            {
                Console.WriteLine(Environment.NewLine + "Leyendo los nombres de los archivos...");

                // Search files and directories
                if (!DiveIntoDir(searchPath))
                    Console.WriteLine(Environment.NewLine + "  ERROR. Favor de referirse al log para mayor información.");
                else
                    Console.WriteLine(Environment.NewLine + "Todos los archivos en '" + searchPath + "' han sido mapeados.");
            }
        }

        // Peek into directories in search of files and more directories
        private static bool DiveIntoDir(string path)
        {
            try
            {
                string[] directories = Directory.GetDirectories(path);
                string[] files = Directory.GetFiles(path);

                // Dive into each directory under current one
                foreach (string directory in directories)
                    DiveIntoDir(directory);

                // Write results to output file (as they are found)
                return WriteFilesToOutput(files);
            }
            catch (Exception ex)
            {
                LogEx(ex.Message, ex.ToString());
                return false;
            }
        }

        // Stream writer to output file
        private static bool WriteFilesToOutput(string[] fileList)
        {
            StreamWriter sWriter;

            try
            {
                sWriter = new StreamWriter(_outputFile, true);

                foreach (string file in fileList)
                    sWriter.WriteLine(file);

                sWriter.Close();
                return true;
            }
            catch (Exception ex)
            {
                LogEx(ex.Message, ex.ToString());
                return false;
            }
        }

        // Error logging
        private static void LogEx(string message, string ex)
        {
            StreamWriter oFile = new StreamWriter(_logFile, true);

            oFile.WriteLine(ex);
            oFile.Close();
        }
    }
}
