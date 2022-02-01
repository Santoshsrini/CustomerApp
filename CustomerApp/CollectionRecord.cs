using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;

namespace CustomerApp
{
    class CollectionRecord
    {
        public string Date { get; set; }
        public string CustomerID { get; set; }
        public int MoneyReceived { get; set; }

        public void Input()
        {
            GetCustomerID();
            GetDate();
            GetMoneyReceived();
            
        }

        public void GetCustomerID()
        {
            int flag = 0;
            try
            {
                Console.WriteLine("Enter Customer ID");
                CustomerID = Console.ReadLine();
                using (var sr = new StreamReader(@"D:\Psiog programs\CustomerApp\Customer.csv"))
                using (var csvreader = new CsvReader(sr, System.Globalization.CultureInfo.CurrentCulture))
                {
                    IEnumerable<CustomerRecord> records = csvreader.GetRecords<CustomerRecord>();

                    foreach (var record in records)
                    {
                        if (record.CustomerID == CustomerID)
                        {
                            flag = 1;
                            break;
                        }
                    }
                }

                if (flag == 0)
                {
                    Console.WriteLine("CustomerID doesn't exist");
                    GetCustomerID();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                GetCustomerID();
            }

        }

        public void GetDate()
        {
            Console.WriteLine("Enter Date");
            Date = Console.ReadLine();
            DateTime mydate = DateTime.Parse(Date);
            var now = DateTime.Now;
            var timespan = now.Subtract(mydate);
            if (timespan.Days > 30)
            {
                Console.WriteLine("Transaction for Date before 30 days can't be entered");
                GetDate();
            }

            if (mydate > now)
            {
                Console.WriteLine("You can't enter Transaction after today");
                GetDate();
            }
        }

        public void GetMoneyReceived()
        {
            try
            {
                Console.WriteLine("Enter the cash Amount");
                MoneyReceived = int.Parse(Console.ReadLine());

                using (var sr = new StreamReader(@"D:\Psiog programs\CustomerApp\Customer.csv"))
                using (var csvreader = new CsvReader(sr, System.Globalization.CultureInfo.CurrentCulture))
                {
                    IEnumerable<CustomerRecord> records = csvreader.GetRecords<CustomerRecord>();
                    foreach(var record in records)
                    {
                        if(record.CustomerID==CustomerID)
                        {
                            if(record.Balance<MoneyReceived)
                            {
                                Console.WriteLine("You dont have to pay extra cash" + "\n" + "Balance to be paid: " + record.Balance);
                                GetMoneyReceived();
                            }
                        }
                    }
                }
            }
            catch ( Exception ex)
            {
                Console.WriteLine(ex.Message);
                GetMoneyReceived();
            }

        }
    }
}
