using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Grimmory.Models;
using Grimmory.Settings;

namespace Grimmory.Forms
{
    // Book Search and Creation Logic
    partial class MainForm
    {
        private bool _isSearching = false;
        private bool _isCreating = false;
        private int _currentBookId = -1;

        private static string NormalizeIsbn(string isbnValue)
        {
            if (string.IsNullOrEmpty(isbnValue))
                return string.Empty;

            StringBuilder normalized = new StringBuilder(isbnValue.Length);
            for (int i = 0; i < isbnValue.Length; i++)
            {
                char c = isbnValue[i];
                if (char.IsWhiteSpace(c) || c == '-')
                    continue;

                normalized.Append(c);
            }

            return normalized.ToString();
        }

        private void isbnTextbox_GotFocus(object sender, EventArgs e)
        {
            // Select all text when focused - this allows scanner to replace it
            // while keeping it visible for the user to see what was scanned
            if (!string.IsNullOrEmpty(isbnTextbox.Text))
            {
                isbnTextbox.SelectAll();
            }
        }

        private void isbnTextbox_KeyUp(object sender, KeyEventArgs e)
        {
            // Scanner button sends F2 key (132) - trigger search when that is detected
            if ((int)e.KeyCode == ScannerF2KeyCode)
            {
                string isbn = NormalizeIsbn(isbnTextbox.Text);

                if (string.IsNullOrEmpty(_apiClient.AccessToken))
                {
                    MessageBox.Show("Please connect to server first", "Not Connected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    isbnTextbox.Text = string.Empty;
                    isbnTextbox.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(isbn))
                {
                    MessageBox.Show("Please enter an ISBN", "Info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    isbnTextbox.Focus();
                    return;
                }

                if (_isSearching || _isCreating)
                {
                    return;
                }

                _isSearching = true;
                SetFormBusy();

                ShowLoadingForm();

                // Start async search
                ThreadPool.QueueUserWorkItem(new WaitCallback(SearchBookByIsbn), isbn);
            }
        }

        private void SearchBookByIsbn(object state)
        {
            string isbn = (string)state;
            BooksApiResult result = null;
            BookResponse foundBook = null;
            List<string> allTags = new List<string>();
            Exception error = null;

            try
            {
                result = _apiClient.GetAllBooks();

                if (result != null && result.IsSuccess && result.Books != null)
                {
                    // Search for book by ISBN (either isbn13 or isbn10)
                    foundBook = result.Books.FirstOrDefault(book =>
                        book.Metadata != null &&
                        (NormalizeIsbn(book.Metadata.Isbn13) == isbn || NormalizeIsbn(book.Metadata.Isbn10) == isbn)
                    );

                    allTags = result.Books
                        .Where(book => book.Metadata != null && book.Metadata.Tags != null)
                        .SelectMany(book => book.Metadata.Tags)
                        .Distinct()
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                error = ex;
            }

            SafeUiInvoke(delegate { OnBookSearchCompleted(result, foundBook, allTags, error); });
        }

        private void OnBookSearchCompleted(BooksApiResult result, BookResponse foundBook, List<string> allTags, Exception error)
        {
            CloseLoadingForm();

            _isSearching = false;
            SetFormReady();


            if (error != null)
            {
                MessageBox.Show("Error: " + error.Message, "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }
            else if (result != null)
            {
                if (!result.IsSuccess)
                {
                    MessageBox.Show("Failed to get books: " + result.ErrorMessage, "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                }
                else if (foundBook != null)
                {
                    // Store current book ID for status updates
                    _currentBookId = foundBook.Id;

                    // Fill the form fields with book data
                    titleTextbox.Text = foundBook.Metadata.Title ?? "";

                    string authorsStr = foundBook.Metadata.Authors != null && foundBook.Metadata.Authors.Count > 0
                        ? string.Join(", ", foundBook.Metadata.Authors.ToArray())
                        : "";
                    authorsTextbox.Text = authorsStr;

                    // Set read status from book data
                    ReadStatus status = ReadStatusHelper.FromServerValue(foundBook.ReadStatus);
                    string displayStatus = ReadStatusHelper.ToDisplay(status);
                    int statusIndex = readStatusCombobox.Items.IndexOf(displayStatus);
                    if (statusIndex >= 0)
                        readStatusCombobox.SelectedIndex = statusIndex;

                    // Load book cover image
                    pictureLabel.Text = "Loading";
                    pictureLabel.Visible = true;
                    LoadBookCover(foundBook.Id);

                    // Populate tags
                    _allTags = allTags;
                    PopulateTagCheckboxes(allTags, foundBook);
                }
                else
                {
                    // Clear current book ID
                    _currentBookId = -1;

                    titleTextbox.Text = string.Empty;
                    authorsTextbox.Text = string.Empty;
                    int unsetIndex = readStatusCombobox.Items.IndexOf("Unset");
                    if (unsetIndex >= 0)
                        readStatusCombobox.SelectedIndex = unsetIndex;

                    // Clear book cover
                    if (pictureBox.Image != null)
                    {
                        pictureBox.Image.Dispose();
                        pictureBox.Image = null;
                    }
                    pictureLabel.Text = "No cover";
                    pictureLabel.Visible = true;

                    // Populate tags for book not found case
                    _allTags = allTags;
                    PopulateTagCheckboxes(allTags, null);

                    DialogResult bookNotFoundPromptResult = MessageBox.Show("Book not found. Do you want to add it?", "Not Found", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    if (bookNotFoundPromptResult == DialogResult.Yes)
                    {
                        _isCreating = true;
                        SetFormBusy();

                        ShowLoadingForm();

                        CreatePhysicalBook(NormalizeIsbn(isbnTextbox.Text));
                        return; // Don't refocus yet - let OnBookCreated handle it after book is created
                    }
                }
            }

            // Refocus after search
            isbnTextbox.Focus();
        }

        private void CreatePhysicalBook(string isbn)
        {
            // Create book asynchronously
            ThreadPool.QueueUserWorkItem(new WaitCallback(PerformCreateBook), isbn);
        }

        private void PerformCreateBook(object state)
        {
            string isbn = (string)state;
            CreateBookApiResult createResult = null;
            BookApiResult bookResult = null;
            Exception error = null;
            IsbnLookupResponse metadata = null;

            try
            {
                // Check if ISBN lookup is enabled in settings
                AppSettings settings = _settingsStore.Load();
                
                if (settings.IsbnLookup)
                {
                    // Perform ISBN lookup first
                    IsbnLookupApiResult lookupResult = _apiClient.LookupIsbn(isbn);
                    
                    if (lookupResult != null && lookupResult.IsSuccess)
                    {
                        metadata = lookupResult.Metadata;
                    }
                    // Continue even if lookup fails - will create book with just ISBN
                }
                
                // Create physical book with or without metadata
                createResult = _apiClient.CreatePhysicalBook(DefaultLibraryId, isbn, metadata);
                
                // If book was created successfully, fetch the book details
                if (createResult != null && createResult.IsSuccess && createResult.BookId > 0)
                {
                    bookResult = _apiClient.GetBookById(createResult.BookId);
                }
            }
            catch (Exception ex)
            {
                error = ex;
            }

            SafeUiInvoke(delegate { OnBookCreated(createResult, bookResult, error); });
        }

        private void OnBookCreated(CreateBookApiResult createResult, BookApiResult bookResult, Exception error)
        {
            CloseLoadingForm();

            SetFormReady();
            
            if (error != null)
            {
                MessageBox.Show("Error creating book: " + error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }
            else if (createResult != null)
            {
                if (createResult.IsSuccess)
                {
                    MessageBox.Show("Book added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                    
                    // Display the created book data if available
                    if (bookResult != null && bookResult.IsSuccess && bookResult.Book != null)
                    {
                        BookResponse book = bookResult.Book;
                        
                        // Store current book ID for status updates
                        _currentBookId = book.Id;

                        // Fill the form fields with book data
                        titleTextbox.Text = book.Metadata != null ? (book.Metadata.Title ?? String.Empty) : String.Empty;

                        string authorsStr = String.Empty;
                        if (book.Metadata != null && book.Metadata.Authors != null && book.Metadata.Authors.Count > 0)
                        {
                            authorsStr = string.Join(", ", book.Metadata.Authors.ToArray());
                        }
                        authorsTextbox.Text = authorsStr;

                        // Set read status from book data
                        ReadStatus status = ReadStatusHelper.FromServerValue(book.ReadStatus);
                        string displayStatus = ReadStatusHelper.ToDisplay(status);
                        int statusIndex = readStatusCombobox.Items.IndexOf(displayStatus);
                        if (statusIndex >= 0)
                            readStatusCombobox.SelectedIndex = statusIndex;

                        // Load book cover image
                        pictureLabel.Text = "Loading";
                        pictureLabel.Visible = true;
                        LoadBookCover(book.Id);
                    }
                }
                else
                {
                    MessageBox.Show("Failed to add book: " + createResult.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                }
            }
            
            // Clear flag after all messageboxes are closed
            _isCreating = false;
            
            // Refocus and clear after book creation
            isbnTextbox.Focus();
        }
    }
}
