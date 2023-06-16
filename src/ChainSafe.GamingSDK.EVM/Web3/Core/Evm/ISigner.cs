using System.Collections.Generic;
using System.Threading.Tasks;
using Nethereum.ABI.EIP712;
using Nethereum.Hex.HexConvertors.Extensions;

namespace Web3Unity.Scripts.Library.Ethers.Signers
{
    public interface ISigner
    {
        Task<string> GetAddress();

        Task<string> SignMessage(string message);

        Task<string> SignTypedData(Domain domain, Dictionary<string, MemberDescription[]> types, MemberValue[] message);

        Task<string> SignMessage(byte[] message) => SignMessage(message.ToHex());
    }
}