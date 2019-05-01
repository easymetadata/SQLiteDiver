using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using CommandLine;
using CommandLine.Text; // if you want text formatting helpers (recommended)

namespace SQLiteDiver
{
    class Options
    {
        public enum OutputFormat
        {
            TXT
            //TXT,
           // JSON,
           // XML
        }

        //[Option('m', "mobile", Required = true, HelpText = "Try to identify and decode ePoch timestamps")]
        //public string OutputFile { get; set; }
        
        [Option('d', "datedecode", Required = false, HelpText = "Decode Dates.")]
        public bool DateDecode { get; set; }

        [Option('i', "input", Required = true, HelpText = "Input file to read.")]
        public string InputFile { get; set; }

       // [Option('r', "recurse", Required = false, HelpText = "Recurse input subdirectories for -i. Important: The path rather than file will be used!")]
       // public string Recurse { get; set; }

      //  [Option('g', "Output format", Required = false, HelpText = "Allows you to choose output format.")]
      //  public OutputFormat SpecifiedOutputFormat { get; set; }
        
        [Option('o', "output", Required = true, HelpText = "Output Path.")]
        public string OutputFile { get; set; }

        // [Option('v', null, HelpText = "Print details during execution.")]
        // public bool Verbose { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            // this without using CommandLine.Text
            //  or using HelpText.AutoBuild
            var help = new HelpText
            {
                Heading = new HeadingInfo("SQLiteDiver", Main.GetAppVer()),
                Copyright = new CopyrightInfo("David Dym", 2014),
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true
            };
           // help.AddPreOptionsLine("<<license details here.>>");
            help.AddPreOptionsLine("Usage: SQLiteDiver -i c:\\... -o c:\\..");
            help.AddOptions(this);
            return help;
        }
        

    }
}
