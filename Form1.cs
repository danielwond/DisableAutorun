using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DisableAutorun
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            FormBorderStyle = FormBorderStyle.FixedSingle;

            var status = GetNoDriveTypeAutoRunValue();
            if (status == 0)
            {
                //button1.Text = "Disable Autorun";
                //lblHelp.Text = "Currently, Autoruns will execute when USB is Plugged in";
                //lblStatus.Text = "ENABLED";
                //lblStatus.BackColor = Color.Green;
                EnableorDisable("ENABLED");
            }
            else
            {
                //button1.Text = "Enable Autorun";
                //lblHelp.Text = "Currently, Autoruns will not execute when USB is Plugged in";
                //lblStatus.Text = "DISABLED";
                //lblStatus.BackColor = Color.Red;
                EnableorDisable("DISABLED");

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //IF IT IS DISABLED, ENABLE IT
            if (GetNoDriveTypeAutoRunValue() == 0)
            {
                string keyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer";
                string subKeyName = "NoDriveTypeAutoRun"; // Subkey name to delete
                var valueData = 0x000000FF; // Example data

                // Open the registry key with write access
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(keyPath, true))
                {
                    if (key != null)
                    {
                        // Delete the subkey if it exists
                        key.DeleteValue(subKeyName, false);
                    }
                }

                // Create the subkey
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(keyPath))
                {
                    key.SetValue(subKeyName, valueData, RegistryValueKind.DWord);
                }
                EnableorDisable("DISABLED");
                MessageBox.Show("Reboot the computer to save changes", "Success", MessageBoxButtons.OK, icon: MessageBoxIcon.Warning);

            }
            //IF IT IS ENABLED, DISABLE IT
            else
            {
                string keyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer";
                string subKeyName = "NoDriveTypeAutoRun"; // Subkey name to delete

                // Open the registry key with write access
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(keyPath, true))
                {
                    if (key != null)
                    {
                        // Delete the subkey if it exists
                        key.DeleteValue(subKeyName, false);
                    }
                }
                //button1.Text = "Disable Autorun";
                //lblStatus.Text = "ENABLED";
                //lblHelp.Text = "Currently, Autoruns will execute when USB is Plugged in";

                EnableorDisable("ENABLED");
                MessageBox.Show("Reboot the computer to save changes", "Success", MessageBoxButtons.OK, icon: MessageBoxIcon.Warning);
            }
        }
        public int GetNoDriveTypeAutoRunValue()
        {
            try
            {
                const string keyName = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer";
                const string valueName = "NoDriveTypeAutoRun";

                using (var key = Registry.CurrentUser.OpenSubKey(keyName, true))
                {
                    if (key != null)
                    {
                        var value = key.GetValue(valueName);
                        if (value != null && value is int)
                        {
                            int dwValue = (int)value;
                            return dwValue;
                        }
                    }
                    return 0;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void EnableorDisable(string value)
        {
            if (value == "ENABLED")
            {
                button1.Text = "Disable Autorun";
                lblHelp.Text = "Currently, Autoruns will execute when USB is Plugged in";
                lblStatus.Text = "ENABLED";
                lblStatus.BackColor = Color.Red;
                lblStatus.ForeColor = Color.White;
            }
            else if (value == "DISABLED")
            {
                button1.Text = "Enable Autorun";
                lblHelp.Text = "Currently, Autoruns will not execute when USB is Plugged in";
                lblStatus.Text = "DISABLED";
                lblStatus.BackColor = Color.Green;
                lblStatus.ForeColor = Color.White;
            }
        }
    }
}
