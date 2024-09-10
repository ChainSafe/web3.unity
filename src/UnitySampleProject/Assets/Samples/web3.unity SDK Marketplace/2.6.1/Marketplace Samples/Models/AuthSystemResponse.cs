namespace ChainSafe.Gaming.Marketplace.Models
{
    /// <summary>
    /// Help with auth system response deserialization.
    /// </summary>
    public class AuthSystemResponse
    {
        /// <summary>
        /// Auth response class.
        /// </summary>
        public class AuthResponse
        {
            public string token;
            public string expires;
        }

        /// <summary>
        /// Login response class.
        /// </summary>
        public class LoginResponse
        {
            public AccessToken access_token;
            public RefreshToken refresh_token;
        }

        /// <summary>
        /// Access token response class.
        /// </summary>
        public class AccessToken
        {
            public string token;
            public string expires;
        }

        /// <summary>
        /// Refresh token response class.
        /// </summary>
        public class RefreshToken
        {
            public string token;
            public string expires;
        }
    }
}
