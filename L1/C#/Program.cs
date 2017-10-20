using System;
using System.Collections.Generic;

namespace C_
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

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
        }
    }
}
