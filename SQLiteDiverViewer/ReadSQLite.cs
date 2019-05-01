using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SQLite;

namespace SQLiteDiverUI
{

    class ReadSQLite
    {
        public static int iErrorHappened = 0;
        public static bool bConvertEpochDates = false;

        public static List<String> ListOfDateIndicators = new List<String>();

        public static DataTable _dtTableNames = new DataTable();

        //public void dtStuff(string _dbPath, string _dbSourceFile, string _outpath)
        public void dtStuff(string _dbPath, string _dbSourceFile)
        {
            ListOfDateIndicators.Clear();
            Fill_ListOfDateIndicators();

            //var _dtTableNames = new DataTable();
            //var names = new List<String>();

            String connString = "Data Source=" + @_dbPath;

            //using (SQLiteConnection conn = new SQLiteConnection(connString))
           // {
                string query;
                query = "SELECT name,sql FROM sqlite_master WHERE type = \"table\"";


               // using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
               // {
                using (var _slda = new SQLiteDataAdapter(query, new SQLiteConnection(connString)))
                {
                        
                        //_slda.FillSchema(dtNames, SchemaType.Source);
                    _slda.Fill(_dtTableNames);

                

                        //conn.Open();
                       // using (SQLiteDataReader dr = _slda.ExecuteReader())
                      //  {
                     //       dtNames.Load(dr);
                     //   }
                   }
               // }

                foreach (DataRow r in _dtTableNames.Rows)
                { 
                        var dt = new DataTable();
                        string tablename = r[0].ToString();

                        query = "SELECT * FROM " + tablename;

                    

                      //  using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                       // {
                            //conn.Open();
                         //   using (SQLiteDataReader dr = cmd.ExecuteReader())
                        //    {
                    using (var _slda = new SQLiteDataAdapter(query, new SQLiteConnection(connString)))
                    {
                                try
                                {
                                 //   DataSet ds = new DataSet();
                                    //var _slda = new SQLiteDataAdapter();
                                    
                                    _slda.FillSchema(dt, SchemaType.Source);

                                    foreach (DataColumn Col in dt.Columns)
                                    {
                                        foreach (string t in ListOfDateIndicators)
                                        {
                                            if (Col.ColumnName.ToLower().Contains(t.ToLower()))
                                            {
                                                Col.DataType = typeof(string);
                                            }
                                        }
                                    }

                                    //Some sqlite tables like Chrome Cookies use date fields as unique constraints. (eg: creation_utc)
                                    dt.Constraints.Clear();

                                    _slda.Fill(dt);

                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message.ToString());
                                    iErrorHappened++;
                                }

                            
                        }

                    
                    //    string outfile = Path.GetDirectoryName(_outpath) + "\\" + _dbSourceFile + "_" + tablename + ".txt";

                        if (dt.Rows.Count > 0)
                        {
                            //Console.WriteLine("table: \"" + tablename + "\"");
                      //      Console.WriteLine(" to file: " + outfile);

                            if(bConvertEpochDates) 
                            {
                                Console.WriteLine("table: \"" + tablename + "\": decoding known datetime values..");

                                int iColumnCount = dt.Columns.Count;
                                foreach (DataRow row in dt.Rows)
                                {
                                    foreach (DataColumn Col in dt.Columns)
                                    {
                                        foreach (string t in ListOfDateIndicators)
                                        {
                                            //if(ListOfDateIndicators.Contains(Col.ColumnName.ToLower())) {

                                            //ListOfDateIndicators().Any(s => myString.Contains(s))
                                            if (Col.ColumnName.ToLower().Contains(t.ToLower())  )         
                                            {
                                                if(EpochHander.IsNumber(row[Col.ColumnName].ToString()))  {
 
                                                    //var _date = EpochHander.FromUnixTime1(row[Col.ColumnName].ToString());
                                                    //row[Col] = _date;

                                                    row[Col] = EpochHander.FromUnixTime1(row[Col.ColumnName].ToString(), _dbPath);
                                                }
                                            }
                                        }
                                    }
                                }
                             }

                             dt.AcceptChanges();

                             Main.ds.Tables.Add(dt);

                            //export.ExportToFile(tablename, dt, outfile);
                        }
                        else
                        {
                            Console.WriteLine("table: \"" + tablename + "\" is empty.");
                        }

                    }
            

            }

        


        private void Fill_ListOfDateIndicators()
        {
            string sDateIndicatorsPath = Main.appExecutionPath + " \\" + "date_indicators.txt";
            using (StringReader reader = new StringReader(File.ReadAllText(@sDateIndicatorsPath)))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if(!String.IsNullOrEmpty(line) | !String.IsNullOrWhiteSpace(line) )
                        ListOfDateIndicators.Add(line);
                }
            }

        }




    }
}
