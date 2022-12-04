using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OwnDBMS.Structures
{
    public class ImpLinkedList<T>
    {
        public class Node
        {
            public T Value { get; set; }
            public Node NextNode { get; set; }
            public Node PrevNode { get; set; }
        }

        private Node head, tail;
        private int count;

        public ImpLinkedList()
        {
            head = tail = null;
        }

        public Node ElementAt(int index)
        {
            int counter = 0;

            var current = First();

            while (counter != index)
            {
                current = current.NextNode;
                counter++;
            }
            return current;
        }

        public Node AddFirst(T value)
        {
            var newNode = new Node
            {
                Value = value,
                PrevNode = null,
                NextNode = head
            };

            if (head != null)
                head.PrevNode = newNode;
            else
                tail = newNode;

            head = newNode;

            count++;

            return newNode;
        }

        public Node AddLast(T value)
        {
            var newNode = new Node
            {
                Value = value,
                NextNode = null,
                PrevNode = tail
            };

            if (tail != null)
                tail.NextNode = newNode;
            else
                head = newNode;

            tail = newNode;

            count++;

            return newNode;
        }

        public Node First() { return head; }

        public Node AddBefore(Node next, T value)
        {
            if (next == null && head != null)
                throw new ArgumentNullException("next");

            var newNode = new Node
            {
                Value = value,
                NextNode = next,
                PrevNode = next?.PrevNode
            };

            if(head == null)
            {
                head = tail = newNode;
                return newNode;
            }

            if (next.PrevNode != null)
                next.PrevNode.NextNode = newNode;
            else
                head = newNode;

            next.PrevNode = newNode;
            count++;

            return newNode;

        }

        public void Remove(Node node)
        {
            if (node.NextNode != null)
                node.NextNode.PrevNode = node.PrevNode;
            else
                tail = node.PrevNode;

            if (node.PrevNode != null)
                node.PrevNode.NextNode = node.NextNode;

            else
                head = node.NextNode;

            count--;
        }

        public void Remove(T value)
        {
            Node node = Find(value);

            if (node.NextNode != null)
                node.NextNode.PrevNode = node.PrevNode;
            else
                tail = node.PrevNode;

            if (node.PrevNode != null)
                node.PrevNode.NextNode = node.NextNode;

            else
                head = node.NextNode;

            count--;
        }

        public Node? Find(T value)
        {
            Node node = head;
            if (node != null)
            {
                if (value != null)
                {
                    do
                    {
                        if (node.Value.Equals(value))
                        {
                            return node;
                        }
                        node = node.NextNode;
                    } while (node != head);
                }
                else
                {
                    do
                    {
                        if (node.Value == null)
                        {
                            return node;
                        }
                        node = node.NextNode;
                    } while (node != head);
                }
            }
            return null;
        }

        public int Count
        {
            get { return count; }
        }

        public Node AddAfter(Node node, T value)
        {
            // protects from null reference exception if node is null
            if (node == null && tail != null)
                    throw new ArgumentNullException("node");

            Node newNode = new Node
            {
                Value = value,
                NextNode = node?.NextNode,
                PrevNode = node
            };
            if (tail == null)
            {
                head = tail = newNode;
                return newNode;
            }

            if (node.NextNode != null)
                node.NextNode.PrevNode = newNode;
            else
            {
                tail = newNode;
            }
            tail.NextNode = newNode;
            count++;

            return newNode;
        }
    }
}
