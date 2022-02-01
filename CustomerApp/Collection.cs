using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
namespace CustomerApp
{
    class Collection
    {
        public void WriteCollection(CollectionRecord rec)
        {
            string tempfilepath = @"D:\Psiog programs\CustomerApp\tempCustomer.csv";
            string filepath = @"D:\Psiog programs\CustomerApp\Customer.csv";

            bool append = true;

            using (var sw = new StreamWriter(@"D:\Psiog programs\CustomerApp\Collection.csv", append))
            using (var csvwriter = new CsvWriter(sw, System.Globalization.CultureInfo.CurrentCulture))
            {
                csvwriter.NextRecord();
                csvwriter.WriteRecord(rec);
            }

            using (var sr = new StreamReader(filepath))
            using (var csvreader = new CsvReader(sr, System.Globalization.CultureInfo.CurrentCulture))
            using (var sw = new StreamWriter(tempfilepath))
            using (var csvwriter = new CsvWriter(sw, System.Globalization.CultureInfo.CurrentCulture))
            {

                csvwriter.WriteHeader<CustomerRecord>();
                csvwriter.NextRecord();
                
                IEnumerable<CustomerRecord> records = csvreader.GetRecords<CustomerRecord>();
                foreach (var record in records)
                {
                    if (record.CustomerID == rec.CustomerID)
                    {
                        if(record.CreditPeriod!=0)
                            record.Balance = record.Balance - rec.MoneyReceived;
                    }
                    csvwriter.WriteRecord(record);
                    csvwriter.NextRecord();
                }


            }

            File.Delete(filepath);
            File.Move(tempfilepath, filepath);
            File.Delete(tempfilepath);
        }
    }



}

