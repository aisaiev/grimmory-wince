using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace Grimmory
{
    // Tag Management Logic
    partial class MainForm
    {
        private bool _isUpdatingTags = false;
        private List<CheckBox> _tagCheckboxes = new List<CheckBox>();
        private List<string> _allTags = new List<string>();
        private System.Windows.Forms.Timer _tagUpdateTimer;
        private int _pendingTagBookId = -1;
        private List<string> _pendingTags = null;

        private void PopulateTagCheckboxes(List<string> allTags, BookResponse foundBook)
        {
            // Clear existing checkboxes
            foreach (CheckBox cb in _tagCheckboxes)
            {
                infoPage.Controls.Remove(cb);
                cb.CheckStateChanged -= tagCheckbox_CheckStateChanged;
                cb.Dispose();
            }
            _tagCheckboxes.Clear();

            if (allTags == null || allTags.Count == 0)
                return;

            // Get book's current tags
            List<string> bookTags = new List<string>();
            if (foundBook != null && foundBook.Metadata != null && foundBook.Metadata.Tags != null)
            {
                bookTags = foundBook.Metadata.Tags;
            }

            // Create checkboxes for each tag
            int yPosition = 25;
            foreach (string tag in allTags)
            {
                CheckBox cb = new CheckBox();
                cb.Text = tag;
                cb.Location = new Point(3, yPosition);
                cb.Size = new Size(220, 20);
                cb.Checked = bookTags.Contains(tag);
                cb.CheckStateChanged += new EventHandler(tagCheckbox_CheckStateChanged);
                
                _tagCheckboxes.Add(cb);
                infoPage.Controls.Add(cb);
                
                yPosition += 25;
            }
        }

        private void tagCheckbox_CheckStateChanged(object sender, EventArgs e)
        {
            // Ignore if no book is currently loaded
            if (_currentBookId <= 0)
                return;

            // Collect all checked tag names
            List<string> selectedTags = new List<string>();
            foreach (CheckBox cb in _tagCheckboxes)
            {
                if (cb.Checked)
                {
                    selectedTags.Add(cb.Text);
                }
            }

            _pendingTagBookId = _currentBookId;
            _pendingTags = selectedTags;
            ScheduleTagUpdateDebounced();
        }

        private void ScheduleTagUpdateDebounced()
        {
            if (_tagUpdateTimer == null)
            {
                _tagUpdateTimer = new System.Windows.Forms.Timer();
                _tagUpdateTimer.Interval = TagUpdateDebounceMs;
                _tagUpdateTimer.Tick += TagUpdateTimer_Tick;
            }

            _tagUpdateTimer.Enabled = false;
            _tagUpdateTimer.Enabled = true;
        }

        private void TagUpdateTimer_Tick(object sender, EventArgs e)
        {
            _tagUpdateTimer.Enabled = false;

            if (_isUpdatingTags)
            {
                // Keep pending changes and retry shortly.
                _tagUpdateTimer.Enabled = true;
                return;
            }

            if (_pendingTagBookId <= 0 || _pendingTags == null)
                return;

            _isUpdatingTags = true;

            object[] parameters = new object[] { _pendingTagBookId, _pendingTags };
            _pendingTagBookId = -1;
            _pendingTags = null;
            ThreadPool.QueueUserWorkItem(new WaitCallback(PerformTagUpdate), parameters);
        }

        private void PerformTagUpdate(object state)
        {
            object[] parameters = (object[])state;
            int bookId = (int)parameters[0];
            List<string> tags = (List<string>)parameters[1];
            ApiResult result = null;
            Exception error = null;

            try
            {
                result = _apiClient.UpdateBookTags(bookId, tags);
            }
            catch (Exception ex)
            {
                error = ex;
            }

            SafeUiInvoke(delegate { OnTagUpdateCompleted(result, error); });
        }

        private void OnTagUpdateCompleted(ApiResult result, Exception error)
        {
            _isUpdatingTags = false;

            if (error != null)
            {
                MessageBox.Show("Error updating tags: " + error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }
            else if (result != null && !result.IsSuccess)
            {
                MessageBox.Show("Failed to update tags: " + result.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }

            // Flush any changes that happened while an update was in flight.
            if (_pendingTagBookId > 0 && _pendingTags != null)
            {
                ScheduleTagUpdateDebounced();
            }
        }
    }
}
