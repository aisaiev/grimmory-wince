using System;

namespace Grimmory
{
    /// <summary>
    /// Read status enumeration
    /// </summary>
    public enum ReadStatus
    {
        UNSET,
        UNREAD,
        PAUSED,
        READING,
        RE_READING,
        READ,
        PARTIALLY_READ,
        ABANDONED,
        WONT_READ
    }

    /// <summary>
    /// Helper class for converting between ReadStatus enum values and display/server strings
    /// </summary>
    public static class ReadStatusHelper
    {
        /// <summary>
        /// Convert ReadStatus to user-friendly display string
        /// </summary>
        public static string ToDisplay(ReadStatus s)
        {
            switch (s)
            {
                case ReadStatus.UNSET: return "Unset";
                case ReadStatus.UNREAD: return "Unread";
                case ReadStatus.PAUSED: return "Paused";
                case ReadStatus.READING: return "Reading";
                case ReadStatus.RE_READING: return "Re-reading";
                case ReadStatus.READ: return "Read";
                case ReadStatus.PARTIALLY_READ: return "Partially Read";
                case ReadStatus.ABANDONED: return "Abandoned";
                case ReadStatus.WONT_READ: return "Won't Read";
                default: return s.ToString();
            }
        }

        /// <summary>
        /// Convert ReadStatus to server-side enum/string value (e.g. "READING")
        /// </summary>
        public static string ToServerValue(ReadStatus s)
        {
            switch (s)
            {
                case ReadStatus.UNSET: return "UNSET";
                case ReadStatus.UNREAD: return "UNREAD";
                case ReadStatus.PAUSED: return "PAUSED";
                case ReadStatus.READING: return "READING";
                case ReadStatus.RE_READING: return "RE_READING";
                case ReadStatus.READ: return "READ";
                case ReadStatus.PARTIALLY_READ: return "PARTIALLY_READ";
                case ReadStatus.ABANDONED: return "ABANDONED";
                case ReadStatus.WONT_READ: return "WONT_READ";
                default: return "UNSET";
            }
        }

        /// <summary>
        /// Convert display string to ReadStatus enum
        /// </summary>
        public static ReadStatus FromDisplay(string display)
        {
            switch (display)
            {
                case "Unset": return ReadStatus.UNSET;
                case "Unread": return ReadStatus.UNREAD;
                case "Paused": return ReadStatus.PAUSED;
                case "Reading": return ReadStatus.READING;
                case "Re-reading": return ReadStatus.RE_READING;
                case "Read": return ReadStatus.READ;
                case "Partially Read": return ReadStatus.PARTIALLY_READ;
                case "Abandoned": return ReadStatus.ABANDONED;
                case "Won't Read": return ReadStatus.WONT_READ;
                default: return ReadStatus.UNSET;
            }
        }

        /// <summary>
        /// Get all display strings for UI dropdowns
        /// </summary>
        public static string[] GetDisplayStrings()
        {
            ReadStatus[] values = new ReadStatus[] {
                    ReadStatus.UNSET,
                    ReadStatus.UNREAD,
                    ReadStatus.PAUSED,
                    ReadStatus.READING,
                    ReadStatus.RE_READING,
                    ReadStatus.READ,
                    ReadStatus.PARTIALLY_READ,
                    ReadStatus.ABANDONED,
                    ReadStatus.WONT_READ
                };
            string[] res = new string[values.Length];
            for (int i = 0; i < values.Length; i++)
                res[i] = ToDisplay(values[i]);
            return res;
        }

        /// <summary>
        /// Convert server-side enum/string value (e.g. "READ") to local ReadStatus
        /// </summary>
        public static ReadStatus FromServerValue(string serverValue)
        {
            if (string.IsNullOrEmpty(serverValue))
                return ReadStatus.UNSET;

            switch (serverValue.ToUpper())
            {
                case "UNSET": return ReadStatus.UNSET;
                case "UNREAD": return ReadStatus.UNREAD;
                case "PAUSED": return ReadStatus.PAUSED;
                case "READING": return ReadStatus.READING;
                case "RE-READING":
                case "REREADING":
                case "RE_READING": return ReadStatus.RE_READING;
                case "READ": return ReadStatus.READ;
                case "PARTIALLY_READ":
                case "PARTIALLYREAD":
                case "PARTIALLY READ": return ReadStatus.PARTIALLY_READ;
                case "ABANDONED": return ReadStatus.ABANDONED;
                case "WONT_READ":
                case "WON'T READ":
                case "WON'T_READ":
                case "WONTREAD": return ReadStatus.WONT_READ;
                default: return ReadStatus.UNSET;
            }
        }
    }
}
