namespace ChainSafe.Gaming.Marketplace.Models
{
    /// <summary>
    /// Helps with auth payload serialization.
    /// </summary>
    public class AuthPayload
    {
        /// <summary>
        /// Payload class for email post request.
        /// </summary>
        public class EmailRequestPayload
        {
            public string email;
        }
    
        /// <summary>
        /// Payload class for authorization post request.
        /// </summary>
        public class AuthCodePayload
        {
            public string email;
            public string nonce;
        }
    
        /// <summary>
        /// Payload class for login request.
        /// </summary>
        public class LoginPayload
        {
            public string provider;
            public string service;
            public string token;
        }

        /// <summary>
        /// Payload class for refresh token request.
        /// </summary>
        public class RefreshPayload
        {
            public string refresh;
        }
    }
}
