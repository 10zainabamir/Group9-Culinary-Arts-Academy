using System;
using System.Drawing;
using System.Windows.Forms;

namespace appnew
{
    public partial class LoginForm : Form
    {
        // ── Controls ──────────────────────────────
        private TextBox txtUsername;
        private TextBox txtPassword;

        public LoginForm()
        {
            InitializeComponent();
            AppServices.Init();
            BuildUI();
        }

        private void BuildUI()
        {
            this.Text = "Skyline Hotel - Login";
            this.Size = new Size(460, 420);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 10f);

            // ── Dark header ──────────────────────
            Panel header = new Panel();
            header.Size = new Size(460, 130);
            header.Location = new Point(0, 0);
            header.BackColor = Color.FromArgb(28, 50, 68);

            Label lblHotel = new Label();
            lblHotel.Text = "Skyline Hotel";
            lblHotel.Font = new Font("Segoe UI", 22f, FontStyle.Bold);
            lblHotel.ForeColor = Color.White;
            lblHotel.AutoSize = true;
            lblHotel.Location = new Point(110, 35);

            Label lblSub = new Label();
            lblSub.Text = "HOTEL MANAGEMENT SYSTEM";
            lblSub.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            lblSub.ForeColor = Color.FromArgb(64, 200, 170);
            lblSub.AutoSize = true;
            lblSub.Location = new Point(118, 90);

            header.Controls.Add(lblHotel);
            header.Controls.Add(lblSub);

            // ── Username ─────────────────────────
            Label lblUser = new Label();
            lblUser.Text = "Username";
            lblUser.ForeColor = Color.FromArgb(60, 60, 60);
            lblUser.Location = new Point(50, 155);
            lblUser.AutoSize = true;

            txtUsername = new TextBox();
            txtUsername.Size = new Size(360, 32);
            txtUsername.Location = new Point(50, 178);
            txtUsername.Font = new Font("Segoe UI", 11f);
            txtUsername.BorderStyle = BorderStyle.FixedSingle;

            // ── Password ─────────────────────────
            Label lblPass = new Label();
            lblPass.Text = "Password";
            lblPass.ForeColor = Color.FromArgb(60, 60, 60);
            lblPass.Location = new Point(50, 222);
            lblPass.AutoSize = true;

            txtPassword = new TextBox();
            txtPassword.Size = new Size(360, 32);
            txtPassword.Location = new Point(50, 245);
            txtPassword.Font = new Font("Segoe UI", 11f);
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.PasswordChar = '*';

            // ── Sign In button ───────────────────
            Button btnSignIn = new Button();
            btnSignIn.Text = "Sign In";
            btnSignIn.Size = new Size(170, 44);
            btnSignIn.Location = new Point(50, 305);
            btnSignIn.BackColor = Color.FromArgb(32, 178, 140);
            btnSignIn.ForeColor = Color.White;
            btnSignIn.Font = new Font("Segoe UI", 11f, FontStyle.Bold);
            btnSignIn.FlatStyle = FlatStyle.Flat;
            btnSignIn.FlatAppearance.BorderSize = 0;
            btnSignIn.Cursor = Cursors.Hand;
            btnSignIn.Click += BtnSignIn_Click;

            // ── Sign Up button ───────────────────
            Button btnSignUp = new Button();
            btnSignUp.Text = "Sign Up";
            btnSignUp.Size = new Size(170, 44);
            btnSignUp.Location = new Point(240, 305);
            btnSignUp.BackColor = Color.FromArgb(220, 220, 220);
            btnSignUp.ForeColor = Color.FromArgb(60, 60, 60);
            btnSignUp.Font = new Font("Segoe UI", 11f, FontStyle.Bold);
            btnSignUp.FlatStyle = FlatStyle.Flat;
            btnSignUp.FlatAppearance.BorderSize = 0;
            btnSignUp.Cursor = Cursors.Hand;
            btnSignUp.Click += (s, e) => new SignUpForm().ShowDialog();

            this.Controls.Add(header);
            this.Controls.Add(lblUser);
            this.Controls.Add(txtUsername);
            this.Controls.Add(lblPass);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnSignIn);
            this.Controls.Add(btnSignUp);
        }

        private void BtnSignIn_Click(object sender, EventArgs e)
        {
            string u = txtUsername.Text.Trim();
            string p = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(u) || string.IsNullOrEmpty(p))
            {
                MessageBox.Show("Please enter username and password.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string role = AppServices.CheckLogin(u, p);

            if (role == "EMPLOYEE")
            {
                new EmployeeForm().Show();
                this.Hide();
            }
            else if (role == "ADMIN")
            {
                new AdminForm().Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Wrong username or password.", "Login Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

