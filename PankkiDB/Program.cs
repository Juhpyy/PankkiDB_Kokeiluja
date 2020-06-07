using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
namespace PankkiDB

{
    public static class BankDefs
    {
        public const int BankAccount = 1;
        public const int CreditAccount = 2;
    }


    class Program
    {
        public static PankkiEntities context;
        //          static List<Asiakas> customerList;
        //          static List<PankkiTili> accountList;
        static void Main(string[] args)
        {
            context = new PankkiEntities();
            //              customerList = new List<Asiakas>();
            //              accountList = new List<PankkiTili>();
            // App title
            Console.WriteLine("BANK");
            Console.WriteLine("====");
            bool leaveBank = default;
            do
            {
                switch (GUIMainDisplay())
                {
                    case 0:
                        leaveBank = true;
                        Console.WriteLine("Leaving Bank...");
                        break;
                    case 1:
                        GUICreateCustomer();
                        break;
                    case 2:
                        GUICreateBankAccount();
                        break;
                    case 3:
                        GUICreateCreditAccount();
                        break;
                    case 4:
                        GUIJoinCustomerAccount();
                        break;
                    case 5:
                        GUIShowCustomer();
                        break;
                    case 6:
                        GUIShowAccount();
                        break;
                    case 7:
                        GUIDeleteCustomer();
                        break;
                    case 8:
                        GUIDeleteAccount();
                        break;
                    case 9:
                        GUIChangeSaldo();
                        break;
                    case 10:
                        GUIAsiakasToiseen();
                        break;
                }
            } while (!leaveBank);
            // end program
            Console.ReadLine();
        }
        private static int GUIMainDisplay()
        {
            bool validResponse = false;
            int response;
            do
            {
                Console.WriteLine(@"
                           Select Activity (0 to 10)
                           0) Lopetus
                           1) Uusi Asiakas
                           2) Uusi Pankkitili
                           3) Uusi Luottotili
                           4) Liitä tili asiakkaalle
                           5) Näytä asiakkaat
                           6) Näytä tilit
                           7) Poista asiakas
                           8) Poista tili
                           9) Tee tilitapahtumia (nosto ja talletus)
                           10) Siirrä tili asiakkaalta toiselle

                        ");
                string guessInput = Console.ReadLine();
                // convert string to number
                validResponse = int.TryParse(guessInput, out response);
            } while (!validResponse);
            return response;
        }
        private static void GUICreateCustomer()
        {
            Console.WriteLine(@"
                           Customer First Name?
            ");
            string firstInput = Console.ReadLine();
            Console.WriteLine(@"
                           Customer Family Name?
            ");
            string familyInput = Console.ReadLine();
            if (confirmInput())
            {
                var newCustomer = new customers()
                {
                    customer_first_name = firstInput,
                    customer_last_name = familyInput,
                };
                context.customers.Add(newCustomer);
                context.SaveChanges();
            }
        }
        private static void GUICreateBankAccount()
        {
            bool validResponse = false;
            int customerNumber = default;
            //int accountType = 1;
            decimal creditLimit = default;
            decimal currentSaldo = default;
            GUIShowCustomer();

            do
            {
                Console.WriteLine(@"
                           Who gets new account ( customer number)?
            ");
                string guessInput = Console.ReadLine();
                // convert string to number
                validResponse = int.TryParse(guessInput, out customerNumber);
            } while (!validResponse);

            /*
            do
            {
                Console.WriteLine(@"
                           What is account type (1=bank, 2=credit)?
            ");
                string guessInput = Console.ReadLine();
                // convert string to number
                validResponse = int.TryParse(guessInput, out accountType);
            } while (!validResponse);
            if (accountType.Equals(2))
            {
                do
                {
                    Console.WriteLine(@"
                           Credit limit?
            ");
                    string guessInput = Console.ReadLine();
                    // convert string to number
                    validResponse = decimal.TryParse(guessInput, out creditLimit);
                } while (!validResponse);
            }
            */

            do
            {
                Console.WriteLine(@"
                           Current saldo?
            ");
                string guessInput = Console.ReadLine();
                // convert string to number
                validResponse = decimal.TryParse(guessInput, out currentSaldo);
            } while (!validResponse);

            if (confirmInput())
            {
                var henkilö = context.customers.FirstOrDefault<customers>
                             (x => x.customer_number.Equals(customerNumber));
                var newAccount = new accounts()
                {
                    account_name = henkilö.customer_last_name,
                    account_type = BankDefs.BankAccount,
                    account_saldo = currentSaldo,
                    credit_limit = creditLimit,
                    customer_number = customerNumber
                };
                context.accounts.Add(newAccount);
                context.SaveChanges();
            }
        }
        private static void GUICreateCreditAccount()
        {
            bool validResponse = false;
            int customerNumber = default;
            //int accountType = 2;
            decimal creditLimit = default;
            decimal currentSaldo = default;
            GUIShowCustomer();


            do
            {
                Console.WriteLine(@"
                           Who gets new account ( customer number)?
            ");
                string guessInput = Console.ReadLine();
                // convert string to number
                validResponse = int.TryParse(guessInput, out customerNumber);
            } while (!validResponse);

            do
            {
                Console.WriteLine(@"
                           Credit limit?
            ");
                string guessInput = Console.ReadLine();
                // convert string to number
                validResponse = decimal.TryParse(guessInput, out creditLimit);
            } while (!validResponse);

            do
            {
                Console.WriteLine(@"
                           Current saldo?
            ");
                string guessInput = Console.ReadLine();
                // convert string to number
                validResponse = decimal.TryParse(guessInput, out currentSaldo);
            } while (!validResponse);

            if (confirmInput())
            {
                var henkilö = context.customers.FirstOrDefault<customers>
                             (x => x.customer_number.Equals(customerNumber));
                var newAccount = new accounts()
                {
                    account_name = henkilö.customer_last_name,
                    account_type = BankDefs.CreditAccount,
                    account_saldo = currentSaldo,
                    credit_limit = creditLimit,
                    customer_number = customerNumber
                };
                context.accounts.Add(newAccount);
                context.SaveChanges();
            }

        }
        private static void GUIJoinCustomerAccount()
        {
            bool validResponse = false;
            int response = default;
            int response2 = default;
            GUIShowAccount();
            GUIShowCustomer();
            do
            {
                Console.WriteLine(@"
                           Select account number?
            ");
                string guessInput = Console.ReadLine();
                validResponse = int.TryParse(guessInput, out response);
            } while (!validResponse);
            do
            {
                Console.WriteLine(@"
                           Select customer number?
            ");
                string guessInput = Console.ReadLine();
                validResponse = int.TryParse(guessInput, out response2);
            } while (!validResponse);

        }

        private static void GUIShowCustomer()
        {
            Console.WriteLine(@"
                           Customer List:
            ");
            var list =
                from customer in context.customers
                select new
                {
                    Customer = customer.customer_number +
                    " " + customer.customer_first_name +
                    " " + customer.customer_last_name,
                    Account = customer.customer_number
                };



            foreach (var iter in list)
            {
                Console.Write("  " + iter.Customer);
                Console.Write(" Tilisi ovat: ");



                foreach (accounts iter2 in context.accounts)
                {
                    if (iter2.customer_number.Equals(iter.Account))
                    {
                        Console.Write(iter2.account_number + " ");
                    }
                }
                Console.WriteLine("");
            }

            Console.WriteLine("Press Key to continue");
            Console.ReadLine();
        }

        private static void GUIShowAccount()
        {
            Console.WriteLine(@"
                           Account List:
            ");


            foreach (accounts iter in context.accounts)
            {
                Console.Write(iter.account_number + " ");
                Console.Write(iter.account_name + ": ");
                Console.Write(iter.account_saldo + " ");
                Console.WriteLine(iter.credit_limit + " ");

            }

            Console.WriteLine("Press Key to continue");
            Console.ReadLine();
        }

        private static void GUIDeleteCustomer()
        {
            GUIShowCustomer();
            bool validResponse = false;
            int response;
            do
            {
                Console.WriteLine(@"
                           Select Customernumber to be deleted
                        ");
                string guessInput = Console.ReadLine();
                // convert string to number
                validResponse = int.TryParse(guessInput, out response);
            } while (!validResponse);

            if (confirmInput())
            {
                var dummy = context.customers.FirstOrDefault<customers>(x => x.customer_number.Equals(response));
                var dummy2 = context.accounts.FirstOrDefault<accounts>(x => x.customer_number.Equals(response));
                if (dummy2 is null)
                {
                    context.customers.Remove(dummy);
                    context.SaveChanges();
                }
                else
                {
                    Console.WriteLine("Cannot remove customer with account!");
                }
            }

        }
        private static void GUIDeleteAccount()
        {
            GUIShowAccount();
            bool validResponse = false;
            int response;
            do
            {
                Console.WriteLine(@"
                           Select Account number to be deleted
                        ");
                string guessInput = Console.ReadLine();
                // convert string to number
                validResponse = int.TryParse(guessInput, out response);
            } while (!validResponse);

            if (confirmInput())
            {
                var dummy = context.accounts.FirstOrDefault<accounts>(x => x.account_number.Equals(response));

                context.accounts.Remove(dummy);
                context.SaveChanges();
            }
        }
        private static void GUIChangeSaldo()
        {
            GUIShowAccount();
            bool validResponse = false;
            int response = default;
            decimal response2 = default;
            do
            {
                Console.WriteLine(@"
                           Select Account?
            ");
                string guessInput = Console.ReadLine();
                // convert string to number
                validResponse = int.TryParse(guessInput, out response);
            } while (!validResponse);
            do
            {
                Console.WriteLine(@"
                           Initial change in saldo (negative for withdrawal)?
            ");
                string guessInput = Console.ReadLine();
                // convert string to number
                validResponse = decimal.TryParse(guessInput, out response2);
            } while (!validResponse);

            if (confirmInput())
            {
                accounts dummy = context.accounts.FirstOrDefault<accounts>(x => x.account_number.Equals(response));

                if (dummy.account_type.Equals(BankDefs.BankAccount))
                {
                    dummy.account_saldo += response2;
                    context.SaveChanges();
                    Console.WriteLine($"Tilin {response} saldoa muutettu...");
                }
                else
                {
                    decimal? dummy2 = dummy.account_saldo + response2;
                    if (dummy2 >= 0)
                    {
                        dummy.account_saldo += response2;
                        context.SaveChanges();
                        Console.WriteLine($"Tilin {response} saldoa muutettu...");
                    }
                    else if (dummy2 >= -dummy.credit_limit)
                    {
                        dummy.account_saldo += response2;
                        context.SaveChanges();
                        Console.WriteLine($"Tili käyttää nyt luottoa..");
                    }
                    else
                    {
                        Console.WriteLine($"Tilin saldo liian alhainen...");
                    }

                }

            }

        }
        private static void GUIAsiakasToiseen()
        {
            bool validResponse = false;
            int response = default;
            int response2 = default;
            GUIShowAccount();
            GUIShowCustomer();
            do
            {
                Console.WriteLine(@"
                           Select account number?
            ");
                string guessInput = Console.ReadLine();
                validResponse = int.TryParse(guessInput, out response);
            } while (!validResponse);
            do
            {
                Console.WriteLine(@"
                           Select customer number?
            ");
                string guessInput = Console.ReadLine();
                validResponse = int.TryParse(guessInput, out response2);
            } while (!validResponse);

            if (confirmInput())
            {
                accounts dummy1 = context.accounts.Find(response);
                dummy1.customer_number = response2;
                context.SaveChanges();
                Console.WriteLine($"Tilillä {response} on uusi käyttäjä...");

            }

        }

        private static bool confirmInput()
        {
            bool response = default;
            Console.WriteLine("Confirm Y/N?");
            string confirmInput = Console.ReadLine();
            if (confirmInput.ToUpper() == "Y")
            {
                return true;
            }
            return response;
        }
    }
}