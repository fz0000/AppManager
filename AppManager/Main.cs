using Microsoft.Win32;
using System;
using System.Collections;
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
            string errApps = "";
            ArrayList tmpNames = new ArrayList();
            toolStripProgressBar1.Maximum = dataGridView1.RowCount;
            toolStripProgressBar1.Value = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                tmpKey = row.Tag.ToString();
                strName = row.Cells[0].Value.ToString();
                try
                {
                    rk.CreateSubKey(tmpKey).SetValue("DisplayName", strName);
                    if (row.Cells[1].Value == null)
                    {
                        RegistryKey rk_app = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + tmpKey);
                        if (rk_app.GetValue("Publisher") != null)
                        {
                            rk.CreateSubKey(tmpKey).DeleteValue("Publisher");
                        }
                    }
                    else
                    {
                        strPublisher = row.Cells[1].Value.ToString();
                        rk.CreateSubKey(tmpKey).SetValue("Publisher", strPublisher);
                    }
                }
                catch
                {
                    tmpNames.Add(strName);
                }
                toolStripProgressBar1.Value += 1;
                errApps = string.Join(", ", tmpNames.ToArray());
            }
            if (errApps != "")
            {
                MessageBox.Show("The following apps are not modified:" + Environment.NewLine + Environment.NewLine + errApps);
            }
            else
            {
                MessageBox.Show("OK");
            }
            toolStripProgressBar1.Value = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.Style.BackColor = Color.Empty;
                }
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
