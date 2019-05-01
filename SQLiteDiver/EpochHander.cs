using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace SQLiteDiver
{
    class EpochHander
    {
        public static string FromUnixTime1(string unixTime, string _dbSourceFile)
        {
            if (string.IsNullOrEmpty(unixTime) | !IsNumber(unixTime) | unixTime == "0")
                return unixTime;

            //if it's too short it's not a epoch time...
            if (unixTime.Length < 6)
                return unixTime;

            try
            {
                //Stat the file to get it's last modified date.
                var modDate = new DateTime();
                if (File.Exists(_dbSourceFile))
                {
                    FileInfo fileinfo = new FileInfo(_dbSourceFile);

                    if (fileinfo.Exists)
                    {
                        modDate = fileinfo.LastWriteTime;
                    }
                    else
                    {
                        modDate = DateTime.Now;
                        Console.WriteLine("DateTime Conversion: 'modDate' could not be determined using FileInfo. Using today's date instead...");
                    }
                }

                //#Convert the time string to double. 
                //Allows for handling variable length of numeric values including some with decimals.
                var dunixTime = Convert.ToDouble(unixTime);

                // First make a System.DateTime equivalent to the UNIX Epoch.
                System.DateTime dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                // Add the number of seconds in UNIX timestamp to be converted.

                //dateTime = dateTime.AddSeconds(dunixTime / 1000000);

                //create list of possible options...?


                try
                {
                    dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

                    dateTime = dateTime.AddSeconds(dunixTime / 1000000);
                    if ((dateTime.Year >= modDate.Year - 3 & dateTime.Year <= DateTime.Now.Year) | dateTime.Year == modDate.Year)
                        return dateTime.ToString();
                }
                catch { }
                //  catch (System.ArgumentOutOfRangeException ex) { //Console.WriteLine("No Joy, Case #3 " + ex.Message.ToString());
                //}

                //Case #1
                double convertedTime;
                try
                {

                    convertedTime = (dunixTime - 11644473600000000) / 1000000;//divide by 1000000 because we are going to add Seconds on to the base date
                    DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    dateTime = date.AddSeconds(convertedTime);
                    if ((dateTime.Year >= modDate.Year - 3 & dateTime.Year <= DateTime.Now.Year) | dateTime.Year == modDate.Year)
                    {
                        return dateTime.ToString();
                    }
                }
                catch
                {
                    //Some rare cases still cause errors. Cookies db is examp
                }

                //Case #1
                try
                {
                    //DateTime.MaxValue.Ticks
                    //if (dunixTime > Int64.MaxValue)
                    //{
                    dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    dateTime = dateTime.AddSeconds(dunixTime);
                    if ((dateTime.Year >= modDate.Year - 3 & dateTime.Year <= DateTime.Now.Year) | dateTime.Year == modDate.Year)
                        return dateTime.ToString();
                    //}
                }
                catch { }
                //catch (System.ArgumentOutOfRangeException ex)   {   //Console.WriteLine("No Joy, Case #2 " + ex.Message.ToString());   
                // }

                try
                {
                    dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    dateTime = dateTime.AddSeconds(dunixTime / 10000000);
                    if ((dateTime.Year >= modDate.Year - 3 & dateTime.Year <= DateTime.Now.Year) | dateTime.Year == modDate.Year)
                        return dateTime.ToString();
                }
                catch { }

                try
                {
                    dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    dateTime = dateTime.AddSeconds(dunixTime / 10000);
                    if ((dateTime.Year >= modDate.Year - 3 & dateTime.Year <= DateTime.Now.Year) | dateTime.Year == modDate.Year)
                        return dateTime.ToString();
                }
                catch { }


                try
                {
                    dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    dateTime = dateTime.AddSeconds(dunixTime / 1000);
                    if ((dateTime.Year >= modDate.Year - 3 & dateTime.Year <= DateTime.Now.Year) | dateTime.Year == modDate.Year)
                        return dateTime.ToString();
                }
                catch { }

                //Mac ePoch
                try
                {
                    // if (new System.DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(dunixTime).Year == 1982)
                    // {
                    dateTime = new System.DateTime(2001, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    dateTime = dateTime.AddSeconds(dunixTime);
                    if ((dateTime.Year >= modDate.Year - 3 & dateTime.Year <= DateTime.Now.Year) | dateTime.Year == modDate.Year)
                        return dateTime.ToString();
                    //  }
                }
                catch { }

                return dateTime.ToString();
            }
            catch (System.ArgumentOutOfRangeException)
            {
                return unixTime;
            }


        }

        public static bool IsNumber(string s)
        {
            //Console.WriteLine(s);

            Regex regEx = new Regex("[.0-9]");

            if (regEx.IsMatch(s))
            {
                s = s.Replace(".", "");
                //s = ((int)Convert.ToDouble(s)).ToString();
                //Console.WriteLine(s);
            }

            return s.All(char.IsDigit);
        }

    }
}
