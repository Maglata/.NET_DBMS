﻿using System.Globalization;
using DBMSPain.Structures;
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
            {
                if (input[i] >= 'a' && input[i] <= 'z')
                {
                    upper += Convert.ToChar(input[i] - 32);
                    continue;
                }
                upper += input[i];
            }
                
                    

            return upper;
        }
        public static string[] Split(string input, char separator, int count)
        {
            string[] splitinput = new string[count];
            int counter = 0;
            string tempstring = string.Empty;

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == separator && counter != count - 1)
                {
                    splitinput[counter] = tempstring;
                    counter++;
                    tempstring = string.Empty;
                    continue;
                }
                tempstring += input[i];
            }
            splitinput[counter] = tempstring;

            return splitinput;
        }
        public static string[] Split(string input, char separator)
        {
            int counter = 1;
            for (int i = 0; i < input.Length; i++)
                if (input[i] == separator)
                    counter++;
            string[] splitinput = new string[counter];
            counter = -1;
            string tempstring = string.Empty;        
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] != separator)
                {
                    tempstring += input[i];
                }

                if (input[i] == separator || i == input.Length - 1)
                {
                    counter++;
                    splitinput[counter] = tempstring;
                    tempstring = string.Empty;
                }
            }
            return splitinput;
        }
        public static string[] Split(string input, char[] separators)
        {
            int counter = 1;

            for (int i = 0; i < input.Length; i++)
                for (int k = 0; k < separators.Length; k++)
                    if (input[i] == separators[k])
                        counter++;

            string[] splitinput = new string[counter];
            counter = -1;
            string tempstring = string.Empty;
            bool isnotSeparator = false;

            for (int i = 0; i < input.Length; i++)
            {               
                for (int k = 0; k < separators.Length; k++)
                {
                    if (input[i] != separators[k])
                        isnotSeparator = true;
                    if (input[i] == separators[k])
                    {                       
                        isnotSeparator = false;
                        break;
                    }
                }
                if (isnotSeparator)
                    tempstring += input[i];
                else
                {
                    counter++;
                    splitinput[counter] = tempstring;
                    tempstring = string.Empty;
                }
            }
            splitinput[counter + 1] = tempstring;

            return splitinput;
        }
        public static string Trim(string input, char[] trimChars)
        {
            int start = 0;
            int end = input.Length - 1;

            // Find the first non-trim character from the start
            while (start <= end)
            {
                bool found = false;
                for (int i = 0; i < trimChars.Length; i++)
                {
                    // check if the current character is present in the trimChars array
                    if (input[start] == trimChars[i])
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    // if it's not a trim character, then break the loop
                    break;
                }
                start++;
            }

            // Find the last non-trim character from the end
            while (end >= start)
            {
                bool found = false;
                for (int i = 0; i < trimChars.Length; i++)
                {
                    // check if the current character is present in the trimmedchars
                    if (input[end] == trimChars[i])
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    // if it's not a trim character, then break the loop
                    break;
                }
                end--;
            }

            // create a new string with the non-trim characters
            char[] modified = new char[end - start + 1];
            int k = 0;
            for (int i = start; i <= end; i++)
            {
                modified[k++] = input[i];
            }
            // create a new string from the char array
            return new string(modified);
        }
        public static string Trim(string input, char trimChar)
        {
            int start = 0;
            int end = input.Length - 1;

            // Find the first non-trim character from the start
            while (start <= end && input[start] == trimChar)
            {
                start++;
            }

            // Find the last non-trim character from the end
            while (end >= start && input[end] == trimChar)
            {
                end--;
            }

            // create a new string with the non-trim characters
            char[] modified = new char[end - start + 1];
            int k = 0;
            for (int i = start; i <= end; i++)
            {
                modified[k++] = input[i];
            }
            return new string(modified);
        }
        public static string TrimEnd(string input, char[] trimChars)
        {
            int end = input.Length - 1;

            // Find the last non-trim character from the end
            while (end >= 0)
            {
                bool found = false;
                for (int i = 0; i < trimChars.Length; i++)
                {
                    if (input[end] == trimChars[i])
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    break;
                }
                end--;
            }

            // create a new string with the non-trim characters
            char[] modified = new char[end + 1];
            for (int i = 0; i <= end; i++)
            {
                modified[i] = input[i];
            }
            return new string(modified);
        }
        public static string TrimEnd(string input, char trimChar)
        {
            int end = input.Length - 1;

            // Find the last non-trim character from the end
            while (end >= 0 && input[end] == trimChar)
            {
                end--;
            }

            // create a new string with the non-trim characters
            char[] modified = new char[end + 1];
            for (int i = 0; i <= end; i++)
            {
                modified[i] = input[i];
            }
            return new string(modified);
        }
        public static string TrimStart(string input, char trimChar)
        {
            int start = 0;

            // Find the first non-trim character from the start
            while (start < input.Length && input[start] == trimChar)
            {
                start++;
            }

            // create a new string with the non-trim characters
            return input.Substring(start);
        }
        public static string TrimStart(string input)
        {
            int start = 0;

            // Find the first non-white space character from the start
            while (start < input.Length && char.IsWhiteSpace(input[start]))
            {
                start++;
            }

            // create a new string with the non-white space characters
            char[] modified = new char[input.Length - start];
            int k = 0;
            for (int i = start; i < input.Length; i++)
            {
                modified[k++] = input[i];
            }
            return new string(modified);
        }
        public static string Replace(string original, string search, string replacement)
        {
            // Initialize variables
            string modified = "";
            int i = 0;
            while (i < original.Length)
            {
                // Check if substring starting at i matches search string
                bool match = true;
                for (int j = 0; j < search.Length; j++)
                {
                    if (i + j >= original.Length || original[i + j] != search[j])
                    {
                        match = false;
                        break;
                    }
                }
                if (match)
                {
                    // If match, add replacement string to modified
                    modified += replacement;
                    i += search.Length;
                }
                else
                {
                    // If not match, add current character to modified
                    modified += original[i];
                    i++;
                }
            }
            return modified;
        }
        public static bool Contains(string[] input, string item)
        {       
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == item)
                    return true;
            }
            return false;
        }
        public static bool Contains(int[] input, int item)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == item)
                    return true;
            }
            return false;
        }
        public static bool Contains(List<int> input, int item)
        {
            for (int i = 0; i < input.Count; i++)
            {
                if (input[i] == item)
                    return true;
            }
            return false;
        }
        public static void Sort<T>(Type sortindextype ,string[] alllines,ImpLinkedList<T> linkedlist,int sortindex, ImpLinkedList<int> linkedlistindexes, int ascending = 1) where T : IComparable<T>
        {
            // Inputs are given by reference
            // Using IComparable to compare the elements in the array,
            // so this algorithm will work with any data type that implements this interface,
            // including integers, strings, and dates

            string[] selectedcols = new string[linkedlist.Count];

            int index = 0;
            if(linkedlistindexes.Count == 0)
            {
                for (int i = 0; i < selectedcols.Length; i++)
                {
                    selectedcols[i] = alllines[i];
                }
            }
            else
            {
                for (int i = 0; i < selectedcols.Length; i++)
                {
                    selectedcols[i] = alllines[linkedlistindexes.ElementAt(index).Value];
                    index++;
                }
            }
            

            for (int i = 0; i < selectedcols.Length - 1; i++)
            {
                for (int j = i + 1; j < selectedcols.Length; j++)
                {
                    int comparison = 0;
                    var colvalues = TableUtils.Split(selectedcols[i], '\t');
                    var nextcolvalues = TableUtils.Split(selectedcols[j], '\t');
                    // Checks for the type and compares with the appropriate parse
                    switch (Type.GetTypeCode(sortindextype))
                    {
                        case TypeCode.Int32:
                            comparison = int.Parse(colvalues[sortindex]).CompareTo(int.Parse(nextcolvalues[sortindex]));
                            break;
                        case TypeCode.DateTime:
                            {
                                //colvalues[sortindex] = colvalues[sortindex].Trim('"');
                                colvalues[sortindex] = TableUtils.Trim(colvalues[sortindex],'"');
                                //nextcolvalues[sortindex] = nextcolvalues[sortindex].Trim('"');
                                nextcolvalues[sortindex] = TableUtils.Trim(nextcolvalues[sortindex],'"');
                                comparison = DateTime.ParseExact(colvalues[sortindex], "dd.MM.yyyy", CultureInfo.InvariantCulture).
                                    CompareTo(DateTime.ParseExact(nextcolvalues[sortindex], "dd.MM.yyyy", CultureInfo.InvariantCulture));
                                
                            }                         
                            break;
                        case TypeCode.String:
                            comparison = colvalues[sortindex].CompareTo(nextcolvalues[sortindex]);
                            break;
                    }
                    
                    if ((ascending == 1 && comparison > 0) || (ascending == -1 && comparison < 0))
                    {
                        T temp = linkedlist.ElementAt(i).Value;
                        linkedlist.ElementAt(i).Value = linkedlist.ElementAt(j).Value;
                        linkedlist.ElementAt(j).Value = temp;

                        string temp2 = selectedcols[i];
                        selectedcols[i] = selectedcols[j];
                        selectedcols[j] = temp2;
                    }
                }
            }


        }
        public static bool Contains(ImpLinkedList<ulong> input, ulong item)
        {
            if (input == null)
                return false;

            for (int i = 0; i < input.Count; i++)
            {
                if (input.ElementAt(i).Value == item)
                    return true;
            }
            return false;
        }
        public static Node CreateTree(List<Token> tokens)
        {
            ImpStack<Node> nodes = new();

            for (int i = 0; i < tokens.Count; i++)
            {
                switch (tokens[i].type)
                {
                    case Token.Type.CONDITION: nodes.Push(new Node(tokens[i].Value)); break;
                    case Token.Type.NOT:
                        {
                            Node leftnode = nodes.Pop();
                            nodes.Push(new Node(tokens[i].Value, leftnode));
                            break;
                        }
                    default:
                        {
                            Node leftnode = nodes.Pop();
                            Node rightnode = nodes.Pop();
                            nodes.Push(new Node(tokens[i].Value, leftnode, rightnode));
                            break;
                        }
                }
            }
            return nodes.Pop();
        }
    }
}
