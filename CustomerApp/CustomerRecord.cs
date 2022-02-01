using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
namespace CustomerApp
{
    class CustomerRecord
    {
        public string CustomerID { get; set; }
         public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public int CreditPeriod { get; set; }
        public string State { get; set; }
        public int Balance { get; set; }

        public void Input()
        {
            GetCustID();
            GetName();
            GetAddress();
            GetCreditPeriod();
            GetState();
            GetBalance();
        }

        public void GetCustID()
        {
            try
            {
                Console.Write(" Enter Customer ID: ");
                CustomerID = Console.ReadLine();

                using (var sr = new StreamReader(@"D:\Psiog programs\CustomerApp\Customer.csv"))
                using (var csvreader = new CsvReader(sr, System.Globalization.CultureInfo.CurrentCulture))
                {
                    IEnumerable<CustomerRecord> records = csvreader.GetRecords<CustomerRecord>();
                    foreach (var record in records)
                    {

                        if (record.CustomerID == CustomerID)
                        {
                            Console.WriteLine("CustomerID already axists");
                            GetCustID();
                        }                                                      
                    }
                }

               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                GetCustID();
            }
        }

        public void GetName()
        {
            try
            {
                Console.Write(" Enter Customer Name: ");
                CustomerName = Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                GetName();
            }
        }

        public void GetAddress()
        {
            try
            {
                Console.Write(" Enter Customer Address: ");
                CustomerAddress = Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                GetAddress();
            }
        }

        public void GetCreditPeriod()
        {
            try
            {
                Console.WriteLine("Which Credit Period option do you choose");
                Console.WriteLine("1. Cash Customer:Credit Period = 0");
                Console.WriteLine("2. Credit Customer: Credit Period between 5 to 30 days");
                int option = int.Parse(Console.ReadLine());
                
                switch(option)
                {
                    case 1:
                        CreditPeriod = 0;
                        break;

                    case 2:
                        Console.WriteLine("Enter the Credit period");
                        CreditPeriod = int.Parse(Console.ReadLine());
                        if (!(CreditPeriod >= 5 && CreditPeriod <= 30))
                            Console.WriteLine("Credit period is between 5 to 30 days");
                        break;

                    default:
                        Console.WriteLine("Enter option 1 or 2");
                        break;
                }
                
                              
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                GetCreditPeriod();
            }
        }

        public void GetState()
        {
            State = "Active";
        }

        public void GetBalance()
        {
            Balance = 0;
        }



    }
}
