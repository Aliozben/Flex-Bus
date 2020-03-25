using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedListBoyyzzz {
    public class Node_Seats {

        private Seats data;
        private Node_Seats next;

        public Node_Seats(Seats data, Node_Seats next) {
            this.data = data;
            this.next = next;
        }

        public Seats Data {
            get { return this.data; }
            set { this.data = value; }
        }
        public Node_Seats Next {
            get { return this.next; }
            set { this.next = value; }
        }
    }
}
