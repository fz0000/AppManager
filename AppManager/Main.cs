using Microsoft.Win32;
using System;
using System.Drawing;
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
            Refresh();
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

                row.Tag = key;
                if (textboxcellappName.Value != null)
                {
                    dataGridView1.Rows.Add(row);
                }
            }
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            RegistryKey rk = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");
            string tmpKey;
            string strName, strPublisher;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                tmpKey = row.Tag.ToString();
                strName = row.Cells[0].Value.ToString();
                strPublisher = row.Cells[1].Value.ToString();

                rk.CreateSubKey(tmpKey).SetValue("DisplayName", strName);
                rk.CreateSubKey(tmpKey).SetValue("Publisher", strPublisher);
            }
        }
        private void toolStripButtonBackup_Click(object sender, EventArgs e)
        {
            // TODO: backup registry
            MessageBox.Show("Not ready");
        }

        private void toolStripButtonRestore_Click(object sender, EventArgs e)
        {
            // TODO: restore registry
            MessageBox.Show("Not ready");
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell curCell = (sender as DataGridView).CurrentCell;
            if (curCell != null)
            {
                curCell.Style.BackColor = Color.Yellow;
            }
        }
    }
}
