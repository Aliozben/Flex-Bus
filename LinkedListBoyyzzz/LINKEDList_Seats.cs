using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedListBoyyzzz {
    public class LINKEDList_Seats {
        private Node_Seats head;
        private int count;

        public LINKEDList_Seats() {
            this.head = null;
            this.count = 0;
        }

        public bool Empty {
            get { return this.count == 0; }
        }

        public int Count {
            get { return this.count; }
        }

        public Seats this[int index] {

            get { return this.Get(index); }
        }
        public Seats Add(int index, Seats o) {
            if(index < 0)
                throw new ArgumentOutOfRangeException("index:" + index);

            if(index > count)
                index = count;
            Node_Seats current = this.head;
            if(this.Empty || index == 0)
                this.head = new Node_Seats(o , this.head);
            else {
                for(int i = 0; i < index - 1; i++)
                    current = current.Next;

                current.Next = new Node_Seats(o, current.Next);
            }

            count++;
            return o;
        }
        public Seats Add(Seats o) {
            return this.Add(count, o);
        }

        public Seats Remove(int index) {

            if(index < 0)
                throw new ArgumentOutOfRangeException("index:" + index);

            if(this.Empty)
                return null;

            if(index >= count)
                index = count - 1;

            Node_Seats current = this.head;
            Seats result = null;

            if(index == 0) {
                result = current.Data;
                this.head = current.Next;
            }
            else {
                for(int i = 0; i < index - 1; i++) {
                    current = current.Next;
                }
                result = current.Next.Data;

                current.Next = current.Next.Next;
            }

            count--;
            return result;
        }

        public void Clear() {

            this.head = null;
            this.count = 0;

        }

        public int indexOf(Seats o) {
            Node_Seats current = this.head;
            for(int i = 0; i < this.count; i++) {
                if(current.Data.Equals(o))
                    return i;

                current = current.Next;
            }

            return -1;
        }

        public bool Contains(Seats o) {
            return this.indexOf(o) >= 0;
        }

        public Seats Get(int index) {
            if(index < 0)
                throw new ArgumentOutOfRangeException("index :" + index);

            if(this.Empty)
                return null;
            if(index >= this.count)
                index = count - 1;

            Node_Seats current = this.head;

            for(int i = 0; i < index; i++)
                current = current.Next;

            return current.Data;
        }

    }
}
