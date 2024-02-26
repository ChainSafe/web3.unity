using System.IO;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.SygmaClient.Dto;
using ChainSafe.Gaming.SygmaClient.Types;
using Nethereum.Hex.HexTypes;

namespace ChainSafe.Gaming.SygmaClient.Contracts
{
    public class BasicFeeHandler : IBasicHandler
    {
        private const string MethodCalculateFee = "calculateFee";
        private Contract contract;
        private string address;

        public BasicFeeHandler(IContractBuilder cb, string address)
        {
            var contractAbi = File.ReadAllText("Abi/BasicFeeHandler.json");
            this.address = address;
            contract = cb.Build(contractAbi, address);
        }

        public Task<EvmFee> CalculateBasicFee(
            string sender,
            uint fromDomainID,
            uint destinationDomainID,
            string resourceID)
        {
            var result = this.contract.Call(MethodCalculateFee, new object[] { sender, fromDomainID, destinationDomainID, resourceID }).Result;
            var fee = new EvmFee(this.address, FeeHandlerType.Basic)
            {
                Fee = new HexBigInteger(result[0].ToString()),
                FeeData = result[1] as string,
            };
            return Task.FromResult(fee);
        }
    }
}