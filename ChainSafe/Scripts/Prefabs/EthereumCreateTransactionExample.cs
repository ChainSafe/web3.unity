using System.Numerics;
using EthereumDefinition;
using UnityEngine;

public class EthereumCreateTransactionExample : MonoBehaviour
{
    async void Start()
    {
        string network = "rinkeby"; // mainnet ropsten kovan rinkeby goerli
        string from = "0x35706484aB20Cbf22F5c7a375D5764DA8166aE1c";
        string to = "0x7E3bE66431ba73956213C40C0828355D1A7894D3";
        string eth = "0.00111";

        Transaction transaction = await Ethereum.CreateTransaction(network, from, to, eth);

        print("network: " + transaction.network);
        print("to: " + transaction.to);
        print("wei: " + transaction.wei);
        print("nonce: " + transaction.nonce);
        print("gasLimit: " + transaction.gasLimit);
        print("gasPrice: " + transaction.gasPrice);
        print("hex: " + transaction.hex);
    }
}
