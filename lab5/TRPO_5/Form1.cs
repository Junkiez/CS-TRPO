using System.Diagnostics.Metrics;
using System.Reflection;
using System.Windows.Forms;

namespace TRPO_5
{
    public partial class Form1 : Form
    {
        public int counter = 0;
        public Processing p;
        public string[] countResult = new string[] {};
        public Form1()
        {
            InitializeComponent();
            p = new Processing(this);

            dataGridView1.ColumnCount = 2;
            dataGridView1.Columns[0].Name = "x";
            dataGridView1.Columns[1].Name = "result";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer.Start();
            dataGridView1.Rows.Clear();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(() => this.p.call(counter)));
            t.Start();

            if(countResult.Length > 0)
            {
                dataGridView1.Rows.Add(countResult);
                countResult = new string[] { };
            }

            counter++;
            label1.Text = counter.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer.Stop();
            counter= 0;
            label1.Text = counter.ToString();
        }

        public class Processing
        {
            private Form1 form;

            public Processing(Form1 outer)
            {
                form = outer;
            }

            public void call(int x)
            {
                string[] row = new string[] { x.ToString(), this.fun(x).ToString() };
                Thread.Sleep(5000);
                form.countResult = row;
            }
            private double fun(int x) => x >= 2 ? x * 0.6 : 2 / (x * 3);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }

}