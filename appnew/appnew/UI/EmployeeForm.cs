using appnew.DL;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace appnew
{
    public partial class EmployeeForm : Form
    {
        private Panel contentPanel;

        public EmployeeForm()
        {
            InitializeComponent();
            BuildUI();
        }

        private void BuildUI()
        {
            this.Text = "Skyline Hotel - Employee Dashboard";
            this.Size = new Size(900, 620);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.Font = new Font("Segoe UI", 10f);
            this.FormClosed += (s, e) => Application.Exit();

            // ── Top header bar ────────────────────
            Panel topBar = new Panel();
            topBar.Size = new Size(900, 65);
            topBar.Location = new Point(0, 0);
            topBar.BackColor = Color.FromArgb(28, 50, 68);

            Label lblTitle = new Label();
            lblTitle.Text = "Skyline Hotel  |  Employee Dashboard";
            lblTitle.Font = new Font("Segoe UI", 14f, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(20, 18);
            topBar.Controls.Add(lblTitle);

            // ── Left sidebar ──────────────────────
            Panel sidebar = new Panel();
            sidebar.Size = new Size(200, 555);
            sidebar.Location = new Point(0, 65);
            sidebar.BackColor = Color.FromArgb(38, 65, 85);

            string[] menuLabels = {
                "Reserve Room",
                "View Bookings",
                "Cancel Booking",
                "Available Rooms",
                "Edit Booking",
                "Calculate Bill",
                "Logout"
            };

            for (int i = 0; i < menuLabels.Length; i++)
            {
                Button btn = new Button();
                btn.Text = menuLabels[i];
                btn.Size = new Size(200, 48);
                btn.Location = new Point(0, i * 52 + 20);
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.ForeColor = Color.White;
                btn.Font = new Font("Segoe UI", 10f);
                btn.TextAlign = ContentAlignment.MiddleLeft;
                btn.Padding = new Padding(15, 0, 0, 0);
                btn.Cursor = Cursors.Hand;
                btn.BackColor = Color.FromArgb(38, 65, 85);
                btn.Tag = menuLabels[i];

                btn.MouseEnter += (s, e) => ((Button)s).BackColor = Color.FromArgb(32, 178, 140);
                btn.MouseLeave += (s, e) => ((Button)s).BackColor = Color.FromArgb(38, 65, 85);
                btn.Click += SidebarBtn_Click;
                sidebar.Controls.Add(btn);
            }

            // ── Content area ──────────────────────
            contentPanel = new Panel();
            contentPanel.Size = new Size(690, 555);
            contentPanel.Location = new Point(200, 65);
            contentPanel.BackColor = Color.FromArgb(245, 247, 250);
            contentPanel.Padding = new Padding(20);

            this.Controls.Add(topBar);
            this.Controls.Add(sidebar);
            this.Controls.Add(contentPanel);

            // Show welcome by default
            ShowWelcome();
        }

        private void SidebarBtn_Click(object sender, EventArgs e)
        {
            string tag = ((Button)sender).Tag.ToString();
            contentPanel.Controls.Clear();

            switch (tag)
            {
                case "Reserve Room": ShowReserveRoom(); break;
                case "View Bookings": ShowViewBookings(); break;
                case "Cancel Booking": ShowCancelBooking(); break;
                case "Available Rooms": ShowAvailableRooms(); break;
                case "Edit Booking": ShowEditBooking(); break;
                case "Calculate Bill": ShowCalculateBill(); break;
                case "Logout":
                    this.Hide();
                    new LoginForm().Show();
                    break;
            }
        }

        // ════════════════════════════════════════════
        //  WELCOME
        // ════════════════════════════════════════════
        private void ShowWelcome()
        {
            Label lbl = new Label();
            lbl.Text = "Welcome!\nSelect an option from the menu.";
            lbl.Font = new Font("Segoe UI", 14f);
            lbl.ForeColor = Color.FromArgb(80, 80, 80);
            lbl.AutoSize = true;
            lbl.Location = new Point(200, 220);
            contentPanel.Controls.Add(lbl);
        }

        // ════════════════════════════════════════════
        //  RESERVE ROOM
        // ════════════════════════════════════════════
        private void ShowReserveRoom()
        {
            AddSectionTitle("Reserve a Room");

            int y = 70;

            var numRoom = AddNumberInput("Room Number (0–34):", ref y);
            var txtGuest = AddTextInput("Guest Name:", ref y);
            var cmbType = AddComboInput("Room Type:", new[] { "Single", "Double", "Suite" }, ref y);
            var dtCheckIn = AddDateInput("Check In Date:", ref y);
            var dtCheckOut = AddDateInput("Check Out Date:", ref y);

            Button btnBook = MakeButton("Book Room", 32, 178, 140);
            btnBook.Location = new Point(30, y + 10);
            contentPanel.Controls.Add(btnBook);

            Label lblResult = new Label() { AutoSize = true, Location = new Point(30, y + 65), Font = new Font("Segoe UI", 10f) };
            contentPanel.Controls.Add(lblResult);

            btnBook.Click += (s, e) =>
            {
                int n = (int)numRoom.Value;
                string g = txtGuest.Text.Trim();
                string t = cmbType.SelectedItem?.ToString() ?? "";
                DateTime ci = dtCheckIn.Value.Date;
                DateTime co = dtCheckOut.Value.Date;

                if (string.IsNullOrEmpty(g) || string.IsNullOrEmpty(t))
                { lblResult.ForeColor = Color.Red; lblResult.Text = "Please fill all fields."; return; }

                if (AppServices.Hotel.getRoom(n).getIsBooked())
                { lblResult.ForeColor = Color.Red; lblResult.Text = "Room " + n + " is already booked."; return; }

                if (co < ci)
                { lblResult.ForeColor = Color.Red; lblResult.Text = "Check-out cannot be before check-in."; return; }

                int days = (int)(co - ci).TotalDays;
                if (days == 0) days = 1;

                AppServices.Hotel.getRoom(n).setGuestName(g);
                AppServices.Hotel.getRoom(n).setRoomType(t);
                AppServices.Hotel.getRoom(n).setStayDays(days);
                AppServices.Hotel.getRoom(n).setIsBooked(true);
                AppServices.Hotel.getRoom(n).setCheckInDate(ci.ToString("MM/dd/yyyy"));
                AppServices.Hotel.getRoom(n).setCheckOutDate(co.ToString("MM/dd/yyyy"));
                AppServices.SaveRooms();

                lblResult.ForeColor = Color.FromArgb(32, 178, 140);
                lblResult.Text = "Room " + n + " booked for " + days + " day(s).";
            };
        }

        // ════════════════════════════════════════════
        //  VIEW BOOKINGS
        // ════════════════════════════════════════════
        private void ShowViewBookings()
        {
            AddSectionTitle("All Bookings");

            DataGridView grid = new DataGridView();
            grid.Size = new Size(640, 430);
            grid.Location = new Point(20, 60);
            grid.ReadOnly = true;
            grid.AllowUserToAddRows = false;
            grid.BackgroundColor = Color.White;
            grid.BorderStyle = BorderStyle.None;
            grid.RowHeadersVisible = false;
            grid.Font = new Font("Segoe UI", 9.5f);
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(28, 50, 68);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            grid.EnableHeadersVisualStyles = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            grid.Columns.Add("Room", "Room #");
            grid.Columns.Add("Guest", "Guest Name");
            grid.Columns.Add("Type", "Room Type");
            grid.Columns.Add("CheckIn", "Check In");
            grid.Columns.Add("CheckOut", "Check Out");
            grid.Columns.Add("Days", "Days");

            bool any = false;
            for (int i = 0; i < 35; i++)
            {
                var r = AppServices.Hotel.getRoom(i);
                if (r.getIsBooked())
                {
                    grid.Rows.Add(i, r.getGuestName(), r.getRoomType(),
                                  r.getCheckInDate(), r.getCheckOutDate(), r.getStayDays());
                    any = true;
                }
            }

            if (!any)
            {
                Label lbl = new Label() { Text = "No bookings found.", AutoSize = true, Location = new Point(20, 70), ForeColor = Color.Gray, Font = new Font("Segoe UI", 12f) };
                contentPanel.Controls.Add(lbl);
            }
            else contentPanel.Controls.Add(grid);
        }

        // ════════════════════════════════════════════
        //  CANCEL BOOKING
        // ════════════════════════════════════════════
        private void ShowCancelBooking()
        {
            AddSectionTitle("Cancel Booking");
            int y = 70;
            var numRoom = AddNumberInput("Room Number (0–34):", ref y);

            Button btnCancel = MakeButton("Cancel Booking", 220, 53, 69);
            btnCancel.Location = new Point(30, y + 10);
            contentPanel.Controls.Add(btnCancel);

            Label lblResult = new Label() { AutoSize = true, Location = new Point(30, y + 65), Font = new Font("Segoe UI", 10f) };
            contentPanel.Controls.Add(lblResult);

            btnCancel.Click += (s, e) =>
            {
                int n = (int)numRoom.Value;
                if (!AppServices.Hotel.getRoom(n).getIsBooked())
                { lblResult.ForeColor = Color.Red; lblResult.Text = "Room " + n + " is not booked."; return; }

                AppServices.Hotel.getRoom(n).setGuestName("");
                AppServices.Hotel.getRoom(n).setRoomType("");
                AppServices.Hotel.getRoom(n).setStayDays(0);
                AppServices.Hotel.getRoom(n).setCheckInDate("");
                AppServices.Hotel.getRoom(n).setCheckOutDate("");
                AppServices.Hotel.getRoom(n).setIsBooked(false);
                AppServices.SaveRooms();

                lblResult.ForeColor = Color.FromArgb(32, 178, 140);
                lblResult.Text = "Booking for Room " + n + " cancelled.";
            };
        }

        // ════════════════════════════════════════════
        //  AVAILABLE ROOMS
        // ════════════════════════════════════════════
        private void ShowAvailableRooms()
        {
            AddSectionTitle("Available Rooms");

            FlowLayoutPanel flow = new FlowLayoutPanel();
            flow.Size = new Size(640, 450);
            flow.Location = new Point(20, 60);
            flow.AutoScroll = true;
            flow.BackColor = Color.FromArgb(245, 247, 250);

            for (int i = 0; i < 35; i++)
            {
                if (!AppServices.Hotel.getRoom(i).getIsBooked())
                {
                    Panel card = new Panel();
                    card.Size = new Size(120, 60);
                    card.BackColor = Color.FromArgb(32, 178, 140);
                    card.Margin = new Padding(6);

                    Label lbl = new Label();
                    lbl.Text = "Room " + i;
                    lbl.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
                    lbl.ForeColor = Color.White;
                    lbl.AutoSize = true;
                    lbl.Location = new Point(18, 18);
                    card.Controls.Add(lbl);

                    flow.Controls.Add(card);
                }
            }

            if (flow.Controls.Count == 0)
            {
                Label lbl = new Label() { Text = "No rooms available.", AutoSize = true, Location = new Point(20, 70), ForeColor = Color.Gray, Font = new Font("Segoe UI", 12f) };
                contentPanel.Controls.Add(lbl);
            }
            else contentPanel.Controls.Add(flow);
        }

        // ════════════════════════════════════════════
        //  EDIT BOOKING
        // ════════════════════════════════════════════
        private void ShowEditBooking()
        {
            AddSectionTitle("Edit Booking");
            int y = 70;
            var numRoom = AddNumberInput("Room Number (0–34):", ref y);
            var cmbType = AddComboInput("New Room Type:", new[] { "Single", "Double", "Suite" }, ref y);
            var dtCheckIn = AddDateInput("New Check In Date:", ref y);
            var dtCheckOut = AddDateInput("New Check Out Date:", ref y);

            Button btnEdit = MakeButton("Update Booking", 32, 178, 140);
            btnEdit.Location = new Point(30, y + 10);
            contentPanel.Controls.Add(btnEdit);

            Label lblResult = new Label() { AutoSize = true, Location = new Point(30, y + 65), Font = new Font("Segoe UI", 10f) };
            contentPanel.Controls.Add(lblResult);

            btnEdit.Click += (s, e) =>
            {
                int n = (int)numRoom.Value;
                string t = cmbType.SelectedItem?.ToString() ?? "";
                DateTime ci = dtCheckIn.Value.Date;
                DateTime co = dtCheckOut.Value.Date;

                if (!AppServices.Hotel.getRoom(n).getIsBooked())
                { lblResult.ForeColor = Color.Red; lblResult.Text = "Room " + n + " is not booked."; return; }

                if (string.IsNullOrEmpty(t))
                { lblResult.ForeColor = Color.Red; lblResult.Text = "Please select a room type."; return; }

                if (co < ci)
                { lblResult.ForeColor = Color.Red; lblResult.Text = "Check-out cannot be before check-in."; return; }

                int days = (int)(co - ci).TotalDays;
                if (days == 0) days = 1;

                AppServices.Hotel.getRoom(n).setRoomType(t);
                AppServices.Hotel.getRoom(n).setCheckInDate(ci.ToString("MM/dd/yyyy"));
                AppServices.Hotel.getRoom(n).setCheckOutDate(co.ToString("MM/dd/yyyy"));
                AppServices.Hotel.getRoom(n).setStayDays(days);
                AppServices.SaveRooms();

                lblResult.ForeColor = Color.FromArgb(32, 178, 140);
                lblResult.Text = "Booking updated. Days: " + days;
            };
        }

        // ════════════════════════════════════════════
        //  CALCULATE BILL
        // ════════════════════════════════════════════
        private void ShowCalculateBill()
        {
            AddSectionTitle("Calculate Bill");
            int y = 70;
            var numRoom = AddNumberInput("Room Number (0–34):", ref y);

            Button btnCalc = MakeButton("Calculate", 32, 178, 140);
            btnCalc.Location = new Point(30, y + 10);
            contentPanel.Controls.Add(btnCalc);

            Panel billCard = new Panel();
            billCard.Size = new Size(500, 260);
            billCard.Location = new Point(30, y + 70);
            billCard.BackColor = Color.White;
            billCard.BorderStyle = BorderStyle.FixedSingle;
            billCard.Visible = false;
            contentPanel.Controls.Add(billCard);

            btnCalc.Click += (s, e) =>
            {
                int n = (int)numRoom.Value;

                if (!AppServices.Hotel.getRoom(n).getIsBooked())
                {
                    MessageBox.Show("Room " + n + " is not booked.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string t = AppServices.Hotel.getRoom(n).getRoomType().ToUpper();
                int price = t == "SINGLE" ? 250 : t == "DOUBLE" ? 400 : t == "SUITE" ? 800 : 0;

                if (price == 0)
                { MessageBox.Show("Unknown room type.", "Error"); return; }

                int total = price * AppServices.Hotel.getRoom(n).getStayDays();

                billCard.Controls.Clear();
                billCard.Visible = true;

                string[] lines = {
                    "          BOOKING BILL",
                    "─────────────────────────────────────",
                    "Guest      : " + AppServices.Hotel.getRoom(n).getGuestName(),
                    "Room No.   : " + n,
                    "Room Type  : " + AppServices.Hotel.getRoom(n).getRoomType(),
                    "Check In   : " + AppServices.Hotel.getRoom(n).getCheckInDate(),
                    "Check Out  : " + AppServices.Hotel.getRoom(n).getCheckOutDate(),
                    "Days       : " + AppServices.Hotel.getRoom(n).getStayDays(),
                    "Rate/Night : $" + price,
                    "─────────────────────────────────────",
                    "TOTAL BILL : $" + total
                };

                for (int i = 0; i < lines.Length; i++)
                {
                    Label l = new Label();
                    l.Text = lines[i];
                    l.Font = i == lines.Length - 1
                        ? new Font("Courier New", 11f, FontStyle.Bold)
                        : new Font("Courier New", 10f);
                    l.ForeColor = i == lines.Length - 1 ? Color.FromArgb(32, 178, 140) : Color.FromArgb(40, 40, 40);
                    l.AutoSize = true;
                    l.Location = new Point(15, 10 + i * 22);
                    billCard.Controls.Add(l);
                }

                // Save to bill_history.txt
                StreamWriter f = new StreamWriter("bill_history.txt", append: true);
                f.WriteLine(AppServices.Hotel.getRoom(n).getGuestName() + "," +
                            AppServices.Hotel.getRoom(n).getRoomType() + "," +
                            AppServices.Hotel.getRoom(n).getCheckInDate() + "," +
                            AppServices.Hotel.getRoom(n).getCheckOutDate() + "," +
                            AppServices.Hotel.getRoom(n).getStayDays() + "," + total);
                f.Close();
            };
        }

        // ════════════════════════════════════════════
        //  UI HELPER METHODS
        // ════════════════════════════════════════════
        private void AddSectionTitle(string text)
        {
            Label lbl = new Label();
            lbl.Text = text;
            lbl.Font = new Font("Segoe UI", 15f, FontStyle.Bold);
            lbl.ForeColor = Color.FromArgb(28, 50, 68);
            lbl.AutoSize = true;
            lbl.Location = new Point(20, 20);
            contentPanel.Controls.Add(lbl);
        }

        private NumericUpDown AddNumberInput(string label, ref int y)
        {
            Label lbl = new Label() { Text = label, AutoSize = true, Location = new Point(30, y), ForeColor = Color.FromArgb(60, 60, 60) };
            NumericUpDown num = new NumericUpDown() { Minimum = 0, Maximum = 34, Size = new Size(200, 30), Location = new Point(30, y + 22), Font = new Font("Segoe UI", 11f) };
            contentPanel.Controls.Add(lbl);
            contentPanel.Controls.Add(num);
            y += 65;
            return num;
        }

        private TextBox AddTextInput(string label, ref int y)
        {
            Label lbl = new Label() { Text = label, AutoSize = true, Location = new Point(30, y), ForeColor = Color.FromArgb(60, 60, 60) };
            TextBox txt = new TextBox() { Size = new Size(300, 30), Location = new Point(30, y + 22), Font = new Font("Segoe UI", 11f), BorderStyle = BorderStyle.FixedSingle };
            contentPanel.Controls.Add(lbl);
            contentPanel.Controls.Add(txt);
            y += 65;
            return txt;
        }

        private ComboBox AddComboInput(string label, string[] items, ref int y)
        {
            Label lbl = new Label() { Text = label, AutoSize = true, Location = new Point(30, y), ForeColor = Color.FromArgb(60, 60, 60) };
            ComboBox cmb = new ComboBox() { Size = new Size(300, 30), Location = new Point(30, y + 22), Font = new Font("Segoe UI", 11f), DropDownStyle = ComboBoxStyle.DropDownList };
            cmb.Items.AddRange(items);
            contentPanel.Controls.Add(lbl);
            contentPanel.Controls.Add(cmb);
            y += 65;
            return cmb;
        }

        private DateTimePicker AddDateInput(string label, ref int y)
        {
            Label lbl = new Label() { Text = label, AutoSize = true, Location = new Point(30, y), ForeColor = Color.FromArgb(60, 60, 60) };
            DateTimePicker dtp = new DateTimePicker() { Size = new Size(300, 30), Location = new Point(30, y + 22), Font = new Font("Segoe UI", 11f), Format = DateTimePickerFormat.Short };
            contentPanel.Controls.Add(lbl);
            contentPanel.Controls.Add(dtp);
            y += 65;
            return dtp;
        }

        private Button MakeButton(string text, int r, int g, int b)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Size = new Size(200, 42);
            btn.BackColor = Color.FromArgb(r, g, b);
            btn.ForeColor = Color.White;
            btn.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor = Cursors.Hand;
            return btn;
        }
    }
}
