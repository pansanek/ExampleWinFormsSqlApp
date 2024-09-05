using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExampleWinFormsSqlApp
{
    public partial class RegForm : Form
    {
        public RegForm()
        {
            InitializeComponent();

            nameField.Text = "Введите имя";
            nameField.ForeColor = Color.Gray;
            surnameField.Text = "Введите фамилию";
            surnameField.ForeColor = Color.Gray;
            this.passField.AutoSize = false;
            this.passField.Size = new Size(
                this.passField.Size.Width,
                this.loginField.Size.Height
                );
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void closeButton_MouseEnter(object sender, EventArgs e)
        {
            closeButton.ForeColor = Color.FromArgb(131, 197, 190);
        }

        private void closeButton_MouseLeave(object sender, EventArgs e)
        {
            closeButton.ForeColor = Color.FromArgb(237, 246, 249);
        }

        Point lastPoint;
        private void MainPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void MainPanel_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            String loginUser = this.loginField.Text;
            String passUser = this.passField.Text;

            DB db = new DB();

            DataTable dt = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM `users` WHERE `login` = @uL AND `pass` = @uP", db.getConnection());
            command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = loginUser;
            command.Parameters.Add("@uP", MySqlDbType.VarChar).Value = passUser;

            adapter.SelectCommand = command;
            adapter.Fill(dt);

            if (dt.Rows.Count > 0) MessageBox.Show("Success!");
            else MessageBox.Show("Login failed!");
        }

        private void nameField_Enter(object sender, EventArgs e)
        {
            if (nameField.Text == "Введите имя") {
                nameField.Text = "";
                     nameField.ForeColor = Color.Black ;
            }
           
        }

        private void nameField_Leave(object sender, EventArgs e)
        {
            if (nameField.Text == "")
            {
                nameField.Text = "Введите имя";
                nameField.ForeColor = Color.Gray;
            }
        }

        private void surnameField_Enter(object sender, EventArgs e)
        {
            if (surnameField.Text == "Введите фамилию")
            {
                surnameField.Text = "";
                surnameField.ForeColor = Color.Black;
            }

        }

        private void surnameField_Leave(object sender, EventArgs e)
        {
            if (surnameField.Text == "")
            {
                surnameField.Text = "Введите фамилию";
                surnameField.ForeColor = Color.Gray;
            }
        }

        private void buttonReg_Click(object sender, EventArgs e)
        {
            if(nameField.Text== "Введите имя")
            {
                MessageBox.Show("Введите имя");
                return;
            }
            if (surnameField.Text == "Введите фамилию")
            {
                MessageBox.Show("Введите фамилию");
                return;
            }
            if (loginField.Text == "")
            {
                MessageBox.Show("Введите логин");
                return;
            }
            if (passField.Text == "")
            {
                MessageBox.Show("Введите пароль");
                return;
            }

            if(doesUserExist())
            {
                MessageBox.Show("Пользователь с таким именем уже существует");
                return;
            }
            DB db = new DB();

            MySqlCommand command = new MySqlCommand("INSERT INTO `users` (`login`, `pass`, `name`, `surname`) VALUES (@l, @p, @u, @s);", db.getConnection());
            command.Parameters.Add("@l", MySqlDbType.VarChar).Value = loginField.Text;
            command.Parameters.Add("@p", MySqlDbType.VarChar).Value = passField.Text;
            command.Parameters.Add("@u", MySqlDbType.VarChar).Value = nameField.Text;
            command.Parameters.Add("@s", MySqlDbType.VarChar).Value = surnameField.Text;

            db.openConnection();

            if (command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Регистрация прошла успешно!");
            }
            else
            {
                MessageBox.Show("Ошибка!");
            }
            db.closeConnection();
        }

        public Boolean doesUserExist()
        {
            DB db = new DB();

            DataTable dt = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM `users` WHERE `login` = @uL", db.getConnection());
            command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = loginField.Text;

            adapter.SelectCommand = command;
            adapter.Fill(dt);

            if (dt.Rows.Count > 0) return true;
            else return false;
        }

        private void loginLabel_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
        }
    }
}

