using System.Collections.Generic;
using System.Drawing;
using Grimmory.Abstractions;
using Grimmory.Models;

namespace Grimmory.Services
{
    internal sealed class ApiClientAdapter : IApiClient
    {
        public string AccessToken
        {
            get { return ApiService.AccessToken; }
        }

        public void SetServerAddress(string serverAddress)
        {
            ApiService.ServerAddress = serverAddress;
        }

        public ApiResult Login(string username, string password)
        {
            return ApiService.Login(username, password);
        }

        public BooksApiResult GetAllBooks()
        {
            return ApiService.GetAllBooks();
        }

        public Image GetBookCover(int bookId)
        {
            return ApiService.GetBookCover(bookId);
        }

        public CreateBookApiResult CreatePhysicalBook(int libraryId, string isbn, IsbnLookupResponse metadata)
        {
            return ApiService.CreatePhysicalBook(libraryId, isbn, metadata);
        }

        public BookApiResult GetBookById(int bookId)
        {
            return ApiService.GetBookById(bookId);
        }

        public ApiResult UpdateBookStatus(List<int> bookIds, string status)
        {
            return ApiService.UpdateBookStatus(bookIds, status);
        }

        public IsbnLookupApiResult LookupIsbn(string isbn)
        {
            return ApiService.LookupIsbn(isbn);
        }

        public ApiResult UpdateBookTags(int bookId, List<string> tags)
        {
            return ApiService.UpdateBookTags(bookId, tags);
        }
    }
}
