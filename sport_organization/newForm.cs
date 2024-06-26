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
    public partial class newForm : Form
    {
        DbConnection dbcon = new DbConnection();
        string login;
        public newForm()
        {
            InitializeComponent();
        }
        public newForm(Form1 form1, string login)
        {
            this.login = login;
            InitializeComponent();
        }

        private void newForm_Load(object sender, EventArgs e)
        {
            loadinfo();
            loadteams();
            loadcompetition();
            loadscores();

            button1.Visible = true;
            button3.Visible = true;
            dataGridView2.ReadOnly = true;

        }
        private void loadscores()
        {
            try
            {
                DataTable scores = null;

                scores = dbcon.GetInfo(login, "get_info_about_sportsman_win");

                dataGridView4.DataSource = scores;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке информации: {ex.Message}");
            }
        }
        private void loadcompetition()
        {
            try
            {
                DataTable competition = null;

                competition = dbcon.GetInfo(login, "get_info_about_sportsman_competition");

                dataGridView3.DataSource = competition;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке информации: {ex.Message}");
            }
        }
        private void loadteams()
        {
            try
            {
                DataTable teams = null;

                teams = dbcon.GetInfo(login, "get_info_about_sportsmans_team");

                dataGridView2.DataSource = teams;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке информации: {ex.Message}");
            }
        }
        private void loadinfo()
        {
            try
            {
                DataTable info = null;

                info = dbcon.GetInfo(login, "get_info_about_sportsman");

                if (info == null || info.Rows.Count == 0)
                {
                    dbcon.SetInfo(login, "sportsman");
                    info = dbcon.GetInfo(login, "get_info_about_sportsman");
                }

                dataGridView1.DataSource = info;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке информации: {ex.Message}");
            }
        }
        private void newForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string columnName = correctcolumnname(dataGridView1.Columns[e.ColumnIndex].Name);
                string typeColumn = dataGridView1.Columns[e.ColumnIndex].ValueType.Name;

                object newValue = getNewValue(typeColumn, dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);

                if (columnName != null)
                {
                    string query = $"UPDATE sportsman SET {columnName} = @value WHERE users_id = (SELECT id FROM users WHERE login = @login limit 1)";
                    dbcon.UpdateInfo(query, newValue, login);

                    loadinfo();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении данных: {ex.Message}");
            }
        }
        private string correctcolumnname(string name)
        {
            string correctname = null;

            switch (name)
            {
                case "имя":
                    correctname = "name";
                    break;
                case "фамилия":
                    correctname = "lastname";
                    break;
                case "отчество":
                    correctname = "patronymic";
                    break;
                case "мобильный":
                    correctname = "phone";
                    break;
                case "день_рождения":
                    correctname = "birthday";
                    break;
            }

            return correctname;
        }
        private object getNewValue(string typeColumn, object value)
        {
            object newValue = null;

            switch (typeColumn)
            {
                case "DateTime":
                    newValue = Convert.ToDateTime(value);
                    break;
                case "Decimal":
                    newValue = Convert.ToDecimal(value);
                    break;
                case "String":
                    newValue = value.ToString();
                    break;
                case "Int32":
                    newValue = Convert.ToInt32(value);
                    break;
                case "Boolean":
                    newValue = Convert.ToBoolean(value);
                    break;
            }

            return newValue;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form form1 = new Form1(this);
            form1.Show();
            this.Hide();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            loadinfo();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            loadteams();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form addteam = new addTeam(this, login);
            addteam.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                int id = -1;

                if (dataGridView2.SelectedCells.Count > 0)
                {
                    int selectedRowIndex = dataGridView2.SelectedCells[0].RowIndex;
                    id = Convert.ToInt32(dataGridView2.Rows[selectedRowIndex].Cells[0].Value);
                }

                if (id != -1)
                {
                    string query = "delete from sportsman_team where id_team = @id and id_sportsman = (select s.id from sportsman s join users u on u.id = s.users_id where login = @login limit 1) ";
                    dbcon.deleteTeam(query, id, login);

                    MessageBox.Show("Вы удалены из команды :(");
                    loadteams();
                }
                else
                {
                    MessageBox.Show("Выберите строку для удаления.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении данных: {ex.Message}");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                int id = -1;

                if (dataGridView3.SelectedCells.Count > 0)
                {
                    int selectedRowIndex = dataGridView3.SelectedCells[0].RowIndex;
                    id = Convert.ToInt32(dataGridView3.Rows[selectedRowIndex].Cells[0].Value);
                }

                if (id != -1)
                {
                    string query = "delete from participant where id_competition = @id and id_sportsman = (select s.id from sportsman s join users u on u.id = s.users_id where login = @login limit 1) ";
                    dbcon.deleteTeam(query, id, login);

                    MessageBox.Show("Вы удалены с соревнований");
                    loadcompetition();
                }
                else
                {
                    MessageBox.Show("Выберите строку для удаления.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении данных: {ex.Message}");
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {
            loadcompetition();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            loadscores();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form competitionform = new competitionForm(this, login);
            competitionform.Show();
        }
    }
}
