using Npgsql;
using System;
using System.Data;
using System.Windows.Forms;

namespace Laboratory_3
{
    public partial class Form1 : Form
    {
        private NpgsqlConnection connection = null;
        private DataSet dataSet = null;
        private NpgsqlDataAdapter UserDataAdapter = null;
        private NpgsqlDataAdapter BookDataAdapter = null;
        private Form2 form2 = null;
        private Form3 form3 = null;
        private Form4 form4 = null;

        public void AddBook(int id_user, string title, string author, string publishing)
        {
            getDataSet().Tables["book"]
            .Rows.Add(id_user, title, author, publishing);
            BookDataAdapter.Update(getDataSet(), "book");
        }
        public void UpdateBook(int row, int id_user, string title, string author, string publishing)
        {
            getDataSet().Tables["book"].Rows[row]["id_user"] = id_user;
            getDataSet().Tables["book"].Rows[row]["title"] = title;
            getDataSet().Tables["book"].Rows[row]["author"] = author;
            getDataSet().Tables["book"].Rows[row]["publishing"] = publishing;
            BookDataAdapter.Update(getDataSet(), "book");
        }

        public Form4 getForm4()
        {
            if (form4 == null)
            {
                form4 = new Form4();
                form4.setParent(this);
            }
            return form4;
        }

        public Form3 getForm3()
        {
            if (form3 == null)
            {
                form3 = new Form3();
                form3.setParent(this);
            }
            return form3;
        }
        public void AddUser(string name, string surname, string phone)
        {
            getDataSet().Tables["user"].Rows.Add(dataGridView1.Rows.Count + 1, name, surname, phone);
            UserDataAdapter.Update(getDataSet(), "user");
        }

        public void UpdateUser(int row, string name, string surname, string phone)
        {
            getDataSet().Tables["user"].Rows[row]["name"] = name;
            getDataSet().Tables["user"].Rows[row]["surname"] = surname;
            getDataSet().Tables["user"].Rows[row]["phone"] = phone;
            UserDataAdapter.Update(getDataSet(), "user");
        }


        public void setConnection(NpgsqlConnection connection)
        {
            this.connection = connection;
        }

        //Создание DataSet
        private DataSet getDataSet()
        {
            if (dataSet == null)
            {
                dataSet = new DataSet();
                dataSet.Tables.Add("user");
                dataSet.Tables.Add("book");
            }
            return dataSet;
        }
        //Получить форму установления соединения
        public Form2 getForm2()
        {
            if (form2 == null)
            {
                form2 = new Form2();
                form2.setParent(this);
            }
            return form2;
        }

        //Установить соединение с базой
        public NpgsqlConnection Connect(string host, int port, string database,
         string user, string passw)
        {
            NpgsqlConnectionStringBuilder stringBuilder =
            new NpgsqlConnectionStringBuilder();
            stringBuilder.Host = host;
            stringBuilder.Port = port;
            stringBuilder.Username = user;
            stringBuilder.Password = passw;
            stringBuilder.Database = database;
            stringBuilder.Timeout = 30;
            NpgsqlConnection connection =
            new NpgsqlConnection(stringBuilder.ConnectionString);
            connection.Open();
            return connection;
        }

        //Заполнить DataGridView1
        public void FillDataGridView1ByUser()
        {
            getDataSet().Tables["user"].Clear();
            UserDataAdapter = new NpgsqlDataAdapter(
            "SELECT * FROM \"user\" ORDER BY \"id\"", connection);
            new NpgsqlCommandBuilder(UserDataAdapter);
            UserDataAdapter.Fill(getDataSet(), "user");
            dataGridView1.DataSource = getDataSet().Tables["user"];
            UpdateTransferPopup();
        }
        //Заполнить DataGridView2
        public void FillDataGridView2ByBooks(string id)
        {
            getDataSet().Tables["book"].Clear();
            BookDataAdapter = new NpgsqlDataAdapter(
            "SELECT * " +
            "FROM \"book\" " +
            "WHERE \"id_user\" = " + id, connection);
            new NpgsqlCommandBuilder(BookDataAdapter);
            BookDataAdapter.Fill(dataSet, "book");
            dataGridView2.DataSource = getDataSet().Tables["book"];
            UpdateTransferPopup();
        }



        public Form1()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int selectedRow = dataGridView1.SelectedCells[0].RowIndex;
            //int key = (int)(long)dataGridView1.Rows[0].Cells[0].Value;

            FillDataGridView2ByBooks(dataGridView1.Rows[selectedRow].Cells[0].Value.ToString());
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            getForm3().Visible = true;
            getForm3().setButton1Visible(true);
            getForm3().setButton2Visible(false);
            getForm3().setTextBox2Text("");
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedRow = dataGridView1.SelectedCells[0].RowIndex;
            DialogResult dr = MessageBox.Show("Delete user?", "",
            MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                getDataSet().Tables["user"].Rows[selectedRow].Delete();
                UserDataAdapter.Update(getDataSet(), "user");
                getDataSet().Clear();
                FillDataGridView1ByUser();
            }

        }

