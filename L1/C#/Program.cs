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

            /*int numberOfAccounts = int.Parse(Console.ReadLine());

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
            }*/

            //Filter invalid account
            //accounts = accounts.Where(x => Validate(x.AccountNumber)).ToList();

            int numberOfTransactions = int.Parse(Console.ReadLine());
            List<Transaction> transactions = new List<Transaction>(numberOfTransactions);
            for(int i = 0; i < numberOfTransactions; i++)
            {
                int index = 0;
                string line = Console.ReadLine();
                string[] tmp = line.Split(' ');
                string id = tmp[index].Trim();
                index++;

                int numberOfInputs = int.Parse(tmp[index].Trim());
                index++;

                List<Transaction> inputs = new List<Transaction>();
                for(int inp = 0; inp < numberOfInputs; inp++)
                {
                    string t_id = tmp[index].Trim();
                    index++;
                    string owner = tmp[index].Trim();
                    index++;
                    long amount = long.Parse(tmp[index].Trim());
                    index++;

                    Transaction t = new Transaction{Id = t_id, Owner = owner, Amount = amount};
                    inputs.Add(t);
                }

                int numberOfOutputs = int.Parse(tmp[index].Trim());
                index++;

                List<Transaction> outputs = new List<Transaction>();
                for(int o = 0; o < numberOfOutputs; o++)
                {
                    string owner = tmp[index].Trim();
                    index++;
                    long amount = long.Parse(tmp[index].Trim());
                    index++;

                    Transaction t = new Transaction{Owner = owner, Amount = amount};
                    outputs.Add(t);
                }

                long time  = long.Parse(tmp[index].Trim());
                Transaction tr = new Transaction{Id = id, Input = inputs, Output = outputs, Time = time, Text = line};
                transactions.Add(tr);
            }

            var orderedTransactions = transactions.OrderBy(x => x.Time).ToList();
            List<string> transactionIds = new List<string>();
            var validTransactions = orderedTransactions.Where(x => Validate(x, transactionIds)).ToList();


            StringBuilder sb = new StringBuilder();
            sb.Append(validTransactions.Count).Append(Environment.NewLine);
            foreach(var t in validTransactions)
            {
                /*sb.Append(t.Id).Append(" ").Append(t.Input.Count)
                .Append(" ");
                foreach(var input in t.Input){
                    sb.Append(input.Id).Append(" ").Append(input.Owner).Append(" ")
                    .Append(input.Amount);
                }
                sb.Append(" ").Append(t.Output.Count).Append(" ");
                foreach(var output in t.Output){
                    sb.Append(output.Owner).Append(" ")
                    .Append(output.Amount);
                }*/
                sb.Append(t.Text).Append(Environment.NewLine);
            }

            File.WriteAllText("level3-3.txt", sb.ToString());

        }

        public static bool Validate(Transaction transaction, List<string> transactionPool){

            //Check every Amount >= 0
            if(transaction.Input.Any(x => x.Amount <= 0) || 
               transaction.Output.Any(x => x.Amount <= 0)){
                    return false;
            }
            
            foreach(var input in transaction.Input)
            {
                if(!transactionPool.Contains(input.Id) && input.Owner != "origin")
                {
                    return false;
                }
            }

            long sum1 = transaction.Input.Sum(x => x.Amount);
            long sum2 = transaction.Output.Sum(x => x.Amount);

            if(sum1 != sum2) return false;

            //Check output unique owners
            bool isUnique = transaction.Output.Select(x => x.Owner).Distinct().Count() == transaction.Output.Select(x => x.Owner).Count();
            if(!isUnique) return false;

            //Check input unique IDs
            bool isUniqueId = transaction.Input.Select(x => x.Id).Distinct().Count() == transaction.Input.Select(x => x.Id).Count();
            if(!isUniqueId) return false;


            foreach(var input in transaction.Input)
            {
                if(input.Owner != "origin")
                {
                    transactionPool.Remove(input.Id);
                }
            }

            transactionPool.Add(transaction.Id);
            return true;
        }

    }
}
