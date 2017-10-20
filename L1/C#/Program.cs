using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace C_
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");

            int numberOfAccounts = int.Parse(Console.ReadLine());

            List<Account> accounts = new List<Account>(numberOfAccounts);
            for(int i = 0; i < numberOfAccounts; i++)
            {
                string[] tmp = Console.ReadLine().Split(' ');
                string person = tmp[0].Trim();
                long balance = long.Parse(tmp[1].Trim());
                Account a = new Account{User = person, Balance = balance};
                accounts.Add(a);
            }
            int numberOfTransactions = int.Parse(Console.ReadLine());
            List<Transaction> transactions = new List<Transaction>(numberOfTransactions);
            for(int i = 0; i < numberOfTransactions; i++)
            {
                string[] tmp = Console.ReadLine().Split(' ');
                string personFrom = tmp[0].Trim();
                string personTo = tmp[1].Trim();
                long amount  = long.Parse(tmp[2].Trim());
                long time  = long.Parse(tmp[3].Trim());
                Transaction t = new Transaction{From = personFrom, To = personTo, Amount = amount, Time = time};
                transactions.Add(t);
            }

            var orderedTransactions = transactions.OrderBy(x => x.Time);
            foreach(var t in orderedTransactions)
            {
                var fromAccount = accounts.Where(x => x.User == t.From).First();
                var toAccount = accounts.Where(x => x.User == t.To).First();

                fromAccount.Balance -= t.Amount;
                toAccount.Balance += t.Amount;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(numberOfAccounts).Append(Environment.NewLine);
            foreach(var a in accounts)
            {
                sb.Append(a.User).Append(" ").Append(a.Balance).Append(Environment.NewLine);
            }

            File.WriteAllText("l1.txt", sb.ToString());

        }
    }
}
