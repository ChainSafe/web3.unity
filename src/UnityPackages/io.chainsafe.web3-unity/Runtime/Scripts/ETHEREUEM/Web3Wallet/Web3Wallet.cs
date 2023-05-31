using System;
using System.Threading.Tasks;
using GameData;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Signer;
using Nethereum.Util;
using UnityEngine;

namespace Web3Unity.Scripts.Library.Web3Wallet
{
    public class Web3Wallet
    {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        private static string url = "https://metamask.app.link/dapp/chainsafe.github.io/game-web3wallet/";
#else
        public static string url { get; set; }
#endif

        public static async Task<string> SendTransaction(string _chainId, string _to, string _value, string _data = "",
            string _gasLimit = "", string _gasPrice = "")
        {
            // open application
            Application.OpenURL(url + "?action=send" + "&chainId=" + _chainId + "&to=" + _to + "&value=" + _value +
                                "&data=" + _data + "&gasLimit=" + _gasLimit + "&gasPrice=" + _gasPrice);
            // set clipboard to empty
            GUIUtility.systemCopyBuffer = "";
            // wait for clipboard response
            var clipBoard = "";
            while (clipBoard == "")
            {
                clipBoard = GUIUtility.systemCopyBuffer;
                await Task.Delay(100);
            }
            var data = new
            {
                Client = "Desktop/Mobile",
                Version = "v2",
                ProjectID = PlayerPrefs.GetString("ProjectID"),
                Player = Sha3(PlayerPrefs.GetString("Account") + PlayerPrefs.GetString("ProjectID")),
                ChainId = _chainId,
                Address = _to,
                Value = _value,
                GasLimit = _gasLimit,
                GasPrice = _gasPrice,
                Data = _data
            };
            // check if clipboard response is valid
            if (clipBoard.StartsWith("0x") && clipBoard.Length == 66)
            {
                Logging.SendGameData(data);
                return clipBoard;
            }
            else
            {
                throw new Exception("transaction error");
            }
        }

        public static async Task<string> Sign(string _message)
        {
            // open application
            var message = Uri.EscapeDataString(_message);
            Application.OpenURL(url + "?action=sign" + "&message=" + message);
            // set clipboard to empty
            GUIUtility.systemCopyBuffer = "";
            // wait for clipboard response
            var clipBoard = "";
            while (clipBoard == "")
            {
                clipBoard = GUIUtility.systemCopyBuffer;
                await Task.Delay(100);
            }

            // check if clipboard response is valid
            if (clipBoard.StartsWith("0x") && clipBoard.Length == 132)
                return clipBoard;
            else
                throw new Exception("sign error");
        }

        public static string Sha3(string _message)
        {
            var signer = new EthereumMessageSigner();
            var hash = new Sha3Keccack().CalculateHash(_message).EnsureHexPrefix();
            // 0x06b3dfaec148fb1bb2b066f10ec285e7c9bf402ab32aa78a5d38e34566810cd2
            return hash;
        }
    }
}