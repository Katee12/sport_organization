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
    public partial class registrationForm : Form
    {
        DbConnection dbcon = new DbConnection();
        public registrationForm()
        {
            InitializeComponent();
        }
        public registrationForm(Form1 form1)
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form form1 = new Form1(this);
            form1.Show();
            this.Hide();
        }

        private void registrationForm_Load(object sender, EventArgs e)
        {
            if (dbcon.CheckConnection())
                label6.Text = "Подключено";
            else label6.Text = "Ошибка";
        }

        private void label6_Click(object sender, EventArgs e)
        {
            if (dbcon.CheckConnection())
                label6.Text = "Good";
            else label6.Text = "Error";
        }

        private void registrationForm_FormClosed(object sender, FormClosedEventArgs e)
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
                    dbcon.registration(login, password);
                    Form form1 = new Form1(this);
                    form1.Show();
                    this.Hide();

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
        }
    }
}
