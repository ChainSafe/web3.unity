using System.Numerics;
using System.Threading.Tasks;
using Web3Unity.Scripts.Library.Ethers.Contracts;


namespace Web3Unity.Scripts.Library.ETHEREUEM.EIP
{
    public class ERC20
    {

        private static string abi = ABI.ERC_20;
        /// <summary>
        /// Balance Of ERC20 Address
        /// </summary>
        /// <param name="_contract"></param>
        /// <param name="_account"></param>
        /// <returns></returns>
        public static async Task<BigInteger> BalanceOf(string _contract, string _account)
        {
            var provider = RPC.GetInstance.Provider();
            var contract = new Contract(abi, _contract, provider);
            string method = ETH_METHOD.BalanceOf;
            var contractData = await contract.Call(method, new object[]
            {
                _account
            });
            return BigInteger.Parse(contractData[0].ToString());
        }
        /// <summary>
        /// Name of ERC20 Token
        /// </summary>
        /// <param name="_contract"></param>
        /// <returns></returns>
        public static async Task<string> Name(string _contract)
        {
            string method = ETH_METHOD.Name;
            var provider = RPC.GetInstance.Provider();
            var contract = new Contract(abi, _contract, provider);
            var name = await contract.Call(method);
            return name[0].ToString();
        }
        /// <summary>
        /// Symbol of ERC20 Token
        /// </summary>
        /// <param name="_contract"></param>
        /// <returns></returns>
        public static async Task<string> Symbol(string _contract)
        {
            string method = ETH_METHOD.Symbol;
            var provider = RPC.GetInstance.Provider();
            var contract = new Contract(abi, _contract, provider);
            var symbol = await contract.Call(method);
            return symbol[0].ToString();
        }
        /// <summary>
        /// Decimals of ERC20 Token
        /// </summary>
        /// <param name="_contract"></param>
        /// <returns></returns>
        public static async Task<BigInteger> Decimals(string _contract)
        {
            string method = ETH_METHOD.Decimals;
            var provider = RPC.GetInstance.Provider();
            var contract = new Contract(abi, _contract, provider);
            var decimals = await contract.Call(method);
            return BigInteger.Parse(decimals[0].ToString());
        }
        /// <summary>
        /// Total Supply of ERC20 Token
        /// </summary>
        /// <param name="_contract"></param>
        /// <returns></returns>
        public static async Task<BigInteger> TotalSupply(string _contract)
        {
            string method = ETH_METHOD.TotalSupply;
            var provider = RPC.GetInstance.Provider();
            var contract = new Contract(abi, _contract, provider);
            var totoalsupply = await contract.Call(method);
            return BigInteger.Parse(totoalsupply[0].ToString());
        }
    }
}
