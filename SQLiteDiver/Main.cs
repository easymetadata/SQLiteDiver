using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Reflection;

namespace SQLiteDiver
{
    class Main
    {
        private string _status;
        public string FileStatus { get { return _status; } set { _status = value; } }

        public static DataSet ds = new DataSet();

        public static string SourceFilename;

        public static string GetAppVer()
        {
            string _ver = Assembly.GetExecutingAssembly().GetName().Version.Major.ToString() + "." +
                    Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString() + "." +
                    Assembly.GetExecutingAssembly().GetName().Version.Build.ToString();
            return _ver;
        }

        public bool DoStuff(string _inFile, string _outFile)
        {
            //resets error message form previous errors. Only there for consistency with gui version.
            ReadSQLite.iErrorHappened = 0;

          // if (_datedecode)
           //     ReadSQLite.bConvertEpochDates = true;
            
            SourceFilename = Path.GetFileName(_inFile);

            var readsqlite = new ReadSQLite();

            try
            {
                //check for file and that it is a sqlite3 db
                if (!IsSQLite3(_inFile))
                    return false;

                Console.WriteLine(SourceFilename + " db checks out...");

                var _readsqlite = new ReadSQLite();

                _readsqlite.dtStuff(_inFile, SourceFilename, _outFile);


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


        //public void DoStuff_OLD(string _inFile, string _outFile)
        //{
        //    SourceFilename = Path.GetFileName(_inFile);

        //    var readsqlite = new ReadSQLite();

        //    //check for file and that it is a sqlite3 db
        //    if (IsFileStatusGood(_inFile) && IsSQLite3(_inFile))
        //    {
        //        Console.WriteLine(SourceFilename + " db checks out...");

        //        var _readsqlite = new ReadSQLite();

        //        _readsqlite.dtStuff(_inFile, SourceFilename, _outFile);


        //    }

        //}


        //private bool IsFileStatusGood(string _infile)
        //{
        //    if (!File.Exists(@_infile))
        //    {
        //        Console.WriteLine("File does not exist. " + _infile);
        //        return false;
        //    }

        //    return true;
        //}

        //private bool IsSQLite3(string _infile)
        //{

        //    byte[] bytelist = new byte[16];
        //    using (BinaryReader reader = new BinaryReader(new FileStream(_infile, FileMode.Open)))
        //    {
        //        reader.BaseStream.Seek(0, SeekOrigin.Begin);
        //        reader.Read(bytelist, 0, 16);
        //    }

        //    string sVersion = Encoding.UTF8.GetString(bytelist, 0, bytelist.Length);

        //    if (sVersion == "SQLite format 3\0")
        //        return true;

        //    Console.WriteLine("Not a SQLite 3/0 db. " + _infile);
        //    return false;
        //}


    
    }
}
