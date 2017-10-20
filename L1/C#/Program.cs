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
                string accountNumber = tmp[1].Trim();
                long balance = long.Parse(tmp[2].Trim());
                long limit = long.Parse(tmp[3].Trim());
                Account a = new Account{User = person, Balance = balance, AccountNumber = accountNumber, Limit = limit};
                accounts.Add(a);
            }

            //Filter invalid account
            accounts = accounts.Where(x => AccountValidator.Validate(x.AccountNumber)).ToList();

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
                var fromAccount = accounts.Where(x => x.AccountNumber == t.From).FirstOrDefault();
                var toAccount = accounts.Where(x => x.AccountNumber == t.To).FirstOrDefault();

                //Check if accounts are valid
                if(fromAccount != null && toAccount != null){
                    long maxMoney = fromAccount.Balance + fromAccount.Limit;
                    if(maxMoney <= t.Amount)
                    {
                        fromAccount.Balance -= t.Amount;
                        toAccount.Balance += t.Amount;
                    }
                }
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(numberOfAccounts).Append(Environment.NewLine);
            foreach(var a in accounts)
            {
                sb.Append(a.User).Append(" ").Append(a.Balance).Append(Environment.NewLine);
            }

            File.WriteAllText("level2-1.txt", sb.ToString());

        }

        public static bool Validate(string accountNumber)
        {
            if(string.IsNullOrWhiteSpace(accountNumber))
            {
                return false;
            }
            if(accountNumber.Length != 15)
                return false;
            if(!accountNumber.StartsWith("CAT"))
                return false;
            
            char check1 = accountNumber[3];
            char check2 = accountNumber[4];

            if(!char.IsDigit(check1) || !char.IsDigit(check2))
                return false;

            System.Console.WriteLine("Checking :" + accountNumber);
            var distinctChars = accountNumber.Skip(5).Distinct();
            foreach(char c in distinctChars)
            {
                System.Console.WriteLine("Checking Letter: " + c);
                int count1 = accountNumber.Count(x => x == c);
                System.Console.WriteLine("Found: " + count1);
                int count2 = accountNumber.Count(x => x == char.ToUpper(c));
                System.Console.WriteLine("Found Upper: " + count2);
                if(count1 != count2)
                    return false;
            }

            return true;
        }
    }
}
