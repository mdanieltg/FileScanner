using System;
using System.IO;

namespace FileScanner.ConsoleApplication
{
    internal class Program
    {
        private static string _outputFile;
        private static string _logFile;

        private static ulong _fileSize;
        private static int _fileCount;

        public static void Main(string[] args)
        {
            // Initial (default) settings
            var basePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            _outputFile = basePath + "\\files_mapping.txt";
            _logFile = basePath + "\\fnm_log.txt";

            // Point out the location of the output files
            Console.WriteLine("Archivo de salida en '{0}'", _outputFile);
            Console.WriteLine("Logs en '{0}'", _logFile);
            Console.WriteLine();

            // Read 'search directory' from Console
            Console.Write("Ingrese el directorio raíz en donde buscar: ");
            var searchPath = Console.ReadLine();

            // Directory validation
            if (!Directory.Exists(searchPath))
                Console.WriteLine("  ERROR. El directorio especificado no existe.");
            else
            {
                _fileSize = 0;
                _fileCount = 0;
                Console.WriteLine("Leyendo los nombres de los archivos...");

                // Search files and directories
                if (!Scan(searchPath))
                    Console.WriteLine("  ERROR. Favor de referirse al log para mayor información.");
                else
                {
                    // Results presentation
                    Console.WriteLine("Todos los archivos en '{0}' han sido mapeados.", searchPath);
                }
            }
        }

        private static bool Scan(string path)
        {
            try
            {
                GetContents(path);
                return true;
            }
            catch (Exception e)
            {
                LogEx(e.ToString());
                return false;
            }
        }

        // Get into directories in search of files and more directories
        private static void GetContents(string path)
        {
            string[] directories = Directory.GetDirectories(path),
                files = Directory.GetFiles(path);

            // Dive into each directory under current one
            foreach (var directory in directories)
                GetContents(directory);

            // Write results to output file (as they are found)
            WriteFilesToOutput(files);
        }

        // Stream writer to output file
        private static void WriteFilesToOutput(string[] files)
        {
            if (files.Length == 0) return;

            _fileCount = (int) Math.Floor(_fileSize / 80000d);
            _fileSize += (ulong) files.Length;

            var fileSuffix = $"_{_fileCount.ToString("000")}.txt";
            var filePath = _outputFile.Replace(".txt", fileSuffix);

            using (var streamWriter = new StreamWriter(filePath, true))
            {
                foreach (var file in files)
                    streamWriter.WriteLine(file);
            }
        }

        // Error logging
        private static void LogEx(string ex)
        {
            using (var streamWriter = new StreamWriter(_logFile, true))
            {
                streamWriter.WriteLine(ex);
            }
        }
    }
}