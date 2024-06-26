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
    public partial class competitionForm : Form
    {
        DbConnection dbcon = new DbConnection();
        string login;
        public competitionForm()
        {
            InitializeComponent();
        }
        public competitionForm(newForm newform, string login)
        {
            this.login = login;
            InitializeComponent();
        }

        private void competitionForm_Load(object sender, EventArgs e)
        {
            loadcompetition();
        }
        private void loadcompetition()
        {
            try
            {
                DataTable scores = null;

                scores = dbcon.GetTableData("competition_view");

                dataGridView1.DataSource = scores;
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
                    string query = "insert into participant (id_sportsman, id_competition) values ((select s.id from sportsman s join users u on u.id = s.users_id where u.login = @login limit 1), @id);";
                    dbcon.InsertTeam(query, id, login);

                    MessageBox.Show("Вы записались на соревнование");
                    loadcompetition();
                }
                else
                {
                    MessageBox.Show("Выберите запись");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении данных: {ex.Message}");
            }
        }
    }
}
