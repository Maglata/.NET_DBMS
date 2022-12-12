﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
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
        public static bool Contains(string[] input, string item)
        {       
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == item)
                    return true;
            }
            return false;
        }
        public void Sort<T>(T[] arr) where T : IComparable<T>
        {
            for (int i = 0; i < arr.Length - 1; i++)
            {
                for (int j = i + 1; j < arr.Length; j++)
                {
                    if (arr[i].CompareTo(arr[j]) > 0)
                    {
                        T temp = arr[i];
                        arr[i] = arr[j];
                        arr[j] = temp;
                    }
                }
            }
        }

        public static bool Contains(ImpLinkedList<int> input, int item)
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
