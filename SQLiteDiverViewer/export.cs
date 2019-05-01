using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SQLiteDiverUI
{
    public static class export
    {

        public static bool ExportToFile(string _tablename, DataTable _dt, string outFile)
        {
            try
            {
                string out1 = export.ToCSV(_dt, "\t", true);

                System.IO.File.WriteAllText(@outFile, out1, Encoding.Default);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public static string ToCSV(this DataTable table, string delimiter, bool includeHeader)
        {
            var result = new StringBuilder();

            if (includeHeader)
            {
                foreach (DataColumn column in table.Columns)
                {
                    result.Append(column.ColumnName);
                    result.Append(delimiter);
                }

                result.Remove(--result.Length, 0);
                result.Append(Environment.NewLine);
            }

            foreach (DataRow row in table.Rows)
            {
                foreach (object item in row.ItemArray)
                {
                    if (item is DBNull)
                        result.Append(delimiter);
                    else
                    {
                        string itemAsString = item.ToString();
                        // Double up all embedded double quotes
                        itemAsString = itemAsString.Replace("\"", "\"\"");

                        // To keep things simple, always delimit with double-quotes
                        // so we don't have to determine in which cases they're necessary
                        // and which cases they're not.
                        itemAsString = "\"" + itemAsString + "\"";

                        result.Append(itemAsString + delimiter);
                    }
                }

                result.Remove(--result.Length, 0);
                result.Append(Environment.NewLine);
            }

            return result.ToString();
        }

    }
}
