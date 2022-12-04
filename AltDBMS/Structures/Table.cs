using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnDBMS.Structures
{
    public class Table
    {
        public string Name;
        public ImpLinkedList<ColElement> Cols = new ImpLinkedList<ColElement>();
        public ImpLinkedList<RowElement> Rows = new ImpLinkedList<RowElement>();

        public Table(ImpLinkedList<ColElement> cols, string name)
        {
            Name = name;
            Cols = cols;
        }

        public string[] GetColNames()
        {
            string[] colnames = new string[Cols.Count];          
            var current = Cols.First();
            int i = 0;
            while (current != null)
            {
                colnames[i] = current.Value.GetName();               
                current = current.NextNode;
                i++;
            }
            return colnames;
        }
    }
}
