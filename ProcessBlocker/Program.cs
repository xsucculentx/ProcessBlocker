using System;
using System.Diagnostics;

using System.IO;
namespace ProcessBlocker
{
    class Program
    {
        public const double Version = 0.1;
        public const string Branch = "alpha";

        public static string[] ConfigFileData;
        
        static void Main(string[] args)
        {
            Console.WriteLine($"ProcessBlocker v{Version}-{Branch}");
            Console.WriteLine("FOR BEST EXPERIENCE RUN WITH SUDO/ADMIN");
            Console.Write("Processing Config... ");
            try
            {
                if (OperatingSystem.IsWindows())
                {
                    ConfigFileData = File.ReadAllLines($@"C:\Users\{Environment.UserName}\.processblocker");
                }

                if (OperatingSystem.IsLinux())
                {
                    ConfigFileData = File.ReadAllLines($@"/home/{Environment.UserName}/.config/processblocker");
                }
                else
                {
                    Console.Write("Done.\n");
                    Console.WriteLine("Can't detect operating system.");
                    Environment.Exit(1);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Oops! you don't have a config set up. Check the github for a guide!");
            }

            Console.Write("Done.\n");
            
            BlockProcesses();
        }

        static void BlockProcesses()
        {
            Console.WriteLine("Searching for processes...");
            
            foreach (string processName in ConfigFileData)
            {
                while (true)
                {
                    Process[] processes = Process.GetProcessesByName(processName);
                    foreach (Process process in processes)
                    {
                        if (!process.HasExited)
                        {
                            Console.WriteLine($"Killing {process.Handle} ({process.Id})");
                            process.Kill();
                        }
                    }
                }
            }
        }
    }
}