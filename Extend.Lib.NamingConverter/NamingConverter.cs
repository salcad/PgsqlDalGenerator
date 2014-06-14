using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Extend.Lib.NamingConverter
{
    public class NamingConverter
    {
        public static List<string> FindWordsThatNotUpperCase
            (string str)
        {
            var upper = str.Split(' ')
                        .Where(s =>
                            !String.Equals(s, s.ToUpper(),
                            StringComparison.Ordinal)).ToList();

            return (upper);
        }

        public static string Pascal2Pg
            (string input)
        {
            var rgx = @"([A-Z])([A-Z][a-z])|([a-z0-9])([A-Z])";
            return Regex.Replace(input, rgx, "$1$3_$2$4").ToLower();
        }

        public static string Pg2Pascal
            (string input)
        {
            string output = string.Empty;

            string[] words = input.Split('_');
            for (int i = 0; i < words.Length; i++)
            {
                string word = words[i].ToLower();
                if ((word == "id") || (word == "pk") || (word == "fk"))
                    output = output + word.ToUpper();
                else
                    output = output + FirstCharUpper(word);
            }

            return output;
        }

        public static string FirstCharUpper(string expression)
        {
            string result = expression;
            string upperFirstChar;

            if (!String.IsNullOrEmpty(expression))
            {
                if (expression.Length > 0)
                {
                    upperFirstChar = expression.Substring(0, 1).ToUpper();
                    string remainString = expression.Substring(1).ToLower();

                    result = upperFirstChar + remainString;
                }
            }

            return result.Trim();
        }


        public static string PgFPar2Camel(string expression)
        {
            string result = string.Empty;
            if (expression.Length > 2)
                result = expression.Substring(2);

            result = Pg2Pascal(result);
            string lowerFirstChar = result.Substring(0, 1).ToLower();
            string remainString = result.Substring(1);

            result = lowerFirstChar + remainString;
            return result;
        }

        public static string Pg2Camel(string expression)
        {
            string result = Pg2Pascal(expression);
            string lowerFirstChar = result.Substring(0, 1).ToLower();
            string remainString = result.Substring(1);

            result = lowerFirstChar + remainString;
            return result;
        }

        public static string ConvertPK2ID(string expression, bool isUpper)
        {
            if (expression.Length > 2)
            {
                if (expression.Substring(expression.Length - 2, 2).Trim().ToLower() == "pk")
                {
                    if (isUpper)
                    {
                        expression = expression.Substring(0, expression.Length - 2) + "ID";
                    }
                    else
                    {
                        expression = expression.Substring(0, expression.Length - 2) + "id";
                    }
                }
            }

            return expression;
        }

       
        // \b      Word break:
        // Matches where a word starts.
        // [a-z]   Matches any lowercase ASCII letter.
        // We only need to match words with lowercase first letters.
        // This is a character range expression.
        //\w+     Word characters:
        // Matches must have one or more characters.
        public static string UpperFirst(string s)
        {
            return Regex.Replace(s, @"\b[a-z]\w+", delegate(Match match)
            {
                string v = match.ToString();
                return char.ToUpper(v[0]) + v.Substring(1);
            });
        }

    }
}
