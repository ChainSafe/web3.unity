using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Web3;

namespace Scripts.EVM.Token
{
    public class Erc20
    {
        private static readonly string Abi = ABI.Erc20;
        private Web3 web3;

        public Erc20(Web3 web3)
        {
            this.web3 = web3 ?? throw new Web3Exception(
                "Web3 instance is null. Please ensure that the instance is properly retrieved trough the constructor");
        }

        /// <summary>
        /// Balance Of ERC20 Address
        /// </summary>
        /// <param name="contractAddress"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task<BigInteger> BalanceOf(string contractAddress, string account)
        {
            var contract = web3.ContractBuilder.Build(Abi, contractAddress);
            var contractData = await contract.Call(CommonMethod.BalanceOf, new object[]
            {
                account
            });
            return BigInteger.Parse(contractData[0].ToString());
        }
        
        /// <summary>
        /// Custom ERC20 token balance of an address
        /// </summary>
        /// <param name="contractAbi"></param>
        /// <param name="contractAddress"></param>
        /// <returns></returns>
        public async Task<string> CustomTokenBalance(string contractAbi, string contractAddress)
        {
            var contract = Web3Accessor.Web3.ContractBuilder.Build(contractAbi, contractAddress);
            var address = web3.Signer.GetAddress();
            var response = await contract.Call("balanceOf", new object[] { address });
            var tokenBalance = response[0].ToString();
            return tokenBalance;
        }
        
		/// <summary>
        /// Native ERC20 balance of an Address
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task<BigInteger> NativeBalanceOf(string account)
        {
            return await web3.RpcProvider.GetBalance(account);
        }

        /// <summary>
        /// Name of ERC20 Token
        /// </summary>
        /// <param name="contractAddress"></param>
        /// <returns></returns>
        public async Task<string> Name(string contractAddress)
        {
            var contract = web3.ContractBuilder.Build(Abi, contractAddress);
            var name = await contract.Call(CommonMethod.Name);
            return name[0].ToString();
        }

        /// <summary>
        /// Symbol of ERC20 Token
        /// </summary>
        /// <param name="contractAddress"></param>
        /// <returns></returns>
        public async Task<string> Symbol(string contractAddress)
        {
            var contract = web3.ContractBuilder.Build(Abi, contractAddress);
            var symbol = await contract.Call(CommonMethod.Symbol);
            return symbol[0].ToString();
        }

        /// <summary>
        /// Decimals of ERC20 Token
        /// </summary>
        /// <param name="contractAddress"></param>
        /// <returns></returns>
        public async Task<BigInteger> Decimals(string contractAddress)
        {
            var contract = web3.ContractBuilder.Build(Abi, contractAddress);
            var decimals = await contract.Call(CommonMethod.Decimals);
            return BigInteger.Parse(decimals[0].ToString());
        }

        /// <summary>
        /// Total Supply of ERC20 Token
        /// </summary>
        /// <param name="contractAddress"></param>
        /// <returns></returns>
        public async Task<BigInteger> TotalSupply(string contractAddress)
        {
            var contract = web3.ContractBuilder.Build(Abi, contractAddress);
            var totalSupply = await contract.Call(CommonMethod.TotalSupply);
            return BigInteger.Parse(totalSupply[0].ToString());
        }
    }
}