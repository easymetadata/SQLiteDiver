using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Data;

namespace SQLiteDiverUI
{
    class Main
    {
        private static string _appExecutionPath;
        public static string appExecutionPath {
            get
            {
                _appExecutionPath = System.IO.Path.GetDirectoryName(
                    System.Reflection.Assembly.GetExecutingAssembly().Location);
                    //System.Reflection.Assembly.GetExecutingAssembly().Location.GetName().CodeBase);
//                return _appExecutionPath.Replace("file:\\","");
                return _appExecutionPath;
            }
            set { _appExecutionPath = value; }
        }

        private string _status;
        public string FileStatus  { get { return _status; } set { _status = value;}    }

        public static DataSet ds = new DataSet();

        public static string SourceFilename;

        public bool DoStuff(string _inFile)
        {
            SourceFilename = Path.GetFileName(_inFile);

            var readsqlite = new ReadSQLite();

            try
            {
                //check for file and that it is a sqlite3 db
                if (!IsSQLite3(_inFile))
                    return false;

                    Console.WriteLine(SourceFilename + " db checks out...");

                    var _readsqlite = new ReadSQLite();

                    //_readsqlite.dtStuff(_inFile, SourceFilename, _outFile);
                    _readsqlite.dtStuff(_inFile, SourceFilename);

                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return false;
 
            }

            return true;
        }

        private bool IsSQLite3(string _infile)
        {
            try
            {
                byte[] bytelist = new byte[16];

                using (BinaryReader reader = new BinaryReader(new FileStream(_infile, FileMode.Open, FileAccess.Read)))
                {
                    reader.BaseStream.Seek(0, SeekOrigin.Begin);
                    reader.Read(bytelist, 0, 16);
                }

                string sVersion = Encoding.UTF8.GetString(bytelist, 0, bytelist.Length);

                if (sVersion == "SQLite format 3\0")
                    return true;

                _status = "Not a SQLite 3/0 db. ";
                Console.WriteLine(_status + _infile);
                return false;
            }
            catch (Exception)
            {
                _status = "Failed to open db file.";
                return false;
            }

        }

                //private bool IsFileStatusGood(string _infile)
        //{
        //    if (!File.Exists(@_infile))
        //    {
        //        Console.WriteLine("File does not exist. " + _infile);
        //        return false;
        //    }

        //    return true;
        //}


        public static String GetFileProps(string _infile)
        {
            var sb = new StringBuilder();

            if(File.Exists(@_infile)) {
                FileInfo fileinfo = new FileInfo(@_infile);

                if (fileinfo.Exists)
                {
                    sb.AppendLine("Created      " + fileinfo.CreationTime.ToShortDateString());
                    sb.AppendLine("Last Write   " + fileinfo.LastWriteTime.ToShortDateString());
                    sb.AppendLine("Last Access  " + fileinfo.LastAccessTime.ToShortDateString());
                    sb.AppendLine("Attributes   " + fileinfo.Attributes.ToString());
                }
            }

            return sb.ToString();
            
        }
        
    
    }
}
