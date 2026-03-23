using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Grimmory
{
    class AppSettings
    {
        private const string EncryptedPasswordPrefix = "enc:";
        private static bool _decryptWarningShown = false;

        public string Server { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public bool IsbnLookup { get; set; }
        public bool AutoConnect { get; set; }

        public AppSettings()
        {
            Server = String.Empty;
            User = String.Empty;
            Password = String.Empty;
            IsbnLookup = false;
            AutoConnect = false;
        }

        private static string GetSettingsPath()
        {
            string fullPath = Assembly.GetExecutingAssembly().GetName().CodeBase;
            string folder = Path.GetDirectoryName(fullPath);
            return Path.Combine(folder, "settings.txt");
        }

        public bool Save()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(GetSettingsPath(), false))
                {
                    sw.WriteLine(Server);
                    sw.WriteLine(User);
                    string encryptedPassword = DeviceCrypto.EncryptToBase64(Password, User);
                    sw.WriteLine(EncryptedPasswordPrefix + encryptedPassword);
                    sw.WriteLine(IsbnLookup.ToString());
                    sw.WriteLine(AutoConnect.ToString());
                    return true;
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("File access error: " + ex.Message, "Save Error");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unexpected error while saving: " + ex.Message, "Save Error");
            }
            return false;
        }

        public static AppSettings Load()
        {
            AppSettings config = new AppSettings();
            string path = GetSettingsPath();

            if (!File.Exists(path))
                return config;

            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    config.Server = sr.ReadLine() ?? String.Empty;
                    config.User = sr.ReadLine() ?? String.Empty;
                    string passwordLine = sr.ReadLine() ?? String.Empty;
                    config.Password = ParseStoredPassword(passwordLine, config.User, config);
                    string isbnLine = sr.ReadLine();
                    if (!String.IsNullOrEmpty(isbnLine))
                    {
                        config.IsbnLookup = isbnLine.Equals("true", StringComparison.OrdinalIgnoreCase) || isbnLine == "1";
                    }
                    string autoConnectLine = sr.ReadLine();
                    if (!String.IsNullOrEmpty(autoConnectLine))
                    {
                        config.AutoConnect = autoConnectLine.Equals("true", StringComparison.OrdinalIgnoreCase) || autoConnectLine == "1";
                    }
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("Could not read settings file: " + ex.Message, "Load Error");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading settings: " + ex.Message, "Load Error");
            }

            return config;
        }

        private static string ParseStoredPassword(string passwordLine, string username, AppSettings config)
        {
            if (String.IsNullOrEmpty(passwordLine))
                return String.Empty;

            if (!passwordLine.StartsWith(EncryptedPasswordPrefix, StringComparison.OrdinalIgnoreCase))
            {
                // Legacy plain-text format support.
                return passwordLine;
            }

            string encryptedData = passwordLine.Substring(EncryptedPasswordPrefix.Length);
            try
            {
                return DeviceCrypto.DecryptFromBase64(encryptedData, username);
            }
            catch
            {
                // Prevent repeated failed auto-connect attempts during this session.
                config.AutoConnect = false;
                if (!_decryptWarningShown)
                {
                    _decryptWarningShown = true;
                    MessageBox.Show(
                        "Saved password could not be decrypted on this device. Please re-enter your password and save settings again.",
                        "Credential Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation,
                        MessageBoxDefaultButton.Button1);
                }
                return String.Empty;
            }
        }
    }
}
