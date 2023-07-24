using UnityEngine;
using Web3Unity.Scripts.Library.Ethers.Providers;

public class ERC20NativeBalanceOf : MonoBehaviour
{
    async void Start()
    {
        string account = "0xaBed4239E4855E120fDA34aDBEABDd2911626BA1";
        var getBalance = await Web3Accessor.Web3.RpcProvider.GetBalance(account);
        Debug.Log("Account Balance: " + getBalance);
    }
}