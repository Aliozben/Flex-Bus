using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace LinkedListBoyyzzz {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        public LINKEDList list_voyages = new LINKEDList(); // Linked list for all voyages

        bool isBuyingTicket; // is user on ticket interface or not ?
        private LINKEDList voyages_scanner() { //This function scans all texts file and add to linked list called "list_voyages"

            string path;
            DirectoryInfo d = new DirectoryInfo("texts");
            FileInfo[] files = d.GetFiles("*.txt");

            foreach(FileInfo item in files) {

                path = "texts/" + item.Name;
                List<string> list = File.ReadAllLines(path).ToList();
                int row_counter = 0;

                foreach(var item2 in list) {
                    if(row_counter % 2 == 0) {

                        object[] entries = item2.Split(',');
                        Voyages newVoyage = new Voyages();

                        newVoyage.number = Convert.ToInt32(entries[0]);
                        newVoyage.from = entries[1].ToString();
                        newVoyage.destination = entries[2].ToString();
                        newVoyage.date = DateTime.ParseExact(entries[3].ToString(), "dd-MM-yyyy", null);
                        newVoyage.time = Convert.ToDateTime(entries[4]).TimeOfDay;
                        newVoyage.seat_count = Convert.ToInt32(entries[5]);
                        newVoyage.ticket_price = Convert.ToInt32(entries[6]);
                        newVoyage.plate_number = entries[7].ToString();
                        newVoyage.driver = entries[8].ToString();

                        for(int i = 0; i < newVoyage.seat_count; i++) {
                            Seats newSeat = new Seats();
                            newSeat.number = i + 1;
                            newSeat.passanger_name = null;
                            newSeat.gender = null;
                            newSeat.status = null;
                            newVoyage.LINKED_seats.Add(newSeat);
                        }

                        list_voyages.Add(newVoyage);
                    }
                    else {
                        list_voyages[list_voyages.Count].LINKED_seats.Clear();
                        string[] entries = item2.Split(',');
                        for(int i = 0; i < list_voyages[list_voyages.Count].seat_count; i++) {

                            object[] specific_entries = entries[i].Split('-');
                            Seats newSeat = new Seats();
                            newSeat.number = i + 1;
                            newSeat.passanger_name = specific_entries[1].ToString();
                            newSeat.gender = specific_entries[2].ToString();
                            newSeat.status = specific_entries[3].ToString();
                            list_voyages[list_voyages.Count].LINKED_seats.Add(newSeat);
                        }

                    }

                    row_counter++;
                }

            }

            return list_voyages;

        }

        private void listbox_voyage_scanner() { // This function list all voyages in ListView called ListView_Voyage

            ListView_Voyage.Items.Clear();

            for(int i = 0; i < list_voyages.Count; i++) {
                string[] row = new string[] {

                list_voyages[i].number.ToString(),
                 list_voyages[i].from,
                    list_voyages[i].destination,
                 list_voyages[i].date.ToString("dd-MM-yyyy"),
                 list_voyages[i].time.ToString(@"hh\:mm"),
                   list_voyages[i].seat_count.ToString(),
                 list_voyages[i].ticket_price.ToString(),
                 list_voyages[i].plate_number,
                   list_voyages[i].driver

                };
                var lvi = new ListViewItem(row);
                lvi.Tag = list_voyages[i];
                ListView_Voyage.Items.Add(lvi);

            }
            Label_TotalVoyage.Text = list_voyages.Count.ToString();
        }
        private void Form1_Load(object sender, EventArgs e) {

            voyages_scanner();
        }

        private void Button_Save_Click(object sender, EventArgs e) { // Adds the voyage to linked list

            /// Find the highest number in voyages
            int highest_no = 0;
            if(list_voyages.Empty)
                highest_no = 1000; //If there is not any voyage 1000 will be the default voyage number
            else {
                for(int i = 0; i < list_voyages.Count; i++) {
                    if(list_voyages[i].number >= highest_no) {
                        highest_no = list_voyages[i].number + 1;
                    }
                }
            }
            ///

            // Is any box empty ?
            if(Box_From.Text == "" ||
                Box_Destination.Text == "" ||
                Box_SeatCount.Value <= 0 ||
                Box_TicketPrice.Value <= 0 ||
                Box_Driver.Text == "" ||
                Box_PlateNumber.Text == "") {

                MessageBox.Show("You need to fill all boxes.");
            }
            else {
                Voyages newVoyage = new Voyages();
                newVoyage.number = Convert.ToInt32(highest_no);
                newVoyage.from = Box_From.Text;
                newVoyage.destination = Box_Destination.Text;
                newVoyage.date = Box_Date.Value.Date;
                newVoyage.time = Box_Time.Value.TimeOfDay;
                newVoyage.seat_count = (int)Box_SeatCount.Value;
                newVoyage.ticket_price = (int)Box_TicketPrice.Value;
                newVoyage.plate_number = Box_PlateNumber.Text;
                newVoyage.driver = Box_Driver.Text;

                for(int i = 0; i < newVoyage.seat_count; i++) {
                    Seats newSeat = new Seats();
                    newSeat.number = i + 1;
                    newSeat.passanger_name = null;
                    newSeat.gender = null;
                    newSeat.status = "Available";
                    newVoyage.LINKED_seats.Add(newSeat);
                }

                list_voyages.Add(newVoyage);
                Box_Driver.Text = Box_From.Text = Box_Destination.Text = Box_PlateNumber.Text = "";
                Box_SeatCount.Value = Box_TicketPrice.Value = 0;

            }

        }

        private void Menu_AddVoyage_Click(object sender, EventArgs e) { // opens voyage adding interface
            isBuyingTicket = false;
            Panel_AddVoyage.Visible = true;
            Panel_showVoyage.Visible = Panel_showVoyage2.Visible = ListView_Voyage.Visible = Panel_TicketBuy.Visible = Panel_SoldReserved.Visible = false;

            foreach(var item in btns) { // Clearing bus seats
                item.Dispose();

            }
            btns.Clear();
            //
        }

        private void Menu_ShowVoyages_Click(object sender, EventArgs e) { // opens voyage listing interface
            isBuyingTicket = false;
            Panel_AddVoyage.Visible = false;
            Panel_showVoyage.Visible = Panel_showVoyage2.Visible = ListView_Voyage.Visible = true;
            listbox_voyage_scanner();
            Panel_TicketBuy.Visible = false;
            Panel_SoldReserved.Visible = false;

            foreach(var item in btns) { // Clearing bus seats
                item.Dispose();

            }
            btns.Clear();
            //
        }

        Voyages selectedItem;// selected  item in ListView
        List<Button> btns = new List<Button>(); // Bus seats
        private void ListView_Voyage_SelectedIndexChanged(object sender, EventArgs e) {

            if(ListView_Voyage.SelectedItems.Count == 0)
                return;
            selectedItem = (Voyages)ListView_Voyage.SelectedItems[0].Tag;

            if(!isBuyingTicket) {
                Button_VoyageEdit.Enabled = true;

                int soldtickets = 0;
                for(int i = 0; i < selectedItem.seat_count; i++) {
                    if(selectedItem.LINKED_seats[i].status.Equals("Sold"))
                        soldtickets++;

                }
                Label_Income.Text = soldtickets * selectedItem.ticket_price + "";
            }
            else {
                Panel_TicketBuy.Visible = false;
                creating_seatButtons();
            }
        }
        private void creating_seatButtons() { //Creating Seat Buttons

            foreach(var item in btns) {
                item.Dispose();

            }
            btns.Clear();
            int xPos = 20;
            int yPos = 535;
            for(int i = 0; i < selectedItem.seat_count; i++) {
                Button dynamicButton = new Button();
                btns.Add(dynamicButton);

            }
            for(int i = 0; i < selectedItem.seat_count; i++) {
                switch(i % 3) {
                    case 0: yPos = 450; break;
                    case 1: yPos = 525; break;
                    case 2: yPos = 580; break;
                }
                xPos = 20 + 55 * (i / 3);


                btns[i].Location = new Point(xPos, yPos);
                btns[i].Height = 50;
                btns[i].Width = 50;
                btns[i].Text = selectedItem.LINKED_seats[i].number.ToString();
                switch(selectedItem.LINKED_seats[i].status) {
                    case "Available": btns[i].BackColor = Color.Green; break;
                    case "Reserved": btns[i].BackColor = Color.RoyalBlue; break;
                    case "Sold": btns[i].BackColor = Color.Brown; break;
                }
                btns[i].Click += new EventHandler(DynamicButton_Click);
                Controls.Add(btns[i]);

            }
        }
        private void DynamicButton_Click(object sender, EventArgs e) { // all dynamic buttons click event

            Button btn = sender as Button;

            Box_TicketSeatNumber.Text = btn.Text;
            Label_TicketPrice.Text = "Ticket Price : " + selectedItem.ticket_price;

            // Is the seat sold, reserved or available
            // Green = Available
            // Brown = Sold
            // RoyalBlue = Reserved
            switch(btn.BackColor.Name) {
                case "Green": Panel_TicketBuy.Visible = true; break;
                case "Brown":
                    DialogResult dialog = new DialogResult();
                    dialog = MessageBox.Show("Do you want to cancel this ticket?", " ", MessageBoxButtons.YesNo);
                    if(dialog == DialogResult.Yes) {
                        list_voyages[list_voyages.indexOf(selectedItem.number)].LINKED_seats[Convert.ToInt32(Box_TicketSeatNumber.Text) - 1].passanger_name = "";
                        list_voyages[list_voyages.indexOf(selectedItem.number)].LINKED_seats[Convert.ToInt32(Box_TicketSeatNumber.Text) - 1].gender = "";

                        list_voyages[list_voyages.indexOf(selectedItem.number)].LINKED_seats[Convert.ToInt32(Box_TicketSeatNumber.Text) - 1].status = "Available"; break;

                    }
                    break;

                case "RoyalBlue":                    
                    dialog = MessageBox.Show("If you want to buy this ticket press 'Yes'.\r If you want to cancel your reservation press 'No'.\r" +
                        " If you want to return the previous menu press 'Cancel'", " ", MessageBoxButtons.YesNoCancel); switch(dialog) {

                        case DialogResult.Cancel:
                            break;

                        case DialogResult.Yes:
                            list_voyages[list_voyages.indexOf(selectedItem.number)].LINKED_seats[Convert.ToInt32(Box_TicketSeatNumber.Text) - 1].status = "Sold";
                            break;
                        case DialogResult.No:
                            list_voyages[list_voyages.indexOf(selectedItem.number)].LINKED_seats[Convert.ToInt32(Box_TicketSeatNumber.Text) - 1].passanger_name = "";
                            list_voyages[list_voyages.indexOf(selectedItem.number)].LINKED_seats[Convert.ToInt32(Box_TicketSeatNumber.Text) - 1].gender = "";

                            list_voyages[list_voyages.indexOf(selectedItem.number)].LINKED_seats[Convert.ToInt32(Box_TicketSeatNumber.Text) - 1].status = "Available"; break;

                    }
                    break;
            }
            creating_seatButtons();
        }

        private void Button_VoyageDelete_Click(object sender, EventArgs e) { // Deletes the selected voyage in listview
            if(selectedItem != null) {
                bool soldTicket = false;
                for(int i = 0; i < selectedItem.seat_count; i++) {
                    if(selectedItem.LINKED_seats[i].status != "Available") {
                        soldTicket = true;
                        MessageBox.Show("There is/are sold ticket(s) in this voyage.");
                        break;
                    }
                }
                if(!soldTicket)
                    list_voyages.Remove(list_voyages.indexOf(selectedItem.number));
            }

            listbox_voyage_scanner();
        }

        private void Button_Exit_Click(object sender, EventArgs e) { //It saves all the data to text files and then shuts down the the app. 

            string[] texts = Directory.GetFiles("texts");
            foreach(var item in texts) {
                File.Delete(item);
            }

            for(int x = 0; x < list_voyages.Count; x++) {
                string voyage = $"{list_voyages[x].number},{list_voyages[x].from},{list_voyages[x].destination},{list_voyages[x].date.ToString("dd-MM-yyyy")}," +
                               $"{list_voyages[x].time},{ list_voyages[x].seat_count},{ list_voyages[x].ticket_price},{ list_voyages[x].plate_number},{ list_voyages[x].driver}";

                string seat = "";

                for(int j = 0; j < list_voyages[x].seat_count; j++) {

                    seat += $"{list_voyages[x].LINKED_seats[j].number}-{list_voyages[x].LINKED_seats[j].passanger_name}-{list_voyages[x].LINKED_seats[j].gender}-" +
                        $"{list_voyages[x].LINKED_seats[j].status},";

                }

                StreamWriter write = new StreamWriter($"texts/{list_voyages[x].date.ToString("dd-MM-yyyy")}.txt", true);

                write.WriteLine(voyage);
                write.WriteLine(seat);
                write.Close();
            }


            Application.Exit();
        }

        private void Box_From_SelectedValueChanged(object sender, EventArgs e) { //  it deletes the same value from destination box
            Box_Destination.Enabled = true;
            Box_Destination.Items.Clear();
            foreach(var item in Box_From.Items) {

                Box_Destination.Items.Add(item);
            }
            Box_Destination.Items.Remove(Box_From.SelectedItem);
        }

        private void Button_VoyageEdit_Click(object sender, EventArgs e) {
            Button_VoyageDelete.Enabled = Button_VoyageEdit.Enabled = false;
            Panel_VoyageEdit.Visible = true;
        }

        private void Button_SaveVoyageEdit_Click(object sender, EventArgs e) {
            if(Box_NewDriver.Text == "") MessageBox.Show("You need to fill new driver.");
            else
                list_voyages[list_voyages.indexOf(selectedItem.number)].driver = Box_NewDriver.Text;
            listbox_voyage_scanner();
            Panel_VoyageEdit.Visible = false;
            Button_VoyageDelete.Enabled = Button_VoyageEdit.Enabled = true;
        }

        private void Button_CancelVoyageEdit_Click(object sender, EventArgs e) {
            Box_NewDriver.Text = "";
            Panel_VoyageEdit.Visible = false;
            Button_VoyageDelete.Enabled = Button_VoyageEdit.Enabled = true;

        }

        private void Button_ShowOutdateVoyages_Click(object sender, EventArgs e) {

            ListView_Voyage.Items.Clear();
            for(int i = 0; i < list_voyages.Count; i++) {
                string[] row = new string[] {

                list_voyages[i].number.ToString(),
                 list_voyages[i].from,
                    list_voyages[i].destination,
                 list_voyages[i].date.ToString("dd-MM-yyyy"),
                 list_voyages[i].time.ToString(@"hh\:mm"),
                   list_voyages[i].seat_count.ToString(),
                 list_voyages[i].ticket_price.ToString(),
                 list_voyages[i].plate_number,
                   list_voyages[i].driver

                };
                bool outdated = false;
                if(DateTime.Today.Year > list_voyages[i].date.Year)
                    outdated = true;

                else if(DateTime.Today.Year == list_voyages[i].date.Year) {

                    if(DateTime.Today.Month > list_voyages[i].date.Month)
                        outdated = true;

                    else if(DateTime.Today.Month == list_voyages[i].date.Month) {

                        if(DateTime.Today.Day > list_voyages[i].date.Day)
                            outdated = true;

                        else if(DateTime.Today.Day == list_voyages[i].date.Day) {

                            if(DateTime.Now.Hour > list_voyages[i].time.Hours)
                                outdated = true;

                            else if(DateTime.Now.Hour == list_voyages[i].time.Hours && DateTime.Now.Minute > list_voyages[i].time.Minutes)
                                outdated = true;
                        }
                    }

                }
                if(outdated) {
                    var lvi = new ListViewItem(row);
                    lvi.Tag = list_voyages[i];
                    ListView_Voyage.Items.Add(lvi);
                }

            }
        }

        private void Button_ShowAvailableVoyages_Click(object sender, EventArgs e) {

            ListView_Voyage.Items.Clear();
            for(int i = 0; i < list_voyages.Count; i++) {
                string[] row = new string[] {

                list_voyages[i].number.ToString(),
                 list_voyages[i].from,
                    list_voyages[i].destination,
                 list_voyages[i].date.ToString("dd-MM-yyyy"),
                 list_voyages[i].time.ToString(@"hh\:mm"),
                   list_voyages[i].seat_count.ToString(),
                 list_voyages[i].ticket_price.ToString(),
                 list_voyages[i].plate_number,
                   list_voyages[i].driver

                };
                bool avaiblabe = false;
                if(DateTime.Today.Year < list_voyages[i].date.Year)
                    avaiblabe = true;

                else if(DateTime.Today.Year == list_voyages[i].date.Year) {

                    if(DateTime.Today.Month < list_voyages[i].date.Month)
                        avaiblabe = true;

                    else if(DateTime.Today.Month == list_voyages[i].date.Month) {

                        if(DateTime.Today.Day < list_voyages[i].date.Day)
                            avaiblabe = true;

                        else if(DateTime.Today.Day == list_voyages[i].date.Day) {

                            if(DateTime.Now.Hour < list_voyages[i].time.Hours)
                                avaiblabe = true;

                            else if(DateTime.Now.Hour == list_voyages[i].time.Hours && DateTime.Now.Minute < list_voyages[i].time.Minutes)
                                avaiblabe = true;
                        }
                    }

                }
                if(avaiblabe) {
                    var lvi = new ListViewItem(row);
                    lvi.Tag = list_voyages[i];
                    ListView_Voyage.Items.Add(lvi);
                }

            }
        }

        private void Button_ShowAllVoyages_Click(object sender, EventArgs e) {
            listbox_voyage_scanner();
        }

        private void Menu_BuyTicket_Click(object sender, EventArgs e) {
            isBuyingTicket = true;
            Panel_AddVoyage.Visible = Panel_showVoyage.Visible = Panel_showVoyage2.Visible = false;
            ListView_Voyage.Visible = Panel_SoldReserved.Visible = true;

            ListView_Voyage.Items.Clear();
            for(int i = 0; i < list_voyages.Count; i++) {
                string[] row = new string[] {

                list_voyages[i].number.ToString(),
                 list_voyages[i].from,
                    list_voyages[i].destination,
                 list_voyages[i].date.ToString("dd-MM-yyyy"),
                 list_voyages[i].time.ToString(@"hh\:mm"),
                   list_voyages[i].seat_count.ToString(),
                 list_voyages[i].ticket_price.ToString(),
                 list_voyages[i].plate_number,
                   list_voyages[i].driver

                };
                bool avaiblabe = false;
                if(DateTime.Today.Year < list_voyages[i].date.Year)
                    avaiblabe = true;

                else if(DateTime.Today.Year == list_voyages[i].date.Year) {

                    if(DateTime.Today.Month < list_voyages[i].date.Month)
                        avaiblabe = true;

                    else if(DateTime.Today.Month == list_voyages[i].date.Month) {

                        if(DateTime.Today.Day < list_voyages[i].date.Day)
                            avaiblabe = true;

                        else if(DateTime.Today.Day == list_voyages[i].date.Day) {

                            if(DateTime.Now.Hour < list_voyages[i].time.Hours)
                                avaiblabe = true;

                            else if(DateTime.Now.Hour == list_voyages[i].time.Hours && DateTime.Now.Minute < list_voyages[i].time.Minutes)
                                avaiblabe = true;
                        }
                    }

                }
                if(avaiblabe) {
                    var lvi = new ListViewItem(row);
                    lvi.Tag = list_voyages[i];
                    ListView_Voyage.Items.Add(lvi);
                }

            }


        }

        private void Button_TicketBuy_Click(object sender, EventArgs e) {
            list_voyages[list_voyages.indexOf(selectedItem.number)].LINKED_seats[Convert.ToInt32(Box_TicketSeatNumber.Text) - 1].passanger_name = Box_TicketName.Text;
            list_voyages[list_voyages.indexOf(selectedItem.number)].LINKED_seats[Convert.ToInt32(Box_TicketSeatNumber.Text) - 1].gender = Box_TicketGender.SelectedItem.ToString();
            switch(Box_TicketBuy.SelectedItem) {
                case "Buy": list_voyages[list_voyages.indexOf(selectedItem.number)].LINKED_seats[Convert.ToInt32(Box_TicketSeatNumber.Text) - 1].status = "Sold"; break;
                case "Reserve": list_voyages[list_voyages.indexOf(selectedItem.number)].LINKED_seats[Convert.ToInt32(Box_TicketSeatNumber.Text) - 1].status = "Reserved"; break;
            }
            string x; // Is ticket sold or reserved?
            if(Box_TicketBuy.SelectedItem.ToString() == "Buy") x = "Sold";
            else x = "Reserved";
            MessageBox.Show($"Successfully {x}");
            Panel_TicketBuy.Visible = false;
            creating_seatButtons();
        }
    }
}
