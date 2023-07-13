using System;
using System.Collections.Generic;
using System.IO;

namespace ImportTablePatcher
{
    class Program
    {
        static void Main(string[] args)
        {
            string ascii = @"
  _____                            _ _______    _     _      _____      _       _               
 |_   _|                          | |__   __|  | |   | |    |  __ \    | |     | |              
   | |  _ __ ___  _ __   ___  _ __| |_ | | __ _| |__ | | ___| |__) |_ _| |_ ___| |__   ___ _ __ 
   | | | '_ ` _ \| '_ \ / _ \| '__| __|| |/ _` | '_ \| |/ _ \  ___/ _` | __/ __| '_ \ / _ \ '__|
  _| |_| | | | | | |_) | (_) | |  | |_ | | (_| | |_) | |  __/ |  | (_| | || (__| | | |  __/ |   
 |_____|_| |_| |_| .__/ \___/|_|   \__||_|\__,_|_.__/|_|\___|_|   \__,_|\__\___|_| |_|\___|_|   
                 | |                                                                            
                 |_|                                                                            
";

            Console.WriteLine(ascii);
            Console.WriteLine("Simple patcher for import table, made by @emberantz\n");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("? ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("Enter EXE file path: ");

            string exePath = Console.ReadLine();

            if(!File.Exists(exePath))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("X EXE file not found. Did you write the right path?");
                Console.ResetColor();
                Console.ReadKey();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("? ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("Enter DLL file path: ");

            string dllPath = Console.ReadLine();

            if (!File.Exists(dllPath))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("X DLL file not found. Did you write the right path?");
                Console.ResetColor();
                Console.ReadKey();
                return;
            }

            var exePeFile = new PeNet.PeFile(exePath);
            var dllPeFile = new PeNet.PeFile(dllPath);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("✓ ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Loaded EXE file");

            var symbolsName = new List<string>();

            foreach (PeNet.Header.Pe.ExportFunction exportFunction in dllPeFile.ExportedFunctions)
            {
                symbolsName.Add(exportFunction.Name);
            }

            string symbolName = Sharprompt.Prompt.Select("Which function would you like to import?", symbolsName.ToArray(), pageSize: 3);
            

            exePeFile.AddImport(dllPath, symbolName);
            File.WriteAllBytes(exePath, exePeFile.RawFile.ToArray());

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("✓ ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Patched EXE to : " + exePath + ".");

            Console.ReadKey();
        }
    }
}
