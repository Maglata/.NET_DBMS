using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBMSPain.Structures
{
    public class Node
    {
        private string Value = "";
        private Node? Left;
        private Node? Right;

        public Node(string value, Node left, Node right)
        {
            Value = value;
            Left = left;
            Right = right;
        }

        public Node(string value)
        {
            Value = value;
            Left = null;
            Right = null;
        }

        public Node(string value, Node left)
        {
            Value = value;
            Left = left;
            Right = null;
        }

        public Node? GetLeft()
        {
            return Left;
        }

        public void SetLeft(Node node)
        {
            Left = node;
        }

        public Node? GetRight()
        {
            return Right;
        }

        public void SetRight(Node node)
        {
            Right = node;
        }

        public string GetValue()
        {
            return Value;
        }

        public void SetValue(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return $"{Value.ToString()} {Left.Value.ToString()} {Right.Value.ToString()}";
        }
    }
 
}
