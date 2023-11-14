using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Web3;

namespace Scripts.EVM.Token
{
    public static class Erc20
    {
        /// <summary>
        /// Balance Of ERC20 Address
        /// </summary>
        /// <param name="web3"></param>
        /// <param name="contractAddress"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public static async Task<BigInteger> BalanceOf(Web3 web3, string contractAddress, string account)
        {
            var contract = web3.ContractBuilder.Build(ABI.Erc20, contractAddress);
            var contractData = await contract.Call(EthMethod.BalanceOf, new object[]
            {
                account
            });
            return BigInteger.Parse(contractData[0].ToString());
        }
        
        /// <summary>
        /// Custom ERC20 token balance of an address
        /// </summary>
        /// <param name="web3"></param>
        /// <param name="contractAbi"></param>
        /// <param name="contractAddress"></param>
        /// <returns></returns>
        public static async Task<BigInteger> CustomTokenBalance(Web3 web3, string contractAbi, string contractAddress)
        {
            var contract = web3.ContractBuilder.Build(contractAbi, contractAddress);
            string address = await web3.Signer.GetAddress();
            var contractData = await contract.Call(EthMethod.BalanceOf, new object[] { address });
            return BigInteger.Parse(contractData[0].ToString());
        }
        
		/// <summary>
        /// Native ERC20 balance of an Address
        /// </summary>
        /// <param name="web3"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public static async Task<BigInteger> NativeBalanceOf(Web3 web3, string account)
        {
            return await web3.RpcProvider.GetBalance(account);
        }

        /// <summary>
        /// Name of ERC20 Token
        /// </summary>
        /// <param name="web3"></param>
        /// <param name="contractAddress"></param>
        /// <returns></returns>
        public static async Task<string> Name(Web3 web3, string contractAddress)
        {
            var contract = web3.ContractBuilder.Build(ABI.Erc20, contractAddress);
            var name = await contract.Call(EthMethod.Name);
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
            var contract = web3.ContractBuilder.Build(ABI.Erc20, contractAddress);
            var symbol = await contract.Call(EthMethod.Symbol);
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
            var contract = web3.ContractBuilder.Build(ABI.Erc20, contractAddress);
            var decimals = await contract.Call(EthMethod.Decimals);
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
            var contract = web3.ContractBuilder.Build(ABI.Erc20, contractAddress);
            var totalSupply = await contract.Call(EthMethod.TotalSupply);
            return BigInteger.Parse(totalSupply[0].ToString());
        }
        
        /// <summary>
        /// Mints ERC20 Tokens
        /// </summary>
        /// <param name="web3"></param>
        /// <param name="toAddress"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static async Task<object[]> MintErc20(Web3 web3, string contractAddress, string toAccount, BigInteger amount)
        {
            const string method = EthMethod.Mint;
            var destination = await web3.Signer.GetAddress();
            var contract = web3.ContractBuilder.Build(ABI.Erc20, contractAddress);
            return await contract.Send(method, new object[] { toAccount, amount });
        }

        /// <summary>
        /// Transfers ERC20 Tokens
        /// </summary>
        /// <param name="web3"></param>
        /// <param name="contractAddress"></param>
        /// <param name="toAccount"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        /// <returns></returns>
        public static async Task<object[]> TransferErc20(Web3 web3, string contractAddress, string toAccount, BigInteger amount)
        {
            var method = EthMethod.Transfer;
            var contract = web3.ContractBuilder.Build(ABI.Erc20, contractAddress);
            var response = await contract.Send(method, new object[]
            {
                toAccount,
                amount
            });
            return response;
        }
    }
}