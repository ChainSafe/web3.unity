using System.Numerics;
using UnityEngine;

public class XDai721BalanceOfExample : MonoBehaviour
{
    async void Start()
    {
        string contract = "0x90FdA259CFbdB74F1804e921F523e660bfBE698d";
        string account = "0x525C18aB76A28C367c876BBDFaa16Bb96865F9fE";

        BigInteger balance = await XDai721.BalanceOf(contract, account);

        print (balance);
    }
}
