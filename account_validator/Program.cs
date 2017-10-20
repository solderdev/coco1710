using System;
using System.Collections.Generic;

namespace C_
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = AccountValidator.validate(args[0]);
            Console.WriteLine(result);
        }
    }

    class AccountValidator
    {
        public static bool validate(string account_name)
        {
            if(!account_name.StartsWith("CAT"))
                return false;

            var char_dict=new Dictionary<char, uint>();
            char[] elements = account_name.Substring(5).ToCharArray();

            long checksum = 0;

            // parse string
            foreach(var c in elements)
            {
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
            checksum = 98 - checksum % 97;
            
            if(account_name != "CAT" + checksum.ToString() + account_name.Substring(5))
                return false;

            return true;
        }
    }
}
