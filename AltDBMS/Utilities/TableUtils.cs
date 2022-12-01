using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OwnDBMS.Structures;

namespace OwnDBMS.Utilities
{
    public class TableUtils
    {
        public static string Slice(string text, int beginning, int ending)
        {
            string sliced = "";
            for (int i = beginning; i < ending; i++)
            {
                char c = text[i];
                sliced += c;
            }
            return sliced;
        }

        public static string Slice(string text, int beginning)
        {
            string sliced = "";
            for (int i = beginning; i < text.Length; i++)
            {
                char c = text[i];
                sliced += c;
            }
            return sliced;
        }
        public static string[] Slice(string[] arr, int beginning)
        {
            string[] sliced = new string[arr.Length - beginning];
            int k = 0;
            for (int i = beginning; i < arr.Length; i++, k++)
            {
                sliced[k] = arr[i];
            }
            return sliced;
        }
        public static string[] Slice(string[] arr, int beginning, int ending)
        {
            string[] sliced = new string[ending - beginning];
            int k = 0;
            for (int i = beginning; i < ending; i++, k++)
            {
                sliced[k] = arr[i];
            }
            return sliced;
        }

        public static string ToUpper(string input)
        {
            string upper = "";
            for (int i = 0; i < input.Length; i++)
                if (input[i] >= 'a' && input[i] <= 'z')
                    upper += Convert.ToChar(input[i] - 32);

            return upper;
        }
    }
}
