using System;
using System.Numerics;
using Nethereum.Util;
using TMPro;
using UnityEngine;

namespace ChainSafe.Gaming
{
    public class CustomTokenRowDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI balanceText;
        
        [SerializeField] private TextMeshProUGUI symbolText;
        
        public void Attach(CustomToken token)
        {
            balanceText.text = $"{token.Balance / new BigInteger(Math.Pow(10, (int) token.Decimals))}";
            
            symbolText.text = token.Symbol;
        }
    }
}