using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Project
{
    class Program
    {
        static List<string> data = new List<string>();
        static string path;
        static object locker = new object();

        static void Main(string[] args)
        {
            StartScan();
        }

        static void StartScan()
        {
            Console.WriteLine("Set scan path :");
            path = Console.ReadLine();

            if (!Directory.Exists(path))
            {
                Console.WriteLine("Path not exist =" + path);
                Console.WriteLine();
                StartScan();
            }
            else
            {
                CreateScanThread(path);
            }
        }

        static void CreateScanThread(string scanPath)
        {
            new Thread(Scan).Start(scanPath);
        }

        static void Scan(object pathObj)
        {
            var path = (string)pathObj;
            
            lock (locker)
            {
                data.Add(path);
                Console.WriteLine(path);
            }

            foreach (string dirPath in Directory.GetDirectories(path))
                CreateScanThread(dirPath);
        }
    }
}
