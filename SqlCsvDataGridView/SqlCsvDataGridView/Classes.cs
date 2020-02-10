using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SqlClient = System.Data.SqlClient;
using Odbc = System.Data.Odbc;
using IO = System.IO;

namespace SqlCsvDataGridView
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

        // SQLGetReader
        public Odbc.OdbcDataReader SQLGetReader(string command)
        {
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = command;
            sqlDataReader = sqlCommand.ExecuteReader();
            return sqlDataReader;
        }
    }
}
