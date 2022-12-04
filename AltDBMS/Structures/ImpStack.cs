using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBMSPain.Structures
{
    public class ImpStack<T>
    {
        private List<T> list;
        public ImpStack() 
        { 
            list = new List<T>();
        }

        public void Push(T value)
        {
            list.Add(value);
        }

        public T Pop() 
        {
            if (list.Count == 0)
                throw new NotImplementedException("Stack Empty");           
            var a = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
            return a;
        }

        public T Peek()
        {
            return list[list.Count - 1];
        }

        public bool IsEmpty() 
        {      
            return list.Count == 0;
        }

        public int Count()
        {
            return list.Count;
        }

    }
}
