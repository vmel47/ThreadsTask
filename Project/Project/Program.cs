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
            Console.WriteLine();

            if (!Directory.Exists(path))
            {
                Console.WriteLine("Path not exist =" + path);
                Console.WriteLine();
                StartScan();
            }
            else
            {
                var waitHandle = new AutoResetEvent(false);
                CreateScanThread(new ScanData(path, waitHandle));
                waitHandle.WaitOne();
            }
        }

        static void CreateScanThread(ScanData scanData)
        {
            new Thread(Scan).Start(scanData);
        }

        static void Scan(object scanDataObj)
        {
            var scanData = scanDataObj as ScanData;
                    
            lock (locker)
            {
                data.Add(scanData.path);
                Console.WriteLine(scanData.path);
            }

            foreach (string dirPath in Directory.GetDirectories(scanData.path))
            {
                AutoResetEvent waitHandle = new AutoResetEvent(false);
                CreateScanThread(new ScanData(dirPath, waitHandle));
                waitHandle.WaitOne();
            }

            scanData.waitHandle.Set();
        }
    }

    class ScanData
    {
        public string path;
        public AutoResetEvent waitHandle;

        public ScanData(string path, AutoResetEvent waitHandle)
        {
            this.path = path;
            this.waitHandle = waitHandle;
        }
    }
}
