using System;
using System.Collections.Generic;

namespace Scripts.EVM.Token
{
    /// <summary>
    /// A response object detailing the details of an on chain token from the targeted network.
    /// </summary>
    public class TokenResponse
    {
        public string Contract { get; set; }
        public string TokenId { get; set; }
        public string Uri { get; set; }
        public string Balance { get; set; }

        public override string ToString()
        {
            return $"{nameof(Contract)}: {Contract}, {nameof(TokenId)}: {TokenId}, {nameof(Uri)}: {Uri}, {nameof(Balance)}: {Balance}";
        }

        private sealed class TokenResponseEqualityComparer : IEqualityComparer<TokenResponse>
        {
            public bool Equals(TokenResponse x, TokenResponse y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Contract == y.Contract && x.TokenId == y.TokenId && x.Uri == y.Uri && x.Balance == y.Balance;
            }

            public int GetHashCode(TokenResponse obj)
            {
                return HashCode.Combine(obj.Contract, obj.TokenId, obj.Uri, obj.Balance);
            }
        }

        public static IEqualityComparer<TokenResponse> TokenResponseComparer { get; } = new TokenResponseEqualityComparer();
    }
}