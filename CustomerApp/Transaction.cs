using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
namespace CustomerApp
{
    class Transaction
    {

        

        public void Write(TransactionRecord rec)
        {
            string tempfilepath = @"D:\Psiog programs\CustomerApp\TempInventory.csv";
            string filepath = @"D:\Psiog programs\CustomerApp\Inventory.csv";

            string ctempfilepath = @"D:\Psiog programs\CustomerApp\TempCustomer.csv";
            string cfilepath = @"D:\Psiog programs\CustomerApp\Customer.csv";

            bool append = true;

            using (var sw = new StreamWriter(@"D:\Psiog programs\CustomerApp\Sales.csv", append))
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

                csvwriter.WriteHeader<InventoryRecord>();
                csvwriter.NextRecord();
                IEnumerable<InventoryRecord> records = csvreader.GetRecords<InventoryRecord>();
                foreach (var record in records)
                {
                    if (record.ItemID == rec.ItemID)
                    {
                        record.Stock = record.Stock - rec.QOS;
                    }
                    csvwriter.WriteRecord(record);
                    csvwriter.NextRecord();
                }


            }

            File.Delete(filepath);
            File.Move(tempfilepath, filepath);
            File.Delete(tempfilepath);


            using (var sr = new StreamReader(cfilepath))
            using (var csvreader = new CsvReader(sr, System.Globalization.CultureInfo.CurrentCulture))
            using (var sw = new StreamWriter(ctempfilepath))
            using (var csvwriter = new CsvWriter(sw, System.Globalization.CultureInfo.CurrentCulture))
            {

                csvwriter.WriteHeader<CustomerRecord>();
                csvwriter.NextRecord();
                IEnumerable<CustomerRecord> records = csvreader.GetRecords<CustomerRecord>();
                foreach (var record in records)
                {
                    if (record.CustomerID == rec.CustomerID)
                    {
                        if(rec.CreditPeriod!=0)
                            record.Balance = record.Balance + rec.TotalSales;
                    }
                    csvwriter.WriteRecord(record);
                    csvwriter.NextRecord();
                }


            }

            File.Delete(cfilepath);
            File.Move(ctempfilepath, cfilepath);
            File.Delete(ctempfilepath);



        }
    }
}

