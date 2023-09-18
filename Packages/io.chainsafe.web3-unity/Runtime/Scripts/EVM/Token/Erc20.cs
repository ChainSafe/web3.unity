using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3;

namespace Scripts.EVM.Token
{
    // todo convert this into a service
    public class Erc20
    {

        private static readonly string Abi = ABI.Erc20;
        /// <summary>
        /// Balance Of ERC20 Address
        /// </summary>
        /// <param name="web3"></param>
        /// <param name="contractAddress"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public static async Task<BigInteger> BalanceOf(Web3 web3, string contractAddress, string account)
        {
            var contract = web3.ContractBuilder.Build(Abi, contractAddress);
            var contractData = await contract.Call(CommonMethod.BalanceOf, new object[]
            {
                account
            });
            return BigInteger.Parse(contractData[0].ToString());
        }

        /// <summary>
        /// Name of ERC20 Token
        /// </summary>
        /// <param name="web3"></param>
        /// <param name="contractAddress"></param>
        /// <returns></returns>
        public static async Task<string> Name(Web3 web3, string contractAddress)
        {
            var contract = web3.ContractBuilder.Build(Abi, contractAddress);
            var name = await contract.Call(CommonMethod.Name);
            return name[0].ToString();
        }

        /// <summary>
        /// Symbol of ERC20 Token
        /// </summary>
        /// <param name="web3"></param>
        /// <param name="contractAddress"></param>
        /// <returns></returns>
        public static async Task<string> Symbol(Web3 web3, string contractAddress)
        {
            var contract = web3.ContractBuilder.Build(Abi, contractAddress);
            var symbol = await contract.Call(CommonMethod.Symbol);
            return symbol[0].ToString();
        }

        /// <summary>
        /// Decimals of ERC20 Token
        /// </summary>
        /// <param name="web3"></param>
        /// <param name="contractAddress"></param>
        /// <returns></returns>
        public static async Task<BigInteger> Decimals(Web3 web3, string contractAddress)
        {
            var contract = web3.ContractBuilder.Build(Abi, contractAddress);
            var decimals = await contract.Call(CommonMethod.Decimals);
            return BigInteger.Parse(decimals[0].ToString());
        }

        /// <summary>
        /// Total Supply of ERC20 Token
        /// </summary>
        /// <param name="web3"></param>
        /// <param name="contractAddress"></param>
        /// <returns></returns>
        public static async Task<BigInteger> TotalSupply(Web3 web3, string contractAddress)
        {
            var contract = web3.ContractBuilder.Build(Abi, contractAddress);
            var totalSupply = await contract.Call(CommonMethod.TotalSupply);
            return BigInteger.Parse(totalSupply[0].ToString());
        }
    }
}
