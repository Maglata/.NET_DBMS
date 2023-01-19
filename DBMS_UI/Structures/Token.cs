using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBMSPain.Structures
{
    public class Token
    {
        public Type type;
        public string Value = "";

        public enum Type 
        { 
            AND,
            OR,
            NOT,         
            OPENBR,
            CLOSEBR,
            CONDITION
        }
        public Token(Type type, string value)
        {
            this.type = type;
            Value = value;
        }

        public override string ToString() 
        {
            return type.ToString() + $" {Value}";
        }
    }
}
