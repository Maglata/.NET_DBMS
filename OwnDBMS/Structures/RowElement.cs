using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnDBMS.Structures
{
    public class RowElement
    {
        public List<ColElement> Values;
        // Can it be used?
        private Guid ID;
        public RowElement(List<ColElement> values)
        {
            this.Values = values;
            ID = Guid.NewGuid();
        }
    }
}
