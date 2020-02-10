using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Diagnostics;

namespace DataGridViewCustomComboBox
{
    public partial class MainForm : Form
    {
        string[] cities = new string[] { "Melbourne", "Sydney", "Perth", "Darwin" };
        string[] states = new string[] { "VIC", "NSW", "SA", "NT" };
        string[] countries = new string[] { "Australia" };

        #region Constructor
        public MainForm()
        {
            InitializeComponent();
        }
        #endregion


        #region CustomDataGridViewComboBoxCell
        private DataGridViewComboBoxCell CustomDataGridViewComboBoxCell(object cellValue, string[] items)
        {
            DataGridViewComboBoxCell dataGridViewComboBoxCell = new DataGridViewComboBoxCell();
            if (cellValue != null && !(items.Contains(cellValue)))
            {
                dataGridViewComboBoxCell.Items.Add(cellValue);
                dataGridViewComboBoxCell.Value = cellValue;
            }
            dataGridViewComboBoxCell.Items.AddRange(items);
            return dataGridViewComboBoxCell;
        }
        #endregion


        #region buttonQuit_Click
        private void buttonQuit_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion

        #region buttonRun_Click
        private void buttonRun_Click(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("City", typeof(string));
            dataTable.Columns.Add("State", typeof(string));
            dataTable.Columns.Add("Country", typeof(string));
            dataTable.Rows.Add("Melbourne", "VIC", "Australia");
            dataTable.Rows.Add("Sydney", "NSW", "Australia");
            dataTable.Rows.Add("Adelaide", "SA", "Australia");
            dataTable.Rows.Add("Darwin", "NT", "Australia");
            dataTable.Rows.Add("Hobart", "TAS", "Australia");

            dataGridView.DataSource = null;
            dataGridView.DataSource = dataTable;
        }
        #endregion

        #region dataGridView_DataError
        private void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            Debug.WriteLine(string.Concat("DataError:", " ",
                "e.ColumnIndex: ", e.ColumnIndex, " ",
                "e.RowIndex: ", e.RowIndex, " ",
                "e.Context: ", e.Context, " ",
                "e.Exception: ", e.Exception
            ));
        }
        #endregion

        #region dataGridView_DefaultValuesNeeded
        private void dataGridView_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            DataGridView dataGridView = sender as DataGridView;

            object cellValue = null;
            int i = e.Row.Index;

            cellValue = dataGridView.Rows[i].Cells[0].Value;
            dataGridView.Rows[i].Cells[0] = CustomDataGridViewComboBoxCell(cellValue, cities);

            cellValue = dataGridView.Rows[i].Cells[1].Value;
            dataGridView.Rows[i].Cells[1] = CustomDataGridViewComboBoxCell(cellValue, states);

            cellValue = dataGridView.Rows[i].Cells[2].Value;
            dataGridView.Rows[i].Cells[2] = CustomDataGridViewComboBoxCell(cellValue, countries);
        }
        #endregion

        #region dataGridView_DataSourceChanged
        private void dataGridView_DataSourceChanged(object sender, EventArgs e)
        {
            DataGridView dataGridView = sender as DataGridView;

            object cellValue = null;

            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                cellValue = dataGridView.Rows[i].Cells[0].Value;
                dataGridView.Rows[i].Cells[0] = CustomDataGridViewComboBoxCell(cellValue, cities);

                cellValue = dataGridView.Rows[i].Cells[1].Value;
                dataGridView.Rows[i].Cells[1] = CustomDataGridViewComboBoxCell(cellValue, states);

                cellValue = dataGridView.Rows[i].Cells[2].Value;
                dataGridView.Rows[i].Cells[2] = CustomDataGridViewComboBoxCell(cellValue, countries);
            }
        }
        #endregion
    }
}