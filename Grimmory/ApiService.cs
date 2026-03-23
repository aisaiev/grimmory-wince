using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace Grimmory
{
    static class ApiService
    {
        private const int RequestTimeoutMs = 10000;

        public static string AccessToken { get; set; }

        public static string ServerAddress { get; set; }

        static ApiService()
        {
            AccessToken = String.Empty;
            ServerAddress = String.Empty;
        }

        public static ApiResult Login(string username, string password)
        {
            try
            {
                HttpWebRequest request = CreateRequest("/api/v1/auth/login", "POST", false, "application/json");

                LoginRequest loginReq = new LoginRequest { Username = username, Password = password };
                WriteJsonBody(request, loginReq);

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    LoginResponse authData = ReadJsonResponse<LoginResponse>(response);
                    if (authData != null && !string.IsNullOrEmpty(authData.AccessToken))
                    {
                        AccessToken = authData.AccessToken;
                        return new ApiResult(true, null);
                    }

                    return new ApiResult(false, "Token is empty");
                }
            }
            catch (WebException ex)
            {
                return new ApiResult(false, ReadWebExceptionMessage(ex));
            }
            catch (Exception ex)
            {
                return new ApiResult(false, SanitizeErrorMessage(ex.Message));
            }
        }

        public static BooksApiResult GetAllBooks()
        {
            if (string.IsNullOrEmpty(AccessToken))
            {
                return new BooksApiResult(false, "Not authenticated", null);
            }

            try
            {
                HttpWebRequest request = CreateRequest("/api/v1/books", "GET", true, null);

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    List<BookResponse> books = ReadJsonResponse<List<BookResponse>>(response);
                    return new BooksApiResult(true, null, books ?? new List<BookResponse>());
                }
            }
            catch (WebException ex)
            {
                return new BooksApiResult(false, ReadWebExceptionMessage(ex), null);
            }
            catch (Exception ex)
            {
                return new BooksApiResult(false, SanitizeErrorMessage(ex.Message), null);
            }
        }

        public static Image GetBookCover(int bookId)
        {
            if (string.IsNullOrEmpty(AccessToken))
            {
                return null;
            }

            string encodedToken = Uri.EscapeDataString(AccessToken);
            string path = string.Format("/api/v1/media/book/{0}/cover?token={1}", bookId, encodedToken);

            try
            {
                HttpWebRequest request = CreateRequest(path, "GET", false, null);
                request.AllowAutoRedirect = false;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        return new Bitmap(stream);
                    }
                }
            }
            catch
            {
                // Return null if image unavailable
                return null;
            }
        }

        public static CreateBookApiResult CreatePhysicalBook(int libraryId, string isbn, IsbnLookupResponse metadata)
        {
            if (string.IsNullOrEmpty(AccessToken))
            {
                return new CreateBookApiResult(false, "Not authenticated", -1);
            }

            try
            {
                HttpWebRequest request = CreateRequest("/api/v1/books/physical", "POST", true, "application/json");

                CreatePhysicalBookRequest bookReq = new CreatePhysicalBookRequest
                {
                    LibraryId = libraryId,
                    Isbn = isbn
                };

                if (metadata != null)
                {
                    bookReq.Title = metadata.Title;
                    bookReq.Authors = metadata.Authors;
                    bookReq.Description = metadata.Description;
                    bookReq.Publisher = metadata.Publisher;
                    bookReq.PublishedDate = metadata.PublishedDate;
                    bookReq.Language = metadata.Language;
                    bookReq.PageCount = metadata.PageCount > 0 ? (int?)metadata.PageCount : null;
                    bookReq.Categories = metadata.Categories;
                    bookReq.ThumbnailUrl = metadata.ThumbnailUrl;
                }

                WriteJsonBody(request, bookReq);

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    CreatePhysicalBookResponse createResponse = ReadJsonResponse<CreatePhysicalBookResponse>(response);
                    if (createResponse != null && createResponse.Id > 0)
                    {
                        return new CreateBookApiResult(true, null, createResponse.Id);
                    }

                    return new CreateBookApiResult(false, "Invalid response from server", -1);
                }
            }
            catch (WebException ex)
            {
                return new CreateBookApiResult(false, ReadWebExceptionMessage(ex), -1);
            }
            catch (Exception ex)
            {
                return new CreateBookApiResult(false, SanitizeErrorMessage(ex.Message), -1);
            }
        }

        public static BookApiResult GetBookById(int bookId)
        {
            if (string.IsNullOrEmpty(AccessToken))
            {
                return new BookApiResult(false, "Not authenticated", null);
            }

            try
            {
                HttpWebRequest request = CreateRequest(string.Format("/api/v1/books/{0}", bookId), "GET", true, null);

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    BookResponse book = ReadJsonResponse<BookResponse>(response);
                    return new BookApiResult(true, null, book);
                }
            }
            catch (WebException ex)
            {
                return new BookApiResult(false, ReadWebExceptionMessage(ex), null);
            }
            catch (Exception ex)
            {
                return new BookApiResult(false, SanitizeErrorMessage(ex.Message), null);
            }
        }

        public static ApiResult UpdateBookStatus(List<int> bookIds, string status)
        {
            if (string.IsNullOrEmpty(AccessToken))
            {
                return new ApiResult(false, "Not authenticated");
            }

            try
            {
                HttpWebRequest request = CreateRequest("/api/v1/books/status", "POST", true, "application/json");

                UpdateBookStatusRequest statusReq = new UpdateBookStatusRequest
                {
                    BookIds = bookIds,
                    Status = status
                };

                WriteJsonBody(request, statusReq);

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    return new ApiResult(true, null);
                }
            }
            catch (WebException ex)
            {
                return new ApiResult(false, ReadWebExceptionMessage(ex));
            }
            catch (Exception ex)
            {
                return new ApiResult(false, SanitizeErrorMessage(ex.Message));
            }
        }

        public static IsbnLookupApiResult LookupIsbn(string isbn)
        {
            if (string.IsNullOrEmpty(AccessToken))
            {
                return new IsbnLookupApiResult(false, "Not authenticated", null);
            }

            try
            {
                HttpWebRequest request = CreateRequest("/api/v1/books/metadata/isbn-lookup", "POST", true, "application/json");

                IsbnLookupRequest lookupReq = new IsbnLookupRequest { Isbn = isbn };
                WriteJsonBody(request, lookupReq);

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    IsbnLookupResponse metadata = ReadJsonResponse<IsbnLookupResponse>(response);
                    return new IsbnLookupApiResult(true, null, metadata);
                }
            }
            catch (WebException ex)
            {
                return new IsbnLookupApiResult(false, ReadWebExceptionMessage(ex), null);
            }
            catch (Exception ex)
            {
                return new IsbnLookupApiResult(false, SanitizeErrorMessage(ex.Message), null);
            }
        }

        public static ApiResult UpdateBookTags(int bookId, List<string> tags)
        {
            if (string.IsNullOrEmpty(AccessToken))
            {
                return new ApiResult(false, "Not authenticated");
            }

            try
            {
                string path = string.Format("/api/v1/books/{0}/metadata?mergeCategories=false&replaceMode=REPLACE_WHEN_PROVIDED", bookId);
                HttpWebRequest request = CreateRequest(path, "PUT", true, "application/json");

                UpdateBookTagsRequest tagsReq = new UpdateBookTagsRequest
                {
                    Metadata = new BookMetadataUpdate { Tags = tags },
                    ClearFlags = new ClearFlags { Tags = tags.Count > 0 ? false : true }
                };

                WriteJsonBody(request, tagsReq);

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    return new ApiResult(true, null);
                }
            }
            catch (WebException ex)
            {
                return new ApiResult(false, ReadWebExceptionMessage(ex));
            }
            catch (Exception ex)
            {
                return new ApiResult(false, SanitizeErrorMessage(ex.Message));
            }
        }

        private static HttpWebRequest CreateRequest(string path, string method, bool requiresAuth, string contentType)
        {
            string url = string.Format("http://{0}{1}", ServerAddress, path);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = method;
            request.Timeout = RequestTimeoutMs;

            if (!string.IsNullOrEmpty(contentType))
            {
                request.ContentType = contentType;
            }

            if (requiresAuth)
            {
                request.Headers.Add("Authorization", "Bearer " + AccessToken);
            }

            return request;
        }

        private static void WriteJsonBody(HttpWebRequest request, object payloadObject)
        {
            string jsonPayload = JsonConvert.SerializeObject(payloadObject);
            byte[] data = Encoding.UTF8.GetBytes(jsonPayload);
            request.ContentLength = data.Length;

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
        }

        private static T ReadJsonResponse<T>(HttpWebResponse response)
        {
            using (Stream responseStream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(responseStream))
            {
                string responseText = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(responseText);
            }
        }

        private static string ReadWebExceptionMessage(WebException ex)
        {
            try
            {
                if (ex.Response != null)
                {
                    using (Stream stream = ex.Response.GetResponseStream())
                    {
                        if (stream != null)
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                string body = reader.ReadToEnd();
                                if (!string.IsNullOrEmpty(body))
                                {
                                    return SanitizeErrorMessage(body);
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
            }

            return SanitizeErrorMessage(ex.Message);
        }

        private static string SanitizeErrorMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
                return "Request failed";

            string sanitized = message;

            sanitized = MaskTokenQueryValue(sanitized);

            if (sanitized.IndexOf("http://", StringComparison.OrdinalIgnoreCase) >= 0 ||
                sanitized.IndexOf("https://", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "Network request failed. Check server address and connection.";
            }

            return sanitized;
        }

        private static string MaskTokenQueryValue(string input)
        {
            const string tokenKey = "token=";
            int searchStart = 0;
            StringBuilder result = new StringBuilder(input);

            while (true)
            {
                int start = result.ToString().IndexOf(tokenKey, searchStart, StringComparison.OrdinalIgnoreCase);
                if (start < 0)
                    break;

                int valueStart = start + tokenKey.Length;
                int end = valueStart;
                while (end < result.Length)
                {
                    char c = result[end];
                    if (c == '&' || c == ' ' || c == '\n' || c == '\r' || c == '"' || c == '\'')
                        break;
                    end++;
                }

                result.Remove(valueStart, end - valueStart);
                result.Insert(valueStart, "***");
                searchStart = valueStart + 3;
            }

            return result.ToString();
        }
    }
}
