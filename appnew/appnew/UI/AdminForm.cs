using appnew.DL;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace appnew
{
    public partial class AdminForm : Form
    {
        private Panel contentPanel;

        public AdminForm()
        {
            InitializeComponent();
            BuildUI();
        }

        private void BuildUI()
        {
            this.Text = "Skyline Hotel - Admin Dashboard";
            this.Size = new Size(900, 620);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.Font = new Font("Segoe UI", 10f);
            this.FormClosed += (s, e) => Application.Exit();

            // ── Top header bar ─────────────────────
            Panel topBar = new Panel();
            topBar.Size = new Size(900, 65);
            topBar.Location = new Point(0, 0);
            topBar.BackColor = Color.FromArgb(28, 50, 68);

            Label lblTitle = new Label();
            lblTitle.Text = "Skyline Hotel  |  Admin Dashboard";
            lblTitle.Font = new Font("Segoe UI", 14f, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(20, 18);
            topBar.Controls.Add(lblTitle);

            // ── Left sidebar ───────────────────────
            Panel sidebar = new Panel();
            sidebar.Size = new Size(200, 555);
            sidebar.Location = new Point(0, 65);
            sidebar.BackColor = Color.FromArgb(38, 65, 85);

            string[] menuLabels = { "Add User", "View Bookings", "View All Users", "Remove User", "Logout" };

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

            // ── Content area ───────────────────────
            contentPanel = new Panel();
            contentPanel.Size = new Size(690, 555);
            contentPanel.Location = new Point(200, 65);
            contentPanel.BackColor = Color.FromArgb(245, 247, 250);
            contentPanel.Padding = new Padding(20);

            this.Controls.Add(topBar);
            this.Controls.Add(sidebar);
            this.Controls.Add(contentPanel);

            ShowWelcome();
        }

        private void SidebarBtn_Click(object sender, EventArgs e)
        {
            string tag = ((Button)sender).Tag.ToString();
            contentPanel.Controls.Clear();

            switch (tag)
            {
                case "Add User": ShowAddUser(); break;
                case "View Bookings": ShowViewBookings(); break;
                case "View All Users": ShowViewUsers(); break;
                case "Remove User": ShowRemoveUser(); break;
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
            lbl.Text = "Welcome, Admin!\nSelect an option from the menu.";
            lbl.Font = new Font("Segoe UI", 14f);
            lbl.ForeColor = Color.FromArgb(80, 80, 80);
            lbl.AutoSize = true;
            lbl.Location = new Point(200, 220);
            contentPanel.Controls.Add(lbl);
        }

        // ════════════════════════════════════════════
        //  ADD USER
        // ════════════════════════════════════════════
        private void ShowAddUser()
        {
            AddSectionTitle("Add New User");
            int y = 70;

            var txtU = AddTextInput("Username:", ref y);
            var txtP = AddTextInput("Password:", ref y, isPassword: true);
            var cmbRole = AddComboInput("Role:", new[] { "EMPLOYEE", "ADMIN" }, ref y);

            Button btnAdd = MakeButton("Add User", 32, 178, 140);
            btnAdd.Location = new Point(30, y + 10);
            contentPanel.Controls.Add(btnAdd);

            Label lblResult = new Label() { AutoSize = true, Location = new Point(30, y + 65), Font = new Font("Segoe UI", 10f) };
            contentPanel.Controls.Add(lblResult);

            btnAdd.Click += (s, e) =>
            {
                string u = txtU.Text.Trim();
                string p = txtP.Text.Trim();
                string r = cmbRole.SelectedItem?.ToString() ?? "";

                if (string.IsNullOrEmpty(u) || string.IsNullOrEmpty(p) || string.IsNullOrEmpty(r))
                { lblResult.ForeColor = Color.Red; lblResult.Text = "Please fill all fields."; return; }

                if (AppServices.UserExists(u, p, r))
                { lblResult.ForeColor = Color.Red; lblResult.Text = "User already exists."; return; }

                User newUser = r == "ADMIN"
                    ? (User)new Admin(u, p)
                    : new Employee(u, p);

                AppServices.Hotel.getUserManager().addUser(newUser);
                AppServices.SaveUsers();

                lblResult.ForeColor = Color.FromArgb(32, 178, 140);
                lblResult.Text = "User '" + u + "' added successfully.";
                txtU.Clear(); txtP.Clear(); cmbRole.SelectedIndex = -1;
            };
        }

        // ════════════════════════════════════════════
        //  VIEW BOOKINGS
        // ════════════════════════════════════════════
        private void ShowViewBookings()
        {
            AddSectionTitle("All Bookings");

            DataGridView grid = MakeGrid(new[] { "Room #", "Guest Name", "Room Type", "Check In", "Check Out", "Days" });

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
                contentPanel.Controls.Add(new Label() { Text = "No bookings found.", AutoSize = true, Location = new Point(20, 70), ForeColor = Color.Gray, Font = new Font("Segoe UI", 12f) });
            else
                contentPanel.Controls.Add(grid);
        }

        // ════════════════════════════════════════════
        //  VIEW ALL USERS
        // ════════════════════════════════════════════
        private void ShowViewUsers()
        {
            AddSectionTitle("All Users");

            DataGridView grid = MakeGrid(new[] { "#", "Username", "Role" });

            UserManager um = AppServices.Hotel.getUserManager();
            if (um.getCount() == 0)
            {
                contentPanel.Controls.Add(new Label() { Text = "No users found.", AutoSize = true, Location = new Point(20, 70), ForeColor = Color.Gray, Font = new Font("Segoe UI", 12f) });
                return;
            }

            for (int i = 0; i < um.getCount(); i++)
                grid.Rows.Add(i + 1, um.getUser(i).getUsername(), um.getUser(i).getRole());

            contentPanel.Controls.Add(grid);
        }

        // ════════════════════════════════════════════
        //  REMOVE USER
        // ════════════════════════════════════════════
        private void ShowRemoveUser()
        {
            AddSectionTitle("Remove User");
            int y = 70;

            var txtU = AddTextInput("Username:", ref y);
            var txtP = AddTextInput("Password:", ref y, isPassword: true);
            var cmbRole = AddComboInput("Role:", new[] { "EMPLOYEE", "ADMIN" }, ref y);

            Button btnRemove = MakeButton("Remove User", 220, 53, 69);
            btnRemove.Location = new Point(30, y + 10);
            contentPanel.Controls.Add(btnRemove);

            Label lblResult = new Label() { AutoSize = true, Location = new Point(30, y + 65), Font = new Font("Segoe UI", 10f) };
            contentPanel.Controls.Add(lblResult);

            btnRemove.Click += (s, e) =>
            {
                string u = txtU.Text.Trim();
                string p = txtP.Text.Trim();
                string r = cmbRole.SelectedItem?.ToString() ?? "";

                if (string.IsNullOrEmpty(u) || string.IsNullOrEmpty(p) || string.IsNullOrEmpty(r))
                { lblResult.ForeColor = Color.Red; lblResult.Text = "Please fill all fields."; return; }

                UserManager um2 = AppServices.Hotel.getUserManager();
                bool found = false;
                for (int i = 0; i < um2.getCount(); i++)
                {
                    if (um2.getUser(i).getUsername() == u &&
                        um2.getUser(i).getPassword() == p &&
                        um2.getUser(i).getRole() == r)
                    {
                        um2.removeUser(i);
                        AppServices.SaveUsers();
                        lblResult.ForeColor = Color.FromArgb(32, 178, 140);
                        lblResult.Text = "User '" + u + "' removed.";
                        found = true;
                        break;
                    }
                }
                if (!found)
                { lblResult.ForeColor = Color.Red; lblResult.Text = "User not found."; }
            };
        }

        // ════════════════════════════════════════════
        //  UI HELPERS
        // ════════════════════════════════════════════
        private void AddSectionTitle(string text)
        {
            Label lbl = new Label() { Text = text, Font = new Font("Segoe UI", 15f, FontStyle.Bold), ForeColor = Color.FromArgb(28, 50, 68), AutoSize = true, Location = new Point(20, 20) };
            contentPanel.Controls.Add(lbl);
        }

        private TextBox AddTextInput(string label, ref int y, bool isPassword = false)
        {
            Label lbl = new Label() { Text = label, AutoSize = true, Location = new Point(30, y), ForeColor = Color.FromArgb(60, 60, 60) };
            TextBox txt = new TextBox() { Size = new Size(300, 30), Location = new Point(30, y + 22), Font = new Font("Segoe UI", 11f), BorderStyle = BorderStyle.FixedSingle };
            if (isPassword) txt.PasswordChar = '*';
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

        private DataGridView MakeGrid(string[] columns)
        {
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
            foreach (string col in columns) grid.Columns.Add(col, col);
            return grid;
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