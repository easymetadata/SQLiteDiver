using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SQLiteDiver
{
    class Program
    {
        // public static Options options = new Options();


        static void Main(string[] args)
        {

            var options = new Options();

            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                // consume Options instance properties
               // if (options.Verbose)
               // {
                   // Console.WriteLine(options.InputFile);
                   // Console.WriteLine(options.OutputFile);
                   // Console.WriteLine(options.GetUsage());
              //  }
             //   else
                //    Console.WriteLine("working ...");
            }

            var main = new Main();

            if (options.DateDecode)
            {
                ReadSQLite.bConvertEpochDates = options.DateDecode;
            }

            if(!String.IsNullOrEmpty(options.InputFile) && !String.IsNullOrEmpty(options.OutputFile))
                main.DoStuff(options.InputFile, options.OutputFile);


        }

    }

}
