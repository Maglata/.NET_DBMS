using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnDBMS.Structures
{
    public class RowElement
    {
        public ImpLinkedList<ColElement> Values;
        public RowElement(ImpLinkedList<ColElement> values)
        {
            this.Values = values;
        }
    }
}
