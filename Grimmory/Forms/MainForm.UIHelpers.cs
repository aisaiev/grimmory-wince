using System;
using System.Drawing;
using System.Windows.Forms;

namespace Grimmory.Forms
{
    // UI Helper Methods
    partial class MainForm
    {
        private LoadingForm _loadingForm = null;

        private void ShowLoadingForm()
        {
            _loadingForm = new LoadingForm();
            int x = this.Left + (this.Width - _loadingForm.Width) / 2;
            int y = this.Top + (this.Height - _loadingForm.Height) / 2;
            _loadingForm.Location = new Point(x, y);
            _loadingForm.Show();
        }

        private void CloseLoadingForm()
        {
            if (_loadingForm != null)
            {
                _loadingForm.Close();
                _loadingForm.Dispose();
                _loadingForm = null;
            }
        }

        private void SetFormBusy()
        {
            isbnTextbox.Enabled = false;
            readStatusCombobox.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;
        }

        private void SetFormReady()
        {
            isbnTextbox.Enabled = true;
            readStatusCombobox.Enabled = true;
            Cursor.Current = Cursors.Default;
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex == 0)
            {
                isbnTextbox.Focus();
            }
            else if (tabControl.SelectedIndex == 2)
            {
                LoadSettings();
            }
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
            isbnTextbox.Focus();
        }

        private void MainForm_Closed(object sender, EventArgs e)
        {
            if (_autoConnectTimer != null)
            {
                _autoConnectTimer.Enabled = false;
                _autoConnectTimer.Tick -= AutoConnectTimer_Tick;
                _autoConnectTimer.Dispose();
                _autoConnectTimer = null;
            }

            if (_tagUpdateTimer != null)
            {
                _tagUpdateTimer.Enabled = false;
                _tagUpdateTimer.Tick -= TagUpdateTimer_Tick;
                _tagUpdateTimer.Dispose();
                _tagUpdateTimer = null;
            }

            CloseLoadingForm();
        }

        private void SafeUiInvoke(Action action)
        {
            try
            {
                if (this.IsDisposed)
                    return;

                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new EventHandler(delegate
                    {
                        action();
                    }));
                }
                else
                {
                    action();
                }
            }
            catch (ObjectDisposedException)
            {
            }
            catch (InvalidOperationException)
            {
            }
        }
    }
}
