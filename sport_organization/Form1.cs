using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sport_organization
{
    public partial class Form1 : Form
    {
        DbConnection dbcon = new DbConnection();
        public Form1()
        {
            InitializeComponent();
        }
        public Form1(registrationForm registrationForm)
        {
            InitializeComponent();
        }
        public Form1(mainForm mainform)
        {
            InitializeComponent();
        }
        public Form1(newForm newform)
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Form registrationForm = new registrationForm(this);
            registrationForm.Show();
            this.Hide();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            if (dbcon.CheckConnection())
                label6.Text = "Good";
            else label6.Text = "Error";
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (dbcon.CheckConnection())
                label6.Text = "Подключено";
            else label6.Text = "Ошибка";
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Заполните все поля");
            }
            else
            {
                string login = textBox1.Text;
                string password = textBox2.Text;

                try
                {
                    string role = dbcon.autorization(login, password);

                    if (role == null)
                    {
                        MessageBox.Show("Ошибка в логине или пароле");
                    }
                    else if (role == "админ")
                    {
                        Form mainform = new mainForm(this);
                        mainform.Show();
                        this.Hide();
                    }
                    else if (role == "спортсмен")
                    {
                        Form newform = new newForm(this, login);
                        newform.Show();
                        this.Hide();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
        }
    }
}
