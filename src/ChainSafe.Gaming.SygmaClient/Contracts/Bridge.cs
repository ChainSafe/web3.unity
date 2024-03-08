using System.IO;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Contracts;

namespace ChainSafe.Gaming.SygmaClient.Dto
{
    public class Bridge
    {
        private const string MethodDomainResourceIDToHandlerAddress = "_resourceIDToHandlerAddress";
        private const string BridgeAbi =
            "[{  \"inputs\": [],  \"name\": \"_MPCAddress\",  \"outputs\": [ {  \"internalType\": \"address\",  \"name\": \"\",  \"type\": \"address\" }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"_accessControl\",  \"outputs\": [ {  \"internalType\": \"contract IAccessControlSegregator\",  \"name\": \"\",  \"type\": \"address\" }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [ {  \"internalType\": \"uint8\",  \"name\": \"\",  \"type\": \"uint8\" }  ],  \"name\": \"_depositCounts\",  \"outputs\": [ {  \"internalType\": \"uint64\",  \"name\": \"\",  \"type\": \"uint64\" }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"_domainID\",  \"outputs\": [ {  \"internalType\": \"uint8\",  \"name\": \"\",  \"type\": \"uint8\" }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"_feeHandler\",  \"outputs\": [ {  \"internalType\": \"contract IFeeHandler\",  \"name\": \"\",  \"type\": \"address\" }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [ {  \"internalType\": \"bytes32\",  \"name\": \"\",  \"type\": \"bytes32\" }  ],  \"name\": \"_resourceIDToHandlerAddress\",  \"outputs\": [ {  \"internalType\": \"address\",  \"name\": \"\",  \"type\": \"address\" }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [ {  \"internalType\": \"address\",  \"name\": \"\",  \"type\": \"address\" }  ],  \"name\": \"isValidForwarder\",  \"outputs\": [ {  \"internalType\": \"bool\",  \"name\": \"\",  \"type\": \"bool\" }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"paused\",  \"outputs\": [ {  \"internalType\": \"bool\",  \"name\": \"\",  \"type\": \"bool\" }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [ {  \"internalType\": \"uint8\",  \"name\": \"\",  \"type\": \"uint8\" }, {  \"internalType\": \"uint256\",  \"name\": \"\",  \"type\": \"uint256\" }  ],  \"name\": \"usedNonces\",  \"outputs\": [ {  \"internalType\": \"uint256\",  \"name\": \"\",  \"type\": \"uint256\" }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [ {  \"components\": [     {      \"internalType\": \"uint8\",      \"name\": \"originDomainID\",      \"type\": \"uint8\"     },     {      \"internalType\": \"uint64\",      \"name\": \"depositNonce\",      \"type\": \"uint64\"     },     {      \"internalType\": \"bytes32\",      \"name\": \"resourceID\",      \"type\": \"bytes32\"     },     {      \"internalType\": \"bytes\",      \"name\": \"data\",      \"type\": \"bytes\"     }      ],  \"internalType\": \"struct Bridge.Proposal[]\",  \"name\": \"proposals\",  \"type\": \"tuple[]\" }, {  \"internalType\": \"bytes\",  \"name\": \"signature\",  \"type\": \"bytes\" }  ],  \"name\": \"verify\",  \"outputs\": [ {  \"internalType\": \"bool\",  \"name\": \"\",  \"type\": \"bool\" }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"} ]";

        public Bridge(IContractBuilder contractBuilder, string address)
        {
            this.Contract = contractBuilder.Build(BridgeAbi, address);
        }

        public Contract Contract { get; }

        public Task<string> DomainResourceIDToHandlerAddress(string resourceID)
        {
            var result = this.Contract.Call(MethodDomainResourceIDToHandlerAddress, new object[] { resourceID });
            return Task.FromResult(result.ToString());
        }
    }
}