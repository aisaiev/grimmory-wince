using System;
using System.Windows.Forms;
using System.Threading;

namespace Grimmory
{
    // Authentication and Connection Logic
    partial class MainForm
    {
        private bool _isConnecting = false;
        private System.Windows.Forms.Timer _autoConnectTimer;

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Check if autoconnect is enabled
            AppSettings settings = _settingsStore.Load();
            if (settings.AutoConnect)
            {
                // Perform autoconnect if settings are configured
                if (!string.IsNullOrEmpty(settings.Server) &&
                    !string.IsNullOrEmpty(settings.User) &&
                    !string.IsNullOrEmpty(settings.Password))
                {
                    // Delay autoconnect to let the form fully render
                    _autoConnectTimer = new System.Windows.Forms.Timer();
                    _autoConnectTimer.Interval = AutoConnectDelayMs;
                    _autoConnectTimer.Tick += new EventHandler(AutoConnectTimer_Tick);
                    _autoConnectTimer.Enabled = true;
                }
            }
            
            // Set initial focus to ISBN textbox
            isbnTextbox.Focus();
        }

        private void AutoConnectTimer_Tick(object sender, EventArgs e)
        {
            // Disable timer immediately to prevent additional ticks
            if (_autoConnectTimer != null)
            {
                _autoConnectTimer.Enabled = false;
                _autoConnectTimer.Tick -= AutoConnectTimer_Tick;
                _autoConnectTimer.Dispose();
                _autoConnectTimer = null;
            }
            else
            {
                // Timer already processed, ignore this queued tick
                return;
            }

            // Trigger connection
            AppSettings settings = _settingsStore.Load();
            LoginParameters loginParams = LoginParameters.FromAppSettings(settings);
            _isConnecting = true;
            connectButton.Enabled = false;
            statusBar.Text = "Connecting...";
            Cursor.Current = Cursors.WaitCursor;
            ThreadPool.QueueUserWorkItem(new WaitCallback(PerformLogin), loginParams);
        }

        private void PerformLogin(object state)
        {
            // This runs on a ThreadPool thread
            LoginParameters loginParams = (LoginParameters)state;
            ApiResult result = null;
            Exception error = null;

            try
            {
                _apiClient.SetServerAddress(loginParams.Server);
                result = _apiClient.Login(loginParams.Username, loginParams.Password);
            }
            catch (Exception ex)
            {
                error = ex;
            }

            SafeUiInvoke(delegate { OnLoginCompleted(result, error); });
        }

        private void OnLoginCompleted(ApiResult result, Exception error)
        {
            // This runs on the UI thread
            Cursor.Current = Cursors.Default;
            _isConnecting = false;
            connectButton.Enabled = true;

            if (error != null)
            {
                // Handle any unhandled exceptions from the background thread
                statusBar.Text = "Connection failed";
                MessageBox.Show("Error: " + error.Message, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }
            else if (result != null)
            {
                if (result.IsSuccess)
                {
                    statusBar.Text = "Connected to Grimmory";
                }
                else
                {
                    statusBar.Text = "Failed to login";
                    MessageBox.Show("Login failed: " + result.ErrorMessage, "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                }
            }
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            // Prevent multiple simultaneous login attempts
            if (_isConnecting)
            {
                MessageBox.Show("Already connecting. Please wait...", "Info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return;
            }

            AppSettings settings = _settingsStore.Load();
            if (string.IsNullOrEmpty(settings.Server) ||
               string.IsNullOrEmpty(settings.User) ||
               string.IsNullOrEmpty(settings.Password))
            {
                MessageBox.Show("Please configure server settings first.", "Configuration Required", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return;
            }

            // Prepare parameters for background operation
            LoginParameters loginParams = LoginParameters.FromAppSettings(settings);

            // Update UI and start async operation
            _isConnecting = true;
            connectButton.Enabled = false;
            statusBar.Text = "Connecting...";
            Cursor.Current = Cursors.WaitCursor;

            // Start the background operation using ThreadPool
            ThreadPool.QueueUserWorkItem(new WaitCallback(PerformLogin), loginParams);
        }
    }
}
