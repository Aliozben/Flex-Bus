using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedListBoyyzzz
{
    public class Node
    {
        private Voyages data;
        private Node next;

        public Node(Voyages data,Node next)
        {
            this.data = data;
            this.next = next; 
        }

        public Voyages Data
        {
            get { return this.data; }
            set { this.data = value; }
        }
        public Node Next
        {
            get { return this.next; }
            set { this.next = value; }
        }
    }
}
