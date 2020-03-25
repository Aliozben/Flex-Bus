using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedListBoyyzzz
{
    public class Voyages
    {
        public int number { get; set; }
        public string from { get; set; }
        public string destination { get; set; }
        public DateTime date { get; set; }
        public TimeSpan time { get; set; }
        public int seat_count { get; set; }
       
        // public int soldTicket_count { get; set; }
        public int ticket_price { get; set; }
        public string plate_number { get; set; }
        public string driver { get; set; }

        public LINKEDList_Seats LINKED_seats=new LINKEDList_Seats();
        

        
    }
}
