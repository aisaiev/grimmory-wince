using System;
using System.Windows.Forms;
using Grimmory.Abstractions;
using Grimmory.Services;
using Grimmory.Settings;

namespace Grimmory.Forms
{
    /// <summary>
    /// Main form for Grimmory application.
    /// - MainForm.Authentication.cs: Login and connection logic
    /// - MainForm.BookSearch.cs: ISBN search and book creation
    /// - MainForm.BookCover.cs: Book cover loading
    /// - MainForm.ReadStatus.cs: Read status management
    /// - MainForm.Tags.cs: Tag management
    /// - MainForm.Settings.cs: Settings management
    /// - MainForm.UIHelpers.cs: UI helper methods
    /// </summary>
    public partial class MainForm : Form
    {
        private const int ScannerF2KeyCode = 132;
        private const int DefaultLibraryId = 1;
        private const int AutoConnectDelayMs = 500;
        private const int TagUpdateDebounceMs = 400;

        private readonly IApiClient _apiClient;
        private readonly ISettingsStore _settingsStore;
        private bool _isInitializing = true;

        public MainForm()
            : this(new ApiClientAdapter(), new FileSettingsStore())
        {
        }

        internal MainForm(IApiClient apiClient, ISettingsStore settingsStore)
        {
            _apiClient = apiClient ?? new ApiClientAdapter();
            _settingsStore = settingsStore ?? new FileSettingsStore();

            InitializeComponent();
            this.Closed += new EventHandler(MainForm_Closed);
            PopulateReadStatusCombo();
            _isInitializing = false;
        }
    }
}
