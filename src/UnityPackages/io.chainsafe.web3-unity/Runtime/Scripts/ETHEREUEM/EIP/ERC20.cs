using ChainSafe.GamingWeb3;
using System.Numerics;
using System.Threading.Tasks;
using Web3Unity.Scripts.Library.Ethers.Contracts;


namespace Web3Unity.Scripts.Library.ETHEREUEM.EIP
{
    public class ERC20
    {

        private static readonly string Abi = ABI.ERC_20;
        /// <summary>
        /// Balance Of ERC20 Address
        /// </summary>
        /// <param name="contractAddress"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public static async Task<BigInteger> BalanceOf(Web3 web3, string contractAddress, string account)
        {
            var contract = web3.ContractBuilder.Build(Abi, contractAddress);
            var contractData = await contract.Call(EthMethod.BalanceOf, new object[]
            {
                account
            });
            return BigInteger.Parse(contractData[0].ToString());
        }
        /// <summary>
        /// Name of ERC20 Token
        /// </summary>
        /// <param name="contractAddress"></param>
        /// <returns></returns>
        public static async Task<string> Name(Web3 web3, string contractAddress)
        {
            var contract = web3.ContractBuilder.Build(Abi, contractAddress);
            var name = await contract.Call(EthMethod.Name);
            return name[0].ToString();
        }
        /// <summary>
        /// Symbol of ERC20 Token
        /// </summary>
        /// <param name="contractAddress"></param>
        /// <returns></returns>
        public static async Task<string> Symbol(Web3 web3, string contractAddress)
        {
            var contract = web3.ContractBuilder.Build(Abi, contractAddress);
            var symbol = await contract.Call(EthMethod.Symbol);
            return symbol[0].ToString();
        }
        /// <summary>
        /// Decimals of ERC20 Token
        /// </summary>
        /// <param name="contractAddress"></param>
        /// <returns></returns>
        public static async Task<BigInteger> Decimals(Web3 web3, string contractAddress)
        {
            var contract = web3.ContractBuilder.Build(Abi, contractAddress);
            var decimals = await contract.Call(EthMethod.Decimals);
            return BigInteger.Parse(decimals[0].ToString());
        }
        /// <summary>
        /// Total Supply of ERC20 Token
        /// </summary>
        /// <param name="contractAddress"></param>
        /// <returns></returns>
        public static async Task<BigInteger> TotalSupply(Web3 web3, string contractAddress)
        {
            var contract = web3.ContractBuilder.Build(Abi, contractAddress);
            var totoalsupply = await contract.Call(EthMethod.TotalSupply);
            return BigInteger.Parse(totoalsupply[0].ToString());
        }
    }
}
