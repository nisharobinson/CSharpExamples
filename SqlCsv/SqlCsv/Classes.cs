using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SqlClient = System.Data.SqlClient;
using Odbc = System.Data.Odbc;
using IO = System.IO;

namespace SqlCsv
{
    class SqlCsv
    {
        // Connection
        private Odbc.OdbcConnection sqlConnection = new Odbc.OdbcConnection();
        // Command
        private Odbc.OdbcCommand sqlCommand = new Odbc.OdbcCommand();
        // Reader
        private Odbc.OdbcDataReader sqlDataReader;

        // Constructor
        public SqlCsv()
        {
            string connectionString = string.Format(
                @"Driver={{Microsoft Text Driver (*.txt; *.csv)}};Dbq={0};Extensions={1};ColNameHeader={2};",
                IO.Directory.GetCurrentDirectory(), "csv,txt", "True");
            sqlConnection.ConnectionString = connectionString;
            try
            {
                sqlConnection.Open();
            }
            catch (Exception ex)
            {
                MainForm.AppendTextTextBoxOutputStaticNewLine(ex.ToString());
            }
        }

        // Destructor
        ~SqlCsv()
        {
            sqlDataReader = null;
            sqlCommand = null;
            sqlConnection = null;
        }

        // SQLGetRows
        public List<string[]> SQLGetRows(string command)
        {
            List<string[]> rows = new List<string[]>();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = command;
            sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                string[] row = new string[sqlDataReader.FieldCount];
                for (int i = 0; i < row.Length; i++)
                {
                    row[i] = sqlDataReader[i].ToString();
                }
                rows.Add(row);
            }
            sqlDataReader.Close();
            sqlDataReader = null;

            return rows;
        }
    }
}
