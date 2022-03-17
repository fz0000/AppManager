using Microsoft.Win32;
using System;
using System.Windows.Forms;

namespace AppManager
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void toolStripButtonLoad_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            //dataGridView1.Columns.Clear();
            Refresh();
            //dataGridView1.Columns.Add("Name", "Name");
            //dataGridView1.Columns["Name"].SortMode = DataGridViewColumnSortMode.NotSortable;
            RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");
            string[] keyList = rk.GetSubKeyNames();
            RegistryKey rkApp;
            string appName;
            string pubName;
            foreach (string key in keyList)
            {
                rkApp = rk.OpenSubKey(key);
                appName = (string)rkApp.GetValue("DisplayName");
                pubName = (string)rkApp.GetValue("Publisher");

                DataGridViewRow row = new DataGridViewRow();

                DataGridViewTextBoxCell textboxcellappName = new DataGridViewTextBoxCell();
                textboxcellappName.Value = appName;
                row.Cells.Add(textboxcellappName);

                DataGridViewTextBoxCell textboxcellpubName = new DataGridViewTextBoxCell();
                textboxcellpubName.Value = pubName;
                row.Cells.Add(textboxcellpubName);

                row.Tag = rkApp;
                dataGridView1.Rows.Add(row);
            }
        }
    }
}
