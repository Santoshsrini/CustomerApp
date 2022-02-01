using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string home = "y";
            do
            {

                Console.WriteLine("Enter username and password");
                string uname = Console.ReadLine();
                string pwd = Console.ReadLine();
                Username n = new Username();
                bool access = n.UsernameAuthentication(uname, pwd);

                if (access == true)
                {
                    Console.WriteLine("Access Accepted");
                    switch (uname)
                    {

                        case "admin":

                            {
                                string ch = "y";
                                do
                                {
                                    Console.WriteLine("1. Add New Customer Record" + "\n" + "2. Update Customer Record"+"\n"+ "3. Suspend"+"\n"+"4. Delete Customer Record");
                                    int choice = int.Parse(Console.ReadLine());
                                    Customer customer = new Customer(@"D:\Psiog programs\CustomerApp\Customer.csv");

                                    switch (choice)
                                    {
                                        case 1:

                                            CustomerRecord rec = new CustomerRecord();
                                            rec.Input();
                                            customer.AddCustomer(rec);
                                            break;

                                        case 2:
                                            customer.UpdateCustomer();
                                            break;

                                        case 3:
                                            customer.SuspendCustomer();
                                            break;

                                        case 4:
                                            customer.DeleteCustomer();
                                            break;

                                        default:
                                            Console.WriteLine("Enter choice between 1 to 4");
                                            break;
                                    }
                                    Console.WriteLine("Do you want to add, update, suspend or delete more Customer records (y/n)");
                                    ch = Console.ReadLine();

                                } while (ch == "y");

                                break;
                            }

                        case "operator":

                            {
                                string ch = "y";
                                do
                                {
                                    Console.WriteLine("1. Add New Sales Record" + "\n" + "2. Add Collection Record");
                                    int choice = int.Parse(Console.ReadLine());
                                    Transaction transaction = new Transaction();
                                    Collection collection = new Collection();

                                    switch (choice)
                                    {
                                        case 1:

                                            TransactionRecord rec = new TransactionRecord();
                                            rec.Input();
                                            transaction.Write(rec);
                                            break;

                                        case 2:
                                            CollectionRecord collectionRecord = new CollectionRecord();
                                            collectionRecord.Input();
                                            collection.WriteCollection(collectionRecord);
                                            break;
                                        

                                        default:
                                            Console.WriteLine("Enter choice between 1 to 2");
                                            break;
                                    }

                                    Console.WriteLine("Do you want to add more sales or collection records");
                                    ch = Console.ReadLine();

                                } while (ch == "y");

                                break;
                            }

                        default:
                            {
                                break;
                            }



                    }

                }

                else
                {
                    Console.WriteLine("Access Denied");

                }

                Console.WriteLine("Do you want to go back to home page (y/n)");
                home = Console.ReadLine();

            } while (home == "y");

            Console.ReadLine();

        }


    }
    
}
