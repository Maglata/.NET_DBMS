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
        public List<ColElement> Cols = new List<ColElement>();
        public List<RowElement> Rows = new List<RowElement>();

        public Table(List<ColElement> cols, string name)
        {
            Name = name;
            Cols = cols;
        }
        public Table()
        {

        }

        public string[] GetColNames()
        {
            string[] colnames = new string[Cols.Count];
            for (int i = 0; i < Cols.Count; i++)
            {
                colnames[i] = Cols[i].GetName();
            }
            return colnames;
        }
    }
}
