using System;
using System.Windows.Forms;

namespace Laboratory_3
{
    public partial class Form4 : Form
    {
        private Form1 parent = null;
        private string user;
        private int row;
        public void setTextBox2Text(string text)
        {
            textBox2.Text = text;
        }
        public void setTextBox3Text(string text)
        {
            textBox3.Text = text;
        }
        public void setTextBox4Text(string text)
        {
            textBox4.Text = text;
        }
        public void setButton1Visible(bool value)
        {
            this.button1.Visible = value;
        }
        public void setButton2Visible(bool value)
        {
            this.button2.Visible = value;
        }

        public void setParent(Form1 parent)
        {
            this.parent = parent;
        }
        public void setUser(string user)
        {
            this.user = user;
        }
        public void setRow(int row)
        {
            this.row = row;
        }
        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            parent.AddBook(Convert.ToInt32(this.user), textBox2.Text, textBox3.Text, textBox4.Text);
            parent.FillDataGridView2ByBooks(user);
            this.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            parent.UpdateBook(row, Convert.ToInt32(this.user), textBox2.Text, textBox3.Text, textBox4.Text);
            parent.FillDataGridView2ByBooks(user);
            this.Visible = false;
        }
    }
}
