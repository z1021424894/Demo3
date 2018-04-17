using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ServiceProcess;
using System.Data.SqlClient;
using System.Threading;
using System.Configuration;
using System.Data.Sql;
using System.Transactions;

namespace winfrom1
{
    public partial class Form1 : Form
    {
        private BackgroundWorker _backgroudWorker;
        string source = "server=(local);" + "integrated security=SSPI;" + "database=Northwind";
        string source1 = "Data Source=DMSOF-PC;Initial Catalog=Northwind;Integrated Security=True";
        string source2 = "Data Source=DMSOF-PC;Initial Catalog=Northwind;Persist Security Info=True;User ID=sa;Password=***********";
        public Form1()
        {
            InitializeComponent();
            label1.Text = ConfigurationManager.ConnectionStrings[0].Name;
        }
        private void buttonStart_Click(object sender, EventArgs e)
        {
            //string select = "select contactname,companyname from customers";
            //using(TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            //{
            //    using (SqlConnection conn = new SqlConnection(source))
            //    {
            //        conn.Open();
            //        SqlCommand cmd = new SqlCommand(select, conn);
            //        label1.Text = "online";
            //        SqlDataReader reader = cmd.ExecuteReader();
            //        while (reader.Read())
            //        {
                        
            //        }
            //        scope.Complete();
            //    }
            //}
            if(this._backgroudWorker == null)
            {
                _backgroudWorker = new BackgroundWorker();
                _backgroudWorker.WorkerSupportsCancellation = true;
                _backgroudWorker.WorkerReportsProgress = true;
                _backgroudWorker.DoWork += BackgroudWorker_DoWork;
                _backgroudWorker.ProgressChanged += BackgroudWorker_ProgressChanged;
                _backgroudWorker.RunWorkerCompleted += BackgroudWorker_RunWorkerCompleted;
            }
            _backgroudWorker.RunWorkerAsync();
        }

        private void BackgroudWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {                
                label1.Text = "cancel";
            }
            else
            {
                label1.Text = e.Result.ToString();
            }
        }

        private void BackgroudWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void BackgroudWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string values = ConfigurationManager.AppSettings["Aomi"];
            e.Result = values;

            for (int i = 0; i < 101; i++)
            {
                if (_backgroudWorker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                _backgroudWorker.ReportProgress(i);
                Thread.Sleep(200);
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            if(_backgroudWorker.IsBusy)
            {
                _backgroudWorker.CancelAsync();
            }
        }
    }
}
