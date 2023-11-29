using System;

namespace EventEmitter.NET.Utils
{
    /// <summary>
    /// A static class that can generate random JSONRPC ids using the current time as a source of randomness 
    /// </summary>
    public static class RpcPayloadId
    {
        private static readonly Random rng = new Random();
        private static readonly DateTime UnixEpoch =
            new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        /// <summary>
        /// Generate a new random JSON-RPC id. The clock is used as a source of randomness
        /// </summary>
        /// <returns>A random JSON-RPC id</returns>
        public static long Generate()
        {
            var date = (long)((DateTime.UtcNow - UnixEpoch).TotalMilliseconds) * (10L * 10L * 10L);
            var extra = (long)Math.Floor(rng.NextDouble() * (10.0 * 10.0 * 10.0));
            return date + extra;
        }
    }
}
