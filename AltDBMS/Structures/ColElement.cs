using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnDBMS.Structures
{
    public class ColElement
    {
        private string Name;
        public object Data;
        public object DefaultValue;
        private Type DataType;

        public ColElement(string name, Type datatype, object data = null)
        {
            Name = name;       
            DataType = datatype;
            Data = data;

            if (data == null)
            {
                Data = DefaultValue;
            }       
        }

        public void SetDefaultData(object data) 
        { 
            this.DefaultValue = data;
        }
        public string GetName() { return Name; }
        public Type GetType() { return DataType; }
    }
}
