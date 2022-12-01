using OwnDBMS.Structures;
using OwnDBMS.Utilities;
using System.ComponentModel.Design;
using System.Data;

namespace OwnDBMS
{
    internal class Program
    { 
        static void Main(string[] args)
        {
            InputParser parser = new InputParser();
            parser.RUN();

            //var list = new ObjectLinkedList<string>();
            //list.AddLast("A");
            //list.AddLast("A");
            //list.AddLast("A");
            //list.AddLast("A");

            //Console.WriteLine(list.Count);

            //list.Remove("A");
            //Console.WriteLine(list.Count);
        }       
    }
}