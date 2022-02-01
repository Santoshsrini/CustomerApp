using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
namespace CustomerApp
{
    class Username
    {
        public string username { get; set; }
        public string password { get; set; }


        string filepath = @"D:\Psiog programs\CustomerApp\Username.csv";
        void UsernameExists()
        {
            if (!File.Exists(filepath))
                Console.WriteLine("File does not exist ");

        }

        public bool UsernameAuthentication(string uname, string pwd)
        {

            int flag = 0;
            using (var sr = new StreamReader(filepath))
            using (var csvreader = new CsvReader(sr, System.Globalization.CultureInfo.CurrentCulture))
            {
                IEnumerable<Username> records = csvreader.GetRecords<Username>();
                foreach (var record in records)
                {

                    if (record.username == uname)
                    {
                        if (record.password == pwd)
                        {
                            flag = 1;

                        }
                    }
                }
            }
            if (flag == 0)
                return false;
            else
                return true;
        }

    }

}

