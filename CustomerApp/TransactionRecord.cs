using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using System.Security.Cryptography.X509Certificates;

namespace CustomerApp
{
    class TransactionRecord
    {
        public string Date { get; set; }
        public string CustomerID { get; set; }
        public string ItemID { get; set; }
        public string TransactionType { get; set; }
        public int Price { get; set; }
        public int QOS { get; set; }
        public int TotalSales { get; set; }
        public int CreditPeriod { get; set; }

        public List<string> ErrorList = new List<string>();

        public void Input()
        {
            GetCustomerID();
            GetDate();
            GetItemID();
            GetQOS();
            GetTransactionType();
            GetCreditPeriod();
            if (ErrorList.Count > 0)
                ErrorWrite(ErrorList);
        }

        public void ErrorWrite(List<string> ErrorList)
        {
            if(ErrorList.Count>0)
            {
                string error = CustomerID + "," + Date;
                
                for (int i = 0; i < ErrorList.Count; i++)
                    error += "," + ErrorList[i];

                File.AppendAllText(@"D:\Psiog programs\CustomerApp\ErrorLog.csv", error+"\n");
            }
            

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

                            if (record.State == "Suspended")
                            {
                                Console.WriteLine("Transaction can't be made for suspended ID");
                                GetCustomerID();
                            }


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
         
        public void GetItemID()
        {
            int flag = 0;

            Console.WriteLine("Enter ItemID");
            ItemID = Console.ReadLine();

            using (var sr = new StreamReader(@"D:\Psiog programs\CustomerApp\Inventory.csv"))
            using (var csvreader = new CsvReader(sr, System.Globalization.CultureInfo.CurrentCulture))
            {
                IEnumerable<InventoryRecord> records = csvreader.GetRecords<InventoryRecord>();

                foreach (var record in records)
                {

                    if (record.ItemID == ItemID)
                    {
                        flag = 1;
                        break;

                    }
                }
            }

            if (flag == 0)
            {
                ErrorList.Add("ItemID doesn't exist");
                Console.WriteLine("ItemID doesn't exist");
                GetItemID();
            }


        }

        public void GetQOS()
        {
            int flag = 0;

            Console.WriteLine("Enter the Quantity of Sales");
            QOS = int.Parse(Console.ReadLine());

            if (QOS < 0)
            {
                Console.WriteLine("Qos cant be negative");
                GetQOS();
            }

            using (var sr = new StreamReader(@"D:\Psiog programs\CustomerApp\Inventory.csv"))
            using (var csvreader = new CsvReader(sr, System.Globalization.CultureInfo.CurrentCulture))
            {
                IEnumerable<InventoryRecord> records = csvreader.GetRecords<InventoryRecord>();

                foreach (var record in records)
                {

                    if (record.ItemID == ItemID && record.Stock > QOS)
                    {
                        flag = 1;
                        break;
                    }
                }
            }

            if (flag == 0)
            {
                ErrorList.Add("QOS error");
                Console.WriteLine("QOS cant be more than stock in Inventory");
                GetQOS();
            }

        }

        public void GetTransactionType()
        {
            Console.WriteLine("Enter Transaction Type");
            TransactionType = Console.ReadLine();

            using (var sr = new StreamReader(@"D:\Psiog programs\CustomerApp\Inventory.csv"))
            using (var csvreader = new CsvReader(sr, System.Globalization.CultureInfo.CurrentCulture))
            {
                IEnumerable<InventoryRecord> records = csvreader.GetRecords<InventoryRecord>();

                foreach (var record in records)
                {

                    if (record.ItemID == ItemID)
                    {
                        switch (TransactionType)
                        {

                            case "RET":
                                Price = record.Price;
                                TotalSales = Price * QOS;
                                break;

                            case "WHS":
                                double TempPrice = (record.Price) * (1 - (double)(record.Discount) / (double)(100));
                                Price = (int)(TempPrice);
                                TotalSales = Price * QOS;
                                break;

                            default:
                                Console.WriteLine("Transaction Type should only be in RET or WHS");
                                break;

                        }

                    }
                }
            }

        }

        public void GetCreditPeriod()
        {
            Console.WriteLine("Enter CreditPeriod for this Transaction");
            CreditPeriod = int.Parse(Console.ReadLine());

            using (var sr = new StreamReader(@"D:\Psiog programs\CustomerApp\Customer.csv"))
            using (var csvreader = new CsvReader(sr, System.Globalization.CultureInfo.CurrentCulture))
            {
                IEnumerable<CustomerRecord> records = csvreader.GetRecords<CustomerRecord>();

                foreach (var record in records)
                {

                    if (record.CustomerID == CustomerID)
                    {

                        if (CreditPeriod > record.CreditPeriod)
                        {
                            ErrorList.Add("CreditPeriod error");
                            Console.WriteLine("Credit period for transaction can't be greater than Customer Credit Period");
                            Console.WriteLine("Customer Credit Period: " + record.CreditPeriod);
                            GetCreditPeriod();
                        }

                    }
                }
            }

        }

    }

}

        
            

/*string error;
            error = CustomerID + "," + Date;
            for (int i = 0; i < ErrorList.Count; i++)
                error += "," + ErrorList[i];

            File.AppendAllText(@"D:\Psiog programs\CustomerApp\ErrorLog.csv", error);

        }*/

            /*string error;
            error = CustomerID + "," + Date;
            for (int i = 0; i < ErrorList.Count; i++)
                error += "," + ErrorList[i];

            File.AppendAllText(@"D:\Psiog programs\CustomerApp\ErrorLog.csv", error);*/
               
        /*void ErrorCheck()
        {
            List<string> ErrorList = new List<string>();

            DateTime mydate = DateTime.Parse(Date);
            var now = DateTime.Now;
            var timespan = now.Subtract(mydate);
            if (timespan.Days > 30)
            {
                Console.WriteLine("Transaction for Date before 30 days can't be entered");
                ErrorList.Add("Date");
            }

            if (mydate > now)
            {
                Console.WriteLine("You can't enter Transaction after today");
                ErrorList.Add("Date");
            }

            using (var sr = new StreamReader(@"D:\Psiog programs\InventoryApp\Inventory.csv"))
            using (var csvreader = new CsvReader(sr, System.Globalization.CultureInfo.CurrentCulture))
            {
                IEnumerable<InventoryRecord> records = csvreader.GetRecords<InventoryRecord>();
                int flag = 0;
                foreach (var record in records)
                {

                    if (record.ItemID == ItemID)
                    {
                        flag = 1;
                        Console.WriteLine("Enter the Quantity of Sales");
                        QOS = int.Parse(Console.ReadLine());
                        if (record.Stock > QOS)
                        {
                            Console.WriteLine("Enter Transaction Type");
                            TransactionType = Console.ReadLine();
                            switch (TransactionType)
                            {

                                case "RET":
                                    Price = record.Price;
                                    TotalSales = Price * QOS;
                                    break;

                                case "WHS":
                                    Price = record.Price * (1 - (record.Discount) / 100);
                                    TotalSales = Price * QOS;
                                    break;

                                default:
                                    Console.WriteLine("Transaction Type should only be in RET or WHS");
                                    ErrorList.Add("Transaction Type");
                                    break;

                            }

                        }

                        else
                        {
                            Console.WriteLine("Quantity of Sales can't be more than Inventory Stock");
                            ErrorList.Add("QOS");
                        }

                        break;
                    }

                }

                if (flag == 0)
                {
                    Console.WriteLine("ItemID doesn't exist");
                    ErrorList.Add("ItemID");
                }


            }

            return true;
        }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }


}
        }*/
    

