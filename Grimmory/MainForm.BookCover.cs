using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace Grimmory
{
    // Book Cover Loading Logic
    partial class MainForm
    {
        private void LoadBookCover(int bookId)
        {
            // Download book cover asynchronously
            ThreadPool.QueueUserWorkItem(new WaitCallback(DownloadBookCover), bookId);
        }

        private void DownloadBookCover(object state)
        {
            int bookId = (int)state;
            Image coverImage = null;

            try
            {
                coverImage = _apiClient.GetBookCover(bookId);
            }
            catch
            {
                // Ignore errors
            }

            SafeUiInvoke(delegate { OnBookCoverLoaded(coverImage); });
        }

        private void OnBookCoverLoaded(Image coverImage)
        {
            Image previousImage = pictureBox.Image;

            if (coverImage != null)
            {
                pictureBox.Image = coverImage;
                pictureLabel.Visible = false;
            }
            else
            {
                pictureBox.Image = null;
                pictureLabel.Text = "No cover";
                pictureLabel.Visible = true;
            }

            if (previousImage != null && !object.ReferenceEquals(previousImage, coverImage))
            {
                previousImage.Dispose();
            }
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            using (Pen pen = new Pen(Color.Black))
            {
                e.Graphics.DrawRectangle(pen, 0, 0, pb.Width - 1, pb.Height - 1);
            }
        }
    }
}
