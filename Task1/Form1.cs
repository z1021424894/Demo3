using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Task1
{
    public partial class Form1 : Form
    {
        CancellationTokenSource cts = new CancellationTokenSource();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Task t = new Task(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    if (cts.IsCancellationRequested) return;
                    this.Invoke(new Action(() =>
                    {
                        label1.Text = i.ToString();
                    }));
                    Thread.Sleep(1000);
                }
            }, cts.Token);
            t.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cts.Cancel();
        }
    }
}
