using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.Helpers
{
    public static class NumberToWordExtensionMethods
    {
        public static string ToThai(this double number)
        {
            return NumberToWordExtensionMethods.ThaiNumber(number.ToString());
        }

        public static string ToThai(this double number, string format)
        {
            return NumberToWordExtensionMethods.ThaiNumber(number.ToString(format));
        }

        public static string ToThai(this double number, IFormatProvider format)
        {
            return NumberToWordExtensionMethods.ThaiNumber(number.ToString(format));
        }

        public static string ToWord(this double number)
        {
            string[] word = number.ToString().Split('.');

            string inttext = NumberToWordExtensionMethods.IntToWord(word[0]);
            string dectext = (word.Length > 1) ? NumberToWordExtensionMethods.DecToWord(word[1]) : string.Empty;

            return (word.Length == 1) ? ((inttext != string.Empty) ? inttext : "zero") : string.Format("{0} point {1}", (inttext != string.Empty) ? inttext : "zero", dectext);
        }

        public static string ToThaiWord(this double number)
        {
            string[] word = number.ToString().Split('.');

            string inttext = NumberToWordExtensionMethods.IntToThaiWord(word[0]);
            string dectext = (word.Length > 1) ? NumberToWordExtensionMethods.DecToThaiWord(word[1]) : string.Empty;

            return (word.Length == 1) ? ((inttext != string.Empty) ? inttext : "ศูนย์") : string.Format("{0}จุด{1}", (inttext != string.Empty) ? inttext : "ศูนย์", dectext);
        }

        public static string ToBaht(this double number)
        {
            string[] word = number.ToString().Split('.');

            string inttext = NumberToWordExtensionMethods.IntToThaiWord(word[0]);
            string dectext = (word.Length > 1) ? (word[1].Length <= 2) ? ((inttext != string.Empty) ? "บาท" : string.Empty) + NumberToWordExtensionMethods.IntToThaiWord((word[1].Length == 1) ? word[1] + "0" : word[1]) + "สตางค์" : ((inttext != string.Empty) ? string.Empty : "ศูนย์") + "จุด" + NumberToWordExtensionMethods.DecToThaiWord(word[1]) + "บาท" : string.Empty;

            return (word.Length == 1) ? (inttext == string.Empty) ? "ศูนย์บาท" : inttext + "บาทถ้วน" : inttext + dectext;
        }

        private static string ThaiNumber(string text)
        {
            string result = string.Empty;

            char[] thai = new char[] { '๐', '๑', '๒', '๓', '๔', '๕', '๖', '๗', '๘', '๙' };

            foreach (char c in text.ToCharArray())
            {
                result += (!char.IsDigit(c)) ? c : thai[Convert.ToInt16(c.ToString())];
            }

            return result;
        }

        private static string IntToWord(string str)
        {
            string[] scale = new string[] { string.Empty, "Thousand", "Million", "Billion", "Trillion", "Quadrillion", "Quintillion" };

            List<string> word = new List<string>();
            List<string> num = NumberToWordExtensionMethods.SplitString(str, 3);

            for (int i = 0; i < num.Count; i++)
            {
                string numEng = NumberToWordExtensionMethods.NumToEng(num[i]) + ((scale[i] != string.Empty) ? string.Format(" {0}", scale[i]) : string.Empty);

                word.Add(numEng);
            }

            string[] result = word.ToArray();
            Array.Reverse(result);

            return string.Join(" ", result);
        }

        private static string IntToThaiWord(string str)
        {
            List<string> word = new List<string>();
            List<string> num = NumberToWordExtensionMethods.SplitString(str, 6);

            for (int i = 0; i < num.Count; i++)
            {
                string numThai = NumberToWordExtensionMethods.NumToThai(num[i]) + string.Join(string.Empty, Enumerable.Repeat<string>("ล้าน", i).ToArray());

                word.Add(numThai);
            }

            string[] result = word.ToArray();
            Array.Reverse(result);

            return string.Join(string.Empty, result);
        }

        private static string DecToWord(string str)
        {
            string[] word = new string[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine" };

            return string.Join(" ", str.ToCharArray().Select(c => word[int.Parse(c.ToString())]).ToArray());
        }

        private static string DecToThaiWord(string str)
        {
            string[] thai = new string[] { "ศูนย์", "หนึ่ง", "สอง", "สาม", "สี่", "ห้า", "หก", "เจ็ด", "แปด", "เก้า" };

            return string.Join(string.Empty, str.ToCharArray().Select(c => thai[int.Parse(c.ToString())]).ToArray());
        }

        private static string NumToEng(string str)
        {
            string[] small = new string[] { string.Empty, "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine" };
            string[] teens = new string[] { "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
            string[] tens = new string[] { string.Empty, string.Empty, "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

            string result = string.Empty;

            if (str.Length == 2)
            {
                if (str.Substring(1, 1) == "1")
                {
                    result = teens[int.Parse(str.Substring(0, 1))];
                }
                else
                {
                    string one = small[int.Parse(str.Substring(0, 1))];

                    result = tens[int.Parse(str.Substring(1, 1))] + ((one != string.Empty) ? string.Format(" {0}", one) : string.Empty);
                }
            }
            else
            {
                if (str.Length == 3)
                {
                    string three = small[int.Parse(str.Substring(2))];
                    string two = NumberToWordExtensionMethods.NumToEng(str.Substring(0, 2));

                    result = ((three != string.Empty) ? string.Format("{0} Hundred", three) : string.Empty) + ((two != string.Empty) ? string.Format(" and {0}", two) : string.Empty);
                }
                else
                {
                    result = small[int.Parse(str)];
                }
            }

            return result;
        }

        private static string NumToThai(string str)
        {
            string[] position = new string[] { string.Empty, "สิบ", "ร้อย", "พัน", "หมื่น", "แสน" };
            string[] thai = new string[] { "ศูนย์", "หนึ่ง", "สอง", "สาม", "สี่", "ห้า", "หก", "เจ็ด", "แปด", "เก้า" };
            char[] charArray = str.ToCharArray();
            List<string> word = new List<string>();

            for (int i = 0; i < str.Length; i++)
            {
                if (charArray[i] != '0')
                {
                    if (charArray[i] == '1' && i == 0)
                    {
                        word.Add((str.Length > 1) ? (Convert.ToInt64(str) != 10) ? "เอ็ด" : "หนึ่ง" : "หนึ่ง");
                    }
                    else if (charArray[i] == '1' && i == 1)
                    {
                        word.Add(position[i]);
                    }
                    else if (charArray[i] == '2' && i == 1)
                    {
                        word.Add("ยี่" + position[i]);
                    }
                    else
                    {
                        word.Add(thai[Convert.ToInt16(charArray[i].ToString())] + position[i]);
                    }
                }
            }

            string[] result = word.ToArray();
            Array.Reverse(result);

            return string.Join(string.Empty, result);
        }

        private static List<string> SplitString(string str, int length)
        {
            List<string> result = new List<string>();
            string word = string.Empty;
            char[] charArray = str.ToCharArray();

            for (int i = str.Length - 1; i >= 0; i--)
            {
                if (word.Length < length)
                {
                    word += charArray[i];
                }

                if (i == 0 || word.Length == length)
                {
                    result.Add(word);
                    word = string.Empty;
                }
            }

            return result;
        }

    }
}