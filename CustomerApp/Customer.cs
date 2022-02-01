using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;

namespace CustomerApp
{
    class Customer
    {
        string filepath;

        public Customer(string filepath)
        {
            this.filepath = filepath;
        }

        public void AddCustomer(CustomerRecord rec)
        {
            bool append = true;

            using (var sw = new StreamWriter(filepath, append))
            using (var csvwriter = new CsvWriter(sw, System.Globalization.CultureInfo.CurrentCulture))
            {

                csvwriter.NextRecord();
                csvwriter.WriteRecord(rec);
                //csvwriter.NextRecord();

            }
        }

        public void UpdateCustomer()
        {
            
            string tempfilepath = @"D:\Psiog programs\InventoryApp\TempInventory.csv";
            int flag = 0;
            List<int> choices = new List<int>();
            string option = "y";


            Console.WriteLine("Enter the Customer ID you want to update");
            string id = Console.ReadLine();
            
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
                    if (record.CustomerID == id)
                    {
                        flag = 1;                                                
                        Console.WriteLine("1. Customer Name " + "\n" + "2. Customer Address" + "\n" + "3. Credit Period" + "\n" + "4. State");

                        do
                        {
                            Console.WriteLine("What do you want to update");
                            int ch = int.Parse(Console.ReadLine());
                            choices.Add(ch);
                            Console.WriteLine("Do you want to update anything else (y/n)");
                            option = Console.ReadLine();
                            
                        } while (option=="y");



                        for (int i = 0; i < choices.Count; i++)
                        {
                            switch (choices[i])
                            {
                                case 1:
                                    Console.WriteLine("Current Customer Name: " + record.CustomerName);
                                    Console.WriteLine("Enter the Customer Name");
                                    record.CustomerName = Console.ReadLine();
                                    break;

                                case 2:
                                    Console.WriteLine("Current Customer Address: " + record.CustomerAddress);
                                    Console.WriteLine("Enter the Customer Address");
                                    record.CustomerAddress = Console.ReadLine();
                                    break;

                                case 3:
                                    Console.WriteLine("Current Credit Period: " + record.CreditPeriod);
                                    Console.WriteLine("Enter the Credit Period");
                                    record.CreditPeriod = int.Parse(Console.ReadLine());
                                    break;

                                case 4:
                                    Console.WriteLine("Current Customer State: " + record.State);
                                    Console.WriteLine("Enter the Customer State");
                                    record.State = Console.ReadLine();
                                    break;

                                default:
                                    Console.WriteLine("Choice can be only from 1 to 4");
                                    break;
                            } 
                        }
                    }
                    csvwriter.WriteRecord(record);
                    csvwriter.NextRecord();
                }

                if (flag == 0)
                    Console.WriteLine("Customer ID doesn't exist to be updated");
            }

            File.Delete(filepath);
            File.Move(tempfilepath, filepath);
            File.Delete(tempfilepath);

        }

        public void DeleteCustomer()
        {

            string tempfilepath = @"D:\Psiog programs\CustomerApp\TempCustomer.csv";
            int flag = 0;
            

            Console.WriteLine("Enter the Customer ID you want to delete");
            string id = Console.ReadLine();

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
                    if (record.CustomerID != id)
                    {
                        csvwriter.WriteRecord(record);
                        csvwriter.NextRecord();
                    }
                    else
                        flag = 1;
                }
            }

            if (flag == 0)
                Console.WriteLine("Customer ID doesn't exist to be deleted");
            else
                Console.WriteLine("Record deleted successfully");

            File.Delete(filepath);
            File.Move(tempfilepath, filepath);
            File.Delete(tempfilepath);

        }

        public void SuspendCustomer()
        {

            string tempfilepath = @"D:\Psiog programs\CustomerApp\tempCustomer.csv";


            
            using (var sw = new StreamWriter(tempfilepath))
            using (var csvwriter = new CsvWriter(sw, System.Globalization.CultureInfo.CurrentCulture))
            using (var sr = new StreamReader(filepath))
            using (var csvreader = new CsvReader(sr, System.Globalization.CultureInfo.CurrentCulture))
            {

                csvwriter.WriteHeader<CustomerRecord>();
                csvwriter.NextRecord();

                IEnumerable<CustomerRecord> records = csvreader.GetRecords<CustomerRecord>();
                foreach (var record in records)
                {
                    int TotalLoan = 0;
                    int MustPay = 0;
                    
                    if (record.Balance <= 0 && record.State == "Suspended")
                        record.State = "Active";

                    using (var salesr = new StreamReader(@"D:\Psiog programs\CustomerApp\Sales.csv"))
                    using (var salesreader = new CsvReader(salesr, System.Globalization.CultureInfo.CurrentCulture))
                    {
                        
                        IEnumerable<TransactionRecord> salesrecords = salesreader.GetRecords<TransactionRecord>();
                        foreach (TransactionRecord salesrecord in salesrecords)
                        {
                            
                            if (salesrecord.CustomerID==record.CustomerID)
                            {
                                
                               // Console.WriteLine(salesrecord.TotalSales);
                                TotalLoan += salesrecord.TotalSales;

                                DateTime mydate = DateTime.Parse(salesrecord.Date);
                                var now = DateTime.Now;
                                var timespan = now.Subtract(mydate);
                                if (timespan.Days >= salesrecord.CreditPeriod)
                                    MustPay += salesrecord.TotalSales;
                            }
                            
                        }

                        //Console.WriteLine("Total Loan " + TotalLoan);
                        //Console.WriteLine("MustPay" + MustPay);
                        //Console.WriteLine("Customer Balance " + record.Balance);
                        if (record.Balance > TotalLoan - MustPay)
                        {
                            //Console.WriteLine(record.State);
                            record.State = "Suspended";
                        }
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
        
    

