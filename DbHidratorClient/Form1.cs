using DbHidrator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class MainForm : Form
    {
        DbHidratorClient client;
        SynchronizationContext syncContext;
        public MainForm()
        {
            InitializeComponent();
            syncContext = SynchronizationContext.Current;
            cmbDbProvider.SelectedIndex = 0;
        }

        private async void btnGetTables_Click(object sender, EventArgs e)
        {
            var conn = txtConnStr.Text;
            var provider = cmbDbProvider.SelectedItem.ToString();
            string[] tablesList = await GetTablesList(conn, provider);
            lstchkTables.Items.AddRange(tablesList);
        }

        private Task<string[]> GetTablesList(string connStr, string providerStr)
        {
            return Task<string[]>.Run(() =>
            {
                client = new DbHidratorClient(connStr, providerStr);
                client.GenerateSchema();
                var tablesList = client.GetTablesNames();
                return tablesList;
            });
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            var conn = txtConnStr.Text;
            var provider = cmbDbProvider.SelectedItem.ToString();
            var info = lstchkTables.CheckedItems.Cast<String>().ToArray();
            if (rdbtnAll.Checked)
            {
                await GenerateNRows(conn, provider, (int)numRows.Value, info);
            }
            else
            {
                await SimulateRandomInserts(conn, provider,(int)numRows.Value, info);
            }
        }

        private Task SimulateRandomInserts(string conn, string provider,int numberOfRows, string[] tables)
        {
            return Task.Run(() =>
            {
                client.GenerateRowsInRandomTimes(numberOfRows,tables);
                Report(new Tuple<int, int>(100, 100));
            });
        }

        private Task<bool> GenerateNRows(string connStr, string providerStr,int numberOfRows,string[] tables)
        {
            return Task<bool>.Run(() =>
            {
                client.GenerateNRows(numberOfRows, tables);
                Report(new Tuple<int, int>(100, 100));
                return true;
            });
        }

        public void Report(Tuple<int, int> value)
        {
            syncContext.Post((data) => {
                Tuple<int, int> rangeProgress =(Tuple<int,int>) data;
                progress.Maximum = rangeProgress.Item1;
                progress.Value = rangeProgress.Item2;
            }, value);

        }
    }
}
