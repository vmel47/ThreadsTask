using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Project
{
    class Program
    {
        static List<string> data = new List<string>();
        static string path;

        static void Main(string[] args)
        {
            StartScan();
        }

        static void StartScan()
        {
            Console.WriteLine("Set scan path :");
            path = Console.ReadLine();
            Console.WriteLine();

            if (!Directory.Exists(path))
            {
                Console.WriteLine("Path not exist =" + path);
                Console.WriteLine();
                StartScan();
            }
            else
            {
                CreateScanTask(path);
            }
        }

        static void CreateScanTask(string scanPath)
        {
            var task = Task.Factory.StartNew(() =>
            {
                data.Add(scanPath);
                Console.WriteLine(scanPath);

                foreach (string dirPath in Directory.GetDirectories(scanPath))
                    CreateScanTask(dirPath);
            });
            task.Wait();
        }
    }
}
