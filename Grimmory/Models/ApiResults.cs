using System;
using System.Collections.Generic;

namespace Grimmory
{
    /// <summary>
    /// Base API result class for all API responses
    /// </summary>
    public class ApiResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }

        public ApiResult(bool success, string message)
        {
            this.IsSuccess = success;
            this.ErrorMessage = message;
        }
    }

    /// <summary>
    /// API result for book list queries
    /// </summary>
    public class BooksApiResult : ApiResult
    {
        public List<BookResponse> Books { get; set; }

        public BooksApiResult(bool success, string message, List<BookResponse> books)
            : base(success, message)
        {
            this.Books = books;
        }
    }

    /// <summary>
    /// API result for ISBN lookup operations
    /// </summary>
    public class IsbnLookupApiResult : ApiResult
    {
        public IsbnLookupResponse Metadata { get; set; }

        public IsbnLookupApiResult(bool success, string message, IsbnLookupResponse metadata)
            : base(success, message)
        {
            this.Metadata = metadata;
        }
    }

    /// <summary>
    /// API result for single book queries
    /// </summary>
    public class BookApiResult : ApiResult
    {
        public BookResponse Book { get; set; }

        public BookApiResult(bool success, string message, BookResponse book)
            : base(success, message)
        {
            this.Book = book;
        }
    }

    /// <summary>
    /// API result for book creation operations
    /// </summary>
    public class CreateBookApiResult : ApiResult
    {
        public int BookId { get; set; }

        public CreateBookApiResult(bool success, string message, int bookId)
            : base(success, message)
        {
            this.BookId = bookId;
        }
    }
}
