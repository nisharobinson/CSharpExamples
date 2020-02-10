using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Threading = System.Threading;

namespace SqlCsv
{
    public partial class MainForm : Form
    {
        private static MainForm form = null;
        private delegate void EnableDelegate(string text);

        BackgroundWorker backgroundWorkerRun = new BackgroundWorker();

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
                this.Invoke(new EnableDelegate(AppendTextTextBoxOutput), new object[] { text });
                return; // Important
            }

            // AppendText to TextBox
            textBoxOutput.AppendText(text);
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

            dateTime[1] = DateTime.Now;
            SqlCsv sqlCsv = new SqlCsv();
            List<string[]> rows = sqlCsv.SQLGetRows(@"SELECT * FROM Data.csv");
            secondsTaken[1] = (DateTime.Now - dateTime[1]).TotalSeconds.ToString();

            dateTime[2] = DateTime.Now;
            for (int i = 0; i < rows.Count; i++)
            {
                string[] row = rows[i];
                AppendTextTextBoxOutputStaticNewLine(String.Join(", ", row));
            }
            AppendTextTextBoxOutputStaticNewLine("");
            AppendTextTextBoxOutputStaticNewLine(String.Format("Total rows: {0}", rows.Count));
            AppendTextTextBoxOutputStaticNewLine("");
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
                String.Format("sqlCsv.SQLGetRows(): Time: {0}",
                result[1].ToString()));
            AppendTextTextBoxOutputStaticNewLine(
                String.Format("Show results in textBoxOutput(): Time: {0}",
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