        private void changeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedRow = dataGridView1.SelectedCells[0].RowIndex;
            string sId = Convert.ToString(getDataSet().Tables["user"].Rows[selectedRow].ItemArray[0]);
            string sName = (string)getDataSet().Tables["user"].Rows[selectedRow].ItemArray[1];
            string sSurname = (string)getDataSet().Tables["user"].Rows[selectedRow].ItemArray[2];
            string sPhone = (string)getDataSet().Tables["user"].Rows[selectedRow].ItemArray[3];
            getForm3().Visible = true;
            getForm3().setButton1Visible(false);
            getForm3().setButton2Visible(true);
            getForm3().setTextBox2Text(sName);
            getForm3().setTextBox3Text(sSurname);
            getForm3().setTextBox4Text(sPhone);
            getForm3().setRow(selectedRow);

        }

        private void addToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            getForm4().Visible = true;
            getForm4().setButton1Visible(true);
            getForm4().setButton2Visible(false);
            getForm4().setTextBox2Text("");
            getForm4().setTextBox3Text("");
            getForm4().setTextBox4Text("");
            int selectedRow = dataGridView1.SelectedCells[0].RowIndex;
            int user = (int)getDataSet().Tables["user"].Rows[selectedRow].ItemArray[0];
            getForm4().setUser(user.ToString());
        }

        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int selectedRow = dataGridView2.SelectedCells[0].RowIndex;
            int selectedRow1 = dataGridView1.SelectedCells[0].RowIndex;
            DialogResult dr = MessageBox.Show("Delete book?", "",
            MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                getDataSet().Tables["book"].Rows[selectedRow].Delete();
                BookDataAdapter.Update(getDataSet(), "book");
                //int key = (int)(long)dataGridView1;
                FillDataGridView2ByBooks(dataGridView1.Rows[selectedRow1].Cells[0].Value.ToString());
            }

        }

        private void changeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int selectedRow = dataGridView2.SelectedCells[0].RowIndex;
            string sId = getDataSet().Tables["book"].Rows[selectedRow].ItemArray[0].ToString();
            string sTitle = getDataSet().Tables["book"].Rows[selectedRow].ItemArray[1].ToString();
            string sAuthor = getDataSet().Tables["book"].Rows[selectedRow].ItemArray[2].ToString();
            string sPublishing = getDataSet().Tables["book"].Rows[selectedRow].ItemArray[3].ToString();
            getForm4().Visible = true;
            getForm4().setButton1Visible(false);
            getForm4().setButton2Visible(true);
            getForm4().setTextBox2Text(sTitle);
            getForm4().setTextBox3Text(sAuthor);
            getForm4().setTextBox4Text(sPublishing);
            getForm4().setRow(selectedRow);
            int selectedRow1 = dataGridView1.SelectedCells[0].RowIndex;
            int userId = (int)getDataSet().Tables["user"]
            .Rows[selectedRow1].ItemArray[0];
            getForm4().setUser(userId.ToString());
        }


        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            connection.Close();
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView2.DataSource = null;
            dataGridView2.Rows.Clear();
            dataGridView2.Columns.Clear();
        }

        private void transferToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }



        /// /////////////////////////////// ////////////////////////////
        /// Захист
        private void UpdateTransferPopup()
        {
            transferToolStripMenuItem.DropDownItems.Clear();
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                ToolStripMenuItem newItem = new ToolStripMenuItem();
                object test = dataGridView1.Rows[i].Cells[0].Value;
                newItem.Text = dataGridView1.Rows[i].Cells[0].Value.ToString();
                newItem.Click += new EventHandler(addedItemClickEvent);
                transferToolStripMenuItem.DropDownItems.Add(newItem);
            }

        }
        private void addedItemClickEvent(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection selectedRows = dataGridView2.SelectedRows;
            foreach (DataGridViewRow row in selectedRows)
            {
                UpdateBook(row.Index, (int)(long)row.Cells[0].Value, (string)row.Cells[1].Value, (string)row.Cells[2].Value, (string)row.Cells[3].Value);
            }
            FillDataGridView2ByBooks(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
        }

        private void Custom_Click(object sender, EventArgs e)
        {
            getForm2().Visible = true;
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            string server = "localhost";
            int port = Int32.Parse("5432");
            string database = "Library";
            string user = "postgres";
            string passwd = "postgres";
            NpgsqlConnection connection = Connect(server, port, database, user, passwd);
            setConnection(connection);
            FillDataGridView1ByUser();
        }
    }
}
