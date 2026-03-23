using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Grimmory
{
    /// <summary>
    /// Login response model
    /// </summary>
    public class LoginResponse
    {
        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }

        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }

        [JsonProperty("isDefaultPassword")]
        public string IsDefaultPassword { get; set; }
    }

    /// <summary>
    /// Create physical book response model
    /// </summary>
    public class CreatePhysicalBookResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }
    }

    /// <summary>
    /// Book search response model
    /// </summary>
    public class BookResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("metadata")]
        public BookMetadata Metadata { get; set; }

        [JsonProperty("readStatus")]
        public string ReadStatus { get; set; }
    }

    /// <summary>
    /// Book metadata model
    /// </summary>
    public class BookMetadata
    {
        [JsonProperty("bookId")]
        public int BookId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("isbn13")]
        public string Isbn13 { get; set; }

        [JsonProperty("isbn10")]
        public string Isbn10 { get; set; }

        [JsonProperty("authors")]
        public List<string> Authors { get; set; }

        [JsonProperty("tags")]
        public List<string> Tags { get; set; }
    }

    /// <summary>
    /// ISBN lookup response model
    /// </summary>
    public class IsbnLookupResponse
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("publisher")]
        public string Publisher { get; set; }

        [JsonProperty("publishedDate")]
        public string PublishedDate { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("isbn13")]
        public string Isbn13 { get; set; }

        [JsonProperty("isbn10")]
        public string Isbn10 { get; set; }

        [JsonProperty("pageCount")]
        public int PageCount { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("goodreadsId")]
        public string GoodreadsId { get; set; }

        [JsonProperty("goodreadsRating")]
        public double GoodreadsRating { get; set; }

        [JsonProperty("goodreadsReviewCount")]
        public int GoodreadsReviewCount { get; set; }

        [JsonProperty("authors")]
        public List<string> Authors { get; set; }

        [JsonProperty("categories")]
        public List<string> Categories { get; set; }

        [JsonProperty("provider")]
        public string Provider { get; set; }

        [JsonProperty("thumbnailUrl")]
        public string ThumbnailUrl { get; set; }
    }
}
