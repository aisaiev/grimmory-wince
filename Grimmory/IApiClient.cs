using System.Collections.Generic;
using System.Drawing;

namespace Grimmory
{
    internal interface IApiClient
    {
        string AccessToken { get; }
        void SetServerAddress(string serverAddress);
        ApiResult Login(string username, string password);
        BooksApiResult GetAllBooks();
        Image GetBookCover(int bookId);
        CreateBookApiResult CreatePhysicalBook(int libraryId, string isbn, IsbnLookupResponse metadata);
        BookApiResult GetBookById(int bookId);
        ApiResult UpdateBookStatus(List<int> bookIds, string status);
        IsbnLookupApiResult LookupIsbn(string isbn);
        ApiResult UpdateBookTags(int bookId, List<string> tags);
    }
}
