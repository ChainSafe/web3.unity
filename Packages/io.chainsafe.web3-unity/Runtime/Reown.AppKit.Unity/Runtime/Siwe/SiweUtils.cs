using System;
using System.Security.Cryptography;
using Reown.Sign.Models;
using Reown.Sign.Models.Cacao;

namespace Reown.AppKit.Unity
{
    public class SiweUtils
    {
        public static string GenerateNonce()
        {
            var nonceBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(nonceBytes);
            }

            return BitConverter.ToString(nonceBytes).Replace("-", "").ToLower();
        }

        public static string FormatMessage(SiweCreateMessageArgs args)
        {
            var cacaoPayload = CreateCacaoPayload(args);
            return cacaoPayload.FormatMessage();
        }

        public static CacaoPayload CreateCacaoPayload(SiweCreateMessageArgs args)
        {
            var payloadParams = new AuthPayloadParams(
                new[]
                {
                    args.ChainId
                },
                args.Domain,
                args.Nonce,
                args.Uri,
                args.Type,
                args.Nbf,
                args.Exp,
                args.Iat,
                args.Statement,
                args.RequestId,
                args.Resources,
                null,
                null,
                args.Version
            );

            var iss = $"did:pkh:eip155:{args.ChainId}:{args.Address}";
            var cacaoPayload = CacaoPayload.FromAuthPayloadParams(payloadParams, iss);
            return cacaoPayload;
        }
    }
}