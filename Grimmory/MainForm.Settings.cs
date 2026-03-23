using System;
using System.Windows.Forms;

namespace Grimmory
{
    // Settings Management Logic
    partial class MainForm
    {
        private void LoadSettings()
        {
            AppSettings settings = _settingsStore.Load();
            settingsServerTexbox.Text = settings.Server;
            settingsUserTextbox.Text = settings.User;
            settingsPasswordTextbox.Text = settings.Password;
            isbnLookupCheckbox.Checked = settings.IsbnLookup;
            autoConnectCheckbox.Checked = settings.AutoConnect;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            string user = settingsUserTextbox.Text.Trim();
            string pass = settingsPasswordTextbox.Text.Trim();
            string server = settingsServerTexbox.Text.Trim();
            bool isbnLookup = isbnLookupCheckbox.Checked;
            bool autoConnect = autoConnectCheckbox.Checked;

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass) || string.IsNullOrEmpty(server))
            {
                MessageBox.Show("Please fill in all fields", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return;
            }

            // Save settings
            AppSettings settings = new AppSettings
            {
                Server = server,
                User = user,
                Password = pass,
                IsbnLookup = isbnLookup,
                AutoConnect = autoConnect
            };

            if (_settingsStore.Save(settings))
            {
                MessageBox.Show("Settings saved successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
            }
            else
            {
                MessageBox.Show("Failed to save settings", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }
        }
    }
}
