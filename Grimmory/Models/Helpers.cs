using System;
using Grimmory.Settings;

namespace Grimmory.Models
{
    /// <summary>
    /// Helper class for passing login parameters to background threads
    /// </summary>
    internal class LoginParameters
    {
        public string Server { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        /// <summary>
        /// Create LoginParameters from AppSettings
        /// </summary>
        public static LoginParameters FromAppSettings(AppSettings settings)
        {
            return new LoginParameters
            {
                Server = settings.Server,
                Username = settings.User,
                Password = settings.Password
            };
        }
    }
}
