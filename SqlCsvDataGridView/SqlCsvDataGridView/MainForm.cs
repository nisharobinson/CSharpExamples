using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Threading = System.Threading;

namespace SqlCsvDataGridView
{
    public partial class MainForm : Form
    {
        private static MainForm form = null;

        private delegate void EnableDelegateTextBox(string text);
        private delegate void EnableDelegateDataTable(DataTable dataTable);

        BackgroundWorker backgroundWorkerRun = new BackgroundWorker();

        DataTable MyDataTable = new DataTable();

        // Constructor
        public MainForm()
        {
            InitializeComponent();
            // form is MainForm
            form = this;
            // backgroundWorkerRun
            backgroundWorkerRun.DoWork += backgroundWorkerRunDoWork;
            backgroundWorkerRun.RunWorkerCompleted += backgroundWorkerRunCompleted;
        }

        // AppendTextTextBoxOutputStatic
        public static void AppendTextTextBoxOutputStatic(string text)
        {
            if (form != null)
                form.AppendTextTextBoxOutput(text);
        }

        // AppendTextTextBoxOutputStaticNewLine
        public static void AppendTextTextBoxOutputStaticNewLine(string text)
        {
            if (form != null)
                form.AppendTextTextBoxOutput(text + Environment.NewLine);
        }

        // AppendTextTextBoxOutput
        private void AppendTextTextBoxOutput(string text)
        {
            // If this returns true, it means it was called from an external thread.
            if (InvokeRequired)
            {
                // Create a delegate of this method and let the form run it.
                this.Invoke(new EnableDelegateTextBox(AppendTextTextBoxOutput), new object[] { text });
                return; // Important
            }
            // AppendText to TextBox
            textBoxOutput.AppendText(text);
        }

        // SetDataGridViewDataSource
        private void SetDataGridViewDataSource(DataTable dataTable)
        {
            // If this returns true, it means it was called from an external thread.
            if (InvokeRequired)
            {
                // Create a delegate of this method and let the form run it.
                this.Invoke(new EnableDelegateDataTable(SetDataGridViewDataSource), new object[] { dataTable });
                return; // Important
            }
            dataGridViewMain.DataSource = dataTable;
        }

        // buttonQuit_Click
        private void buttonQuit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        // buttonRun_Click
        private void buttonRun_Click(object sender, EventArgs e)
        {
            // Disable the button
            buttonRun.Enabled = false;
            // Run the Worker Thread
            backgroundWorkerRun.RunWorkerAsync();
        }

        // backgroundWorkerRunDoWork
        private void backgroundWorkerRunDoWork(object sender, DoWorkEventArgs e)
        {
            // Find out how fast things run
            string[] secondsTaken = new string[3];
            DateTime[] dateTime = new DateTime[3];
            // Note the time when the thread starts
            dateTime[0] = DateTime.Now;
            // First
            dateTime[1] = DateTime.Now;
            SqlCsv sqlCsv = new SqlCsv();
            DataTable dataTable = new DataTable();
            dataTable.Constraints.Clear();
            dataTable.Load(sqlCsv.SQLGetReader(form.textBoxSqlCommand.Text));
            secondsTaken[1] = (DateTime.Now - dateTime[1]).TotalSeconds.ToString();
            // Second
            dateTime[2] = DateTime.Now;
            SetDataGridViewDataSource(dataTable);
            secondsTaken[2] = (DateTime.Now - dateTime[2]).TotalSeconds.ToString();

            // Finished all the work note the time
            secondsTaken[0] = (DateTime.Now - dateTime[0]).TotalSeconds.ToString();

            // Set the event Result to how long the thread took to execute
            e.Result = secondsTaken;
        }

        // backgroundWorkerRunCompleted
        private void backgroundWorkerRunCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string[] result = (e.Result as string[]);

            AppendTextTextBoxOutputStaticNewLine(
                String.Format("sqlCsv.SQLGetReader(): Time: {0}",
                result[1].ToString()));
            AppendTextTextBoxOutputStaticNewLine(
                String.Format("SetDataGridViewDataSource(): Time: {0}",
                result[2].ToString()));
            // Report how long the thread took to run
            AppendTextTextBoxOutputStaticNewLine(
                String.Format("backgroundWorkerRunCompleted(): Time: {0}",
                result[0].ToString()));

            // Enable the button
            buttonRun.Enabled = true;
        }
    }
}
