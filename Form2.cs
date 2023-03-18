using Npgsql;
using System;
using System.Windows.Forms;

namespace Laboratory_3
{
    public partial class Form2 : Form
    {
        private Form1 parent;

        public void setParent(Form1 parent)
        {
            this.parent = parent;
        }

        public Form2()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string server = textBox1.Text;
            int port = Int32.Parse(textBox2.Text);
            string database = textBox3.Text;
            string user = textBox4.Text;
            string passwd = textBox5.Text;
            NpgsqlConnection connection = parent.Connect(server, port, database, user, passwd);
            parent.setConnection(connection);
            parent.FillDataGridView1ByUser();
            this.Visible = false;
        }


    }
}
