using System;
using System.Collections.Generic;

namespace C_
{
    public class Transaction
    {
        public Transaction()
        {
            Input = new List<Transaction>();
            Output = new List<Transaction>();
        }

        public string Id {get;set;}

        public string Owner {get; set;}

        public long Amount {get;set;}

        public long Time {get;set;}

        public List<Transaction> Input {get;set;}

        public List<Transaction> Output {get;set;}

        public string Text {get;set;}
    }
}