using System;
using System.Collections.Generic;

namespace C_
{
    public class Account
    {
        public long Balance {get;set;}
        public string User {get;set;}
        public string AccountNumber {get;set;}
        public long Limit {get;set;}

        public static bool Validate(string accountNumber)
        {
            if(string.IsNullOrWhiteSpace(accountNumber))
                return false;
            if(accountNumber.Length != 15)
                return false;
            if(!accountNumber.StartsWith("CAT"))
                return false;

            return ValidateBAN(accountNumber);
        }

        public static bool ValidateBAN(string account_name)
        {
            var char_dict=new Dictionary<char, int>();
            char[] elements = account_name.Substring(5).ToCharArray();

            long checksum = 0;

            // parse string
            foreach(var c in elements)
            {
                if(!char.IsLetter(c))
                {
                    System.Console.WriteLine("IS NOT A LETTER IN BAN!!!");
                    return false;
                }
                    

                checksum += c;

                if(char_dict.ContainsKey(c))
                    char_dict[c]++;
                else
                {
                    char_dict.Add(c, 1);
                }
            }

            foreach(var c in char_dict.Keys)
            {
                if(char.IsUpper(c))
                {
                    if(!char_dict.ContainsKey(Char.ToLower(c)) || char_dict[c] != char_dict[Char.ToLower(c)])
                        return false;
                }
                else{
                    if(!char_dict.ContainsKey(Char.ToUpper(c)) || char_dict[c] != char_dict[Char.ToUpper(c)])
                        return false;
                }
            }

            checksum += 'C' + 'A' + 'T' + '0' + '0';
            checksum = 98 - (checksum % 97);
            
            if(account_name != "CAT" + checksum.ToString() + account_name.Substring(5))
                return false;

            return true;
        }

    
    }
}