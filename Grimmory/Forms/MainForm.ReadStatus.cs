using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using Grimmory.Models;

namespace Grimmory.Forms
{
    // Read Status Management Logic
    partial class MainForm
    {
        private bool _isUpdatingStatus = false;

        private void PopulateReadStatusCombo()
        {
            string[] readStatuses = ReadStatusHelper.GetDisplayStrings();
            foreach (var item in readStatuses)
            {
                readStatusCombobox.Items.Add(item);
            }
            if (readStatusCombobox.Items.Count > 0)
                readStatusCombobox.SelectedIndex = 0;
        }

        private void readStatusCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Ignore events during form initialization
            if (_isInitializing)
                return;

            // Ignore if no book is currently loaded
            if (_currentBookId <= 0)
                return;

            // Ignore if already updating status
            if (_isUpdatingStatus)
                return;

            if (readStatusCombobox.SelectedIndex >= 0)
            {
                string selectedDisplay = readStatusCombobox.SelectedItem.ToString();
                ReadStatus selectedStatus = ReadStatusHelper.FromDisplay(selectedDisplay);
                string serverStatus = ReadStatusHelper.ToServerValue(selectedStatus);

                // Update status on server
                _isUpdatingStatus = true;
                readStatusCombobox.Enabled = false;

                // Create parameters object
                object[] parameters = new object[] { _currentBookId, serverStatus };
                ThreadPool.QueueUserWorkItem(new WaitCallback(PerformStatusUpdate), parameters);
            }
        }

        private void PerformStatusUpdate(object state)
        {
            object[] parameters = (object[])state;
            int bookId = (int)parameters[0];
            string status = (string)parameters[1];
            ApiResult result = null;
            Exception error = null;

            try
            {
                List<int> bookIds = new List<int>();
                bookIds.Add(bookId);
                result = _apiClient.UpdateBookStatus(bookIds, status);
            }
            catch (Exception ex)
            {
                error = ex;
            }

            SafeUiInvoke(delegate { OnStatusUpdateCompleted(result, error); });
        }

        private void OnStatusUpdateCompleted(ApiResult result, Exception error)
        {
            _isUpdatingStatus = false;
            readStatusCombobox.Enabled = true;

            if (error != null)
            {
                MessageBox.Show("Error updating status: " + error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }
            else if (result != null && !result.IsSuccess)
            {
                MessageBox.Show("Failed to update status: " + result.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }

            isbnTextbox.Focus();
        }
    }
}
