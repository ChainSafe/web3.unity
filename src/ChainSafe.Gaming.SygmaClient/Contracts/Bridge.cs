using System.IO;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Contracts;

namespace ChainSafe.Gaming.SygmaClient.Dto
{
    public class Bridge
    {
        private const string MethodDomainResourceIDToHandlerAddress = "_resourceIDToHandlerAddress";
        private Contract contract;

        public Bridge(IContractBuilder contractBuilder, string address)
        {
            var contractAbi = File.ReadAllText("Abi/Bridge.json");
            contract = contractBuilder.Build(contractAbi, address);
        }

        public Contract Contract => contract;

        public Task<string> DomainResourceIDToHandlerAddress(string resourceID)
        {
            var result = this.contract.Call(MethodDomainResourceIDToHandlerAddress, new object[] { resourceID });
            return Task.FromResult(result.ToString());
        }
    }
}