using System.Diagnostics.Metrics;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
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
            Thread t = new Thread(new ThreadStart(() => this.p.call(++counter)));
            t.Start();
            t.Join();

            label1.Text = counter.ToString();

            if (countResult.Length > 0)
            {
                dataGridView1.Rows.Add(countResult);
                countResult = new string[] { };
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer.Stop();
            counter = 1;
            label1.Text = counter.ToString();
        }

        public class Processing
        {
            private int V = 2;
            private Form1 form;
            private double result;

            public Processing(Form1 outer)
            {
                form = outer;
            }

            public void call(int tick)
            {
                Thread adapter = new Thread(new ThreadStart(() =>
                {
                    Thread.Sleep(3200 * V);
                    Thread t = tick >= 2 ? new Thread(new ThreadStart(() =>
                    {
                        this.result = tick * 0.6;
                        string[] row = new string[] { tick.ToString(), result.ToString() };
                        form.countResult = row;
                    })) : new Thread(new ThreadStart(() =>
                    {
                        this.result = 2 / (tick * 3);
                        string[] row = new string[] { tick.ToString(), result.ToString() };
                        form.countResult = row;
                    }));
                    t.Start();
                }));
                adapter.Start();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }

}