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

            int numberOfRequests = int.Parse(Console.ReadLine());
            List<Transaction> requests = new List<Transaction>(numberOfRequests);
            for(int i = 0; i < numberOfRequests; i++)
            {
                string line = Console.ReadLine();
                string[] tmp = line.Split(' ');
                string id = tmp[0].Trim();
                string fromOwner = tmp[1].Trim();
                string toOwner = tmp[2].Trim();
                long amount  = long.Parse(tmp[3].Trim());
                long time  = long.Parse(tmp[4].Trim());

                Transaction t = new Transaction{Id = id, FromOwner = fromOwner, ToOwner = toOwner, 
                                                Amount = amount, Time = time, Text=line};
                requests.Add(t);
            }

            var orderedRequests = requests.OrderBy(x => x.Time).ToList();
            var orderedTransactions = transactions.OrderBy(x => x.Time).ToList();
            var transactionIds = new HashSet<Tuple<string,string>>();
            var validTransactions = ValidateNew(orderedTransactions, orderedRequests);   //orderedTransactions.Where(x => Validate(x, transactionIds)).ToList();

            StringBuilder sb = new StringBuilder();
            sb.Append(validTransactions.Count).Append(Environment.NewLine);
            foreach(var t in validTransactions)
            {
                sb.Append(t.Text).Append(Environment.NewLine);
            }

            File.WriteAllText("level4-1.txt", sb.ToString());

        }

        public static List<Transaction> ValidateNew(List<Transaction> all, List<Transaction> requests)
        {
            List<Transaction> result = new List<Transaction>();
            var transactionPool = new HashSet<Tuple<string, string>>();

            foreach(var transaction in all)
            {
                var newerRequests = requests.Where(x => x.Time < transaction.Time);
                if(newerRequests.Any())
                {
                    foreach(Transaction request in newerRequests)
                    {
                        if(result.Any(x => x.Output.Any(o => o.Owner == request.FromOwner && o.Amount >= request.Amount))){
                            Transaction newTransaction = new Transaction
                            {

                            };
                        }
                    }
                }
                
                if(Validate(transaction, transactionPool))
                {
                    result.Add(transaction);
                }
            }
            return result;
        }

        public static bool Validate(Transaction transaction, HashSet<Tuple<string,string>> transactionPool){

            //System.Console.WriteLine(transaction.Id + " - " + string.Join(", ", transactionPool));

            //Check every Amount > 0
            if(transaction.Input.Any(x => x.Amount <= 0) || 
               transaction.Output.Any(x => x.Amount <= 0)){
                    return false;
            }
            
            foreach(var input in transaction.Input)
            {
                if(!transactionPool.Contains(new Tuple<string, string>(input.Owner,input.Id)) && input.Owner != "origin")
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
                    bool removed = transactionPool.Remove(new Tuple<string,string>(input.Owner, input.Id));
                    if(!removed) System.Console.WriteLine("NOT REMOVED: " + transaction.Id);
                }
            }

            foreach(var output in transaction.Output)
            {
                bool added = transactionPool.Add(new Tuple<string,string>(output.Owner, transaction.Id));
                if(!added) System.Console.WriteLine("ALREADY EXISTS: " + transaction.Id);
            }
            
            
            return true;
        }

    }
}
