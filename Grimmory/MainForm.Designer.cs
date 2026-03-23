namespace Grimmory
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.statusBar = new System.Windows.Forms.StatusBar();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.mainPage = new System.Windows.Forms.TabPage();
            this.pictureLabel = new System.Windows.Forms.Label();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.readStatusCombobox = new System.Windows.Forms.ComboBox();
            this.isbnLabel = new System.Windows.Forms.Label();
            this.readStatusLabel = new System.Windows.Forms.Label();
            this.isbnTextbox = new System.Windows.Forms.TextBox();
            this.authorsTextbox = new System.Windows.Forms.TextBox();
            this.titleLabel = new System.Windows.Forms.Label();
            this.authorsLabel = new System.Windows.Forms.Label();
            this.titleTextbox = new System.Windows.Forms.TextBox();
            this.infoPage = new System.Windows.Forms.TabPage();
            this.settingsPage = new System.Windows.Forms.TabPage();
            this.connectButton = new System.Windows.Forms.Button();
            this.autoConnectCheckbox = new System.Windows.Forms.CheckBox();
            this.isbnLookupCheckbox = new System.Windows.Forms.CheckBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.settingsPasswordTextbox = new System.Windows.Forms.TextBox();
            this.settingsPasswordLabel = new System.Windows.Forms.Label();
            this.settingsUserTextbox = new System.Windows.Forms.TextBox();
            this.settingsUserLabel = new System.Windows.Forms.Label();
            this.settingsServerTexbox = new System.Windows.Forms.TextBox();
            this.settingsServerLabel = new System.Windows.Forms.Label();
            this.tagsLabel = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.mainPage.SuspendLayout();
            this.infoPage.SuspendLayout();
            this.settingsPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusBar
            // 
            this.statusBar.Location = new System.Drawing.Point(0, 271);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(238, 24);
            this.statusBar.Text = "Disconnected";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.mainPage);
            this.tabControl.Controls.Add(this.infoPage);
            this.tabControl.Controls.Add(this.settingsPage);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(238, 271);
            this.tabControl.TabIndex = 4;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // mainPage
            // 
            this.mainPage.Controls.Add(this.pictureLabel);
            this.mainPage.Controls.Add(this.pictureBox);
            this.mainPage.Controls.Add(this.readStatusCombobox);
            this.mainPage.Controls.Add(this.isbnLabel);
            this.mainPage.Controls.Add(this.readStatusLabel);
            this.mainPage.Controls.Add(this.isbnTextbox);
            this.mainPage.Controls.Add(this.authorsTextbox);
            this.mainPage.Controls.Add(this.titleLabel);
            this.mainPage.Controls.Add(this.authorsLabel);
            this.mainPage.Controls.Add(this.titleTextbox);
            this.mainPage.Location = new System.Drawing.Point(4, 25);
            this.mainPage.Name = "mainPage";
            this.mainPage.Size = new System.Drawing.Size(230, 242);
            this.mainPage.Text = "Main";
            // 
            // pictureLabel
            // 
            this.pictureLabel.Location = new System.Drawing.Point(164, 40);
            this.pictureLabel.Name = "pictureLabel";
            this.pictureLabel.Size = new System.Drawing.Size(55, 20);
            this.pictureLabel.Text = "No cover";
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(156, 3);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(71, 95);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox_Paint);
            // 
            // readStatusCombobox
            // 
            this.readStatusCombobox.Location = new System.Drawing.Point(3, 173);
            this.readStatusCombobox.Name = "readStatusCombobox";
            this.readStatusCombobox.Size = new System.Drawing.Size(224, 23);
            this.readStatusCombobox.TabIndex = 14;
            this.readStatusCombobox.SelectedIndexChanged += new System.EventHandler(this.readStatusCombobox_SelectedIndexChanged);
            // 
            // isbnLabel
            // 
            this.isbnLabel.Location = new System.Drawing.Point(3, 3);
            this.isbnLabel.Name = "isbnLabel";
            this.isbnLabel.Size = new System.Drawing.Size(100, 20);
            this.isbnLabel.Text = "ISBN";
            // 
            // readStatusLabel
            // 
            this.readStatusLabel.Location = new System.Drawing.Point(3, 150);
            this.readStatusLabel.Name = "readStatusLabel";
            this.readStatusLabel.Size = new System.Drawing.Size(100, 20);
            this.readStatusLabel.Text = "Read Status";
            // 
            // isbnTextbox
            // 
            this.isbnTextbox.Location = new System.Drawing.Point(3, 26);
            this.isbnTextbox.Name = "isbnTextbox";
            this.isbnTextbox.Size = new System.Drawing.Size(147, 23);
            this.isbnTextbox.TabIndex = 10;
            this.isbnTextbox.GotFocus += new System.EventHandler(this.isbnTextbox_GotFocus);
            this.isbnTextbox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.isbnTextbox_KeyUp);
            // 
            // authorsTextbox
            // 
            this.authorsTextbox.Enabled = false;
            this.authorsTextbox.Location = new System.Drawing.Point(3, 75);
            this.authorsTextbox.Name = "authorsTextbox";
            this.authorsTextbox.Size = new System.Drawing.Size(147, 23);
            this.authorsTextbox.TabIndex = 13;
            // 
            // titleLabel
            // 
            this.titleLabel.Location = new System.Drawing.Point(3, 101);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(51, 20);
            this.titleLabel.Text = "Title";
            // 
            // authorsLabel
            // 
            this.authorsLabel.Location = new System.Drawing.Point(3, 52);
            this.authorsLabel.Name = "authorsLabel";
            this.authorsLabel.Size = new System.Drawing.Size(100, 20);
            this.authorsLabel.Text = "Authors";
            // 
            // titleTextbox
            // 
            this.titleTextbox.Enabled = false;
            this.titleTextbox.Location = new System.Drawing.Point(3, 124);
            this.titleTextbox.Name = "titleTextbox";
            this.titleTextbox.Size = new System.Drawing.Size(224, 23);
            this.titleTextbox.TabIndex = 12;
            // 
            // infoPage
            // 
            this.infoPage.AutoScroll = true;
            this.infoPage.Controls.Add(this.tagsLabel);
            this.infoPage.Location = new System.Drawing.Point(4, 25);
            this.infoPage.Name = "infoPage";
            this.infoPage.Size = new System.Drawing.Size(230, 242);
            this.infoPage.Text = "Info";
            // 
            // settingsPage
            // 
            this.settingsPage.Controls.Add(this.connectButton);
            this.settingsPage.Controls.Add(this.autoConnectCheckbox);
            this.settingsPage.Controls.Add(this.isbnLookupCheckbox);
            this.settingsPage.Controls.Add(this.saveButton);
            this.settingsPage.Controls.Add(this.settingsPasswordTextbox);
            this.settingsPage.Controls.Add(this.settingsPasswordLabel);
            this.settingsPage.Controls.Add(this.settingsUserTextbox);
            this.settingsPage.Controls.Add(this.settingsUserLabel);
            this.settingsPage.Controls.Add(this.settingsServerTexbox);
            this.settingsPage.Controls.Add(this.settingsServerLabel);
            this.settingsPage.Location = new System.Drawing.Point(4, 25);
            this.settingsPage.Name = "settingsPage";
            this.settingsPage.Size = new System.Drawing.Size(230, 242);
            this.settingsPage.Text = "Settings";
            // 
            // connectButton
            // 
            this.connectButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.connectButton.Location = new System.Drawing.Point(0, 202);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(230, 20);
            this.connectButton.TabIndex = 23;
            this.connectButton.Text = "Connect";
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // autoConnectCheckbox
            // 
            this.autoConnectCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.autoConnectCheckbox.Location = new System.Drawing.Point(3, 150);
            this.autoConnectCheckbox.Name = "autoConnectCheckbox";
            this.autoConnectCheckbox.Size = new System.Drawing.Size(109, 20);
            this.autoConnectCheckbox.TabIndex = 19;
            this.autoConnectCheckbox.Text = "Autoconnect";
            // 
            // isbnLookupCheckbox
            // 
            this.isbnLookupCheckbox.Location = new System.Drawing.Point(118, 150);
            this.isbnLookupCheckbox.Name = "isbnLookupCheckbox";
            this.isbnLookupCheckbox.Size = new System.Drawing.Size(107, 20);
            this.isbnLookupCheckbox.TabIndex = 18;
            this.isbnLookupCheckbox.Text = "ISBN Lookup";
            // 
            // saveButton
            // 
            this.saveButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.saveButton.Location = new System.Drawing.Point(0, 222);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(230, 20);
            this.saveButton.TabIndex = 17;
            this.saveButton.Text = "Save";
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // settingsPasswordTextbox
            // 
            this.settingsPasswordTextbox.Location = new System.Drawing.Point(3, 121);
            this.settingsPasswordTextbox.Name = "settingsPasswordTextbox";
            this.settingsPasswordTextbox.PasswordChar = '*';
            this.settingsPasswordTextbox.Size = new System.Drawing.Size(224, 23);
            this.settingsPasswordTextbox.TabIndex = 16;
            // 
            // settingsPasswordLabel
            // 
            this.settingsPasswordLabel.Location = new System.Drawing.Point(3, 98);
            this.settingsPasswordLabel.Name = "settingsPasswordLabel";
            this.settingsPasswordLabel.Size = new System.Drawing.Size(73, 20);
            this.settingsPasswordLabel.Text = "Password";
            // 
            // settingsUserTextbox
            // 
            this.settingsUserTextbox.Location = new System.Drawing.Point(3, 72);
            this.settingsUserTextbox.Name = "settingsUserTextbox";
            this.settingsUserTextbox.Size = new System.Drawing.Size(224, 23);
            this.settingsUserTextbox.TabIndex = 15;
            // 
            // settingsUserLabel
            // 
            this.settingsUserLabel.Location = new System.Drawing.Point(3, 49);
            this.settingsUserLabel.Name = "settingsUserLabel";
            this.settingsUserLabel.Size = new System.Drawing.Size(54, 20);
            this.settingsUserLabel.Text = "User";
            // 
            // settingsServerTexbox
            // 
            this.settingsServerTexbox.Location = new System.Drawing.Point(3, 23);
            this.settingsServerTexbox.Name = "settingsServerTexbox";
            this.settingsServerTexbox.Size = new System.Drawing.Size(224, 23);
            this.settingsServerTexbox.TabIndex = 14;
            // 
            // settingsServerLabel
            // 
            this.settingsServerLabel.Location = new System.Drawing.Point(3, 0);
            this.settingsServerLabel.Name = "settingsServerLabel";
            this.settingsServerLabel.Size = new System.Drawing.Size(54, 20);
            this.settingsServerLabel.Text = "Server";
            // 
            // tagsLabel
            // 
            this.tagsLabel.Location = new System.Drawing.Point(0, 0);
            this.tagsLabel.Name = "tagsLabel";
            this.tagsLabel.Size = new System.Drawing.Size(50, 20);
            this.tagsLabel.Text = "Tags";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(238, 295);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.statusBar);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Grimmory";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.tabControl.ResumeLayout(false);
            this.mainPage.ResumeLayout(false);
            this.infoPage.ResumeLayout(false);
            this.settingsPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.StatusBar statusBar;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage mainPage;
        private System.Windows.Forms.Label pictureLabel;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.ComboBox readStatusCombobox;
        private System.Windows.Forms.Label isbnLabel;
        private System.Windows.Forms.Label readStatusLabel;
        private System.Windows.Forms.TextBox isbnTextbox;
        private System.Windows.Forms.TextBox authorsTextbox;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label authorsLabel;
        private System.Windows.Forms.TextBox titleTextbox;
        private System.Windows.Forms.TabPage settingsPage;
        private System.Windows.Forms.CheckBox autoConnectCheckbox;
        private System.Windows.Forms.CheckBox isbnLookupCheckbox;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.TextBox settingsPasswordTextbox;
        private System.Windows.Forms.Label settingsPasswordLabel;
        private System.Windows.Forms.TextBox settingsUserTextbox;
        private System.Windows.Forms.Label settingsUserLabel;
        private System.Windows.Forms.TextBox settingsServerTexbox;
        private System.Windows.Forms.Label settingsServerLabel;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.TabPage infoPage;
        private System.Windows.Forms.Label tagsLabel;
    }
}

