using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnDBMS.Structures
{
    public class RowElement
    {
        public ObjectLinkedList<ColElement> Values;
        public RowElement(ObjectLinkedList<ColElement> values)
        {
            this.Values = values;
        }
    }
}
