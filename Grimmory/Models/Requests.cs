using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Grimmory
{
    /// <summary>
    /// Login request model
    /// </summary>
    public class LoginRequest
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }

    /// <summary>
    /// Create physical book request model
    /// </summary>
    public class CreatePhysicalBookRequest
    {
        [JsonProperty("libraryId")]
        public int LibraryId { get; set; }

        [JsonProperty("isbn")]
        public string Isbn { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("authors")]
        public List<string> Authors { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("publisher")]
        public string Publisher { get; set; }

        [JsonProperty("publishedDate")]
        public string PublishedDate { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("pageCount")]
        public int? PageCount { get; set; }

        [JsonProperty("categories")]
        public List<string> Categories { get; set; }

        [JsonProperty("thumbnailUrl")]
        public string ThumbnailUrl { get; set; }
    }

    /// <summary>
    /// ISBN lookup request model
    /// </summary>
    public class IsbnLookupRequest
    {
        [JsonProperty("isbn")]
        public string Isbn { get; set; }
    }

    /// <summary>
    /// Update book status request model
    /// </summary>
    public class UpdateBookStatusRequest
    {
        [JsonProperty("bookIds")]
        public List<int> BookIds { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    /// <summary>
    /// Update book tags request model
    /// </summary>
    public class UpdateBookTagsRequest
    {
        [JsonProperty("metadata")]
        public BookMetadataUpdate Metadata { get; set; }

        [JsonProperty("clearFlags")]
        public ClearFlags ClearFlags { get; set; }
    }

    /// <summary>
    /// Book metadata update model for tag updates
    /// </summary>
    public class BookMetadataUpdate
    {
        [JsonProperty("tags")]
        public List<string> Tags { get; set; }
    }

    /// <summary>
    /// Clear flags for metadata updates
    /// </summary>
    public class ClearFlags
    {
        [JsonProperty("tags")]
        public bool Tags { get; set; }
    }
}
