using appnew.DL;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace appnew
{
    public partial class SignUpForm : Form
    {
        private TextBox txtUsername;
        private TextBox txtPassword;
        private ComboBox cmbRole;

        public SignUpForm()
        {
            InitializeComponent();
            BuildUI();
        }

        private void BuildUI()
        {
            this.Text = "Skyline Hotel - Sign Up";
            this.Size = new Size(420, 390);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 10f);

            // Header
            Panel header = new Panel();
            header.Size = new Size(420, 75);
            header.Location = new Point(0, 0);
            header.BackColor = Color.FromArgb(28, 50, 68);

            Label lblTitle = new Label();
            lblTitle.Text = "Create Account";
            lblTitle.Font = new Font("Segoe UI", 16f, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(110, 22);
            header.Controls.Add(lblTitle);

            // Username
            Label lblU = new Label() { Text = "Username", Location = new Point(40, 95), AutoSize = true, ForeColor = Color.FromArgb(60, 60, 60) };
            txtUsername = new TextBox() { Size = new Size(330, 32), Location = new Point(40, 118), Font = new Font("Segoe UI", 11f), BorderStyle = BorderStyle.FixedSingle };

            // Password
            Label lblP = new Label() { Text = "Password", Location = new Point(40, 162), AutoSize = true, ForeColor = Color.FromArgb(60, 60, 60) };
            txtPassword = new TextBox() { Size = new Size(330, 32), Location = new Point(40, 185), Font = new Font("Segoe UI", 11f), BorderStyle = BorderStyle.FixedSingle, PasswordChar = '*' };

            // Role
            Label lblR = new Label() { Text = "Role", Location = new Point(40, 229), AutoSize = true, ForeColor = Color.FromArgb(60, 60, 60) };
            cmbRole = new ComboBox();
            cmbRole.Items.AddRange(new string[] { "EMPLOYEE", "ADMIN" });
            cmbRole.Size = new Size(330, 32);
            cmbRole.Location = new Point(40, 252);
            cmbRole.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRole.Font = new Font("Segoe UI", 11f);

            // Register button
            Button btnReg = new Button();
            btnReg.Text = "Register";
            btnReg.Size = new Size(330, 44);
            btnReg.Location = new Point(40, 305);
            btnReg.BackColor = Color.FromArgb(32, 178, 140);
            btnReg.ForeColor = Color.White;
            btnReg.Font = new Font("Segoe UI", 11f, FontStyle.Bold);
            btnReg.FlatStyle = FlatStyle.Flat;
            btnReg.FlatAppearance.BorderSize = 0;
            btnReg.Cursor = Cursors.Hand;
            btnReg.Click += BtnRegister_Click;

            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                header, lblU, txtUsername, lblP, txtPassword, lblR, cmbRole, btnReg
            });
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            string u = txtUsername.Text.Trim();
            string p = txtPassword.Text.Trim();
            string r = cmbRole.SelectedItem?.ToString() ?? "";

            if (string.IsNullOrEmpty(u) || string.IsNullOrEmpty(p) || string.IsNullOrEmpty(r))
            {
                MessageBox.Show("Please fill all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (AppServices.UserExists(u, p, r))
            {
                MessageBox.Show("User already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            User newUser = r == "ADMIN"
                ? (User)new Admin(u, p)
                : new Employee(u, p);

            AppServices.Hotel.getUserManager().addUser(newUser);
            AppServices.SaveUsers();

            MessageBox.Show("Account created! You can now log in.", "Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}
