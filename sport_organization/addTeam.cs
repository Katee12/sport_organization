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
    public partial class addTeam : Form
    {
        DbConnection dbcon = new DbConnection();
        string login;
        public addTeam()
        {
            InitializeComponent();
        }
        public addTeam(newForm newform, string login)
        {
            this.login = login;
            InitializeComponent();
        }

        private void addTeam_Load(object sender, EventArgs e)
        {
            loadteams();
        }
        private void loadteams()
        {
            try
            {
                DataTable teams = null;

                teams = dbcon.GetTableData("team_view");

                dataGridView1.DataSource = teams;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке информации: {ex.Message}");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int id = -1;

                if (dataGridView1.SelectedCells.Count > 0)
                {
                    int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;
                    id = Convert.ToInt32(dataGridView1.Rows[selectedRowIndex].Cells[0].Value);
                }

                if (id != -1)
                {
                    string query = $"insert into sportsman_team (id_sportsman, id_team) values ((select s.id from sportsman s join users u on u.id = s.users_id where u.login = @login limit 1), @id)";
                    dbcon.InsertTeam(query, id, login);

                    MessageBox.Show("Вы успешно записаны в команду!");
                }
                else
                {
                    MessageBox.Show("Выберите строку.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении данных: {ex.Message}");
            }
        }
    }
}
