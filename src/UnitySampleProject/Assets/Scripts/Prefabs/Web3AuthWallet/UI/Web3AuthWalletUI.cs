using System;
using System.Globalization;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.GamingWeb3;
using ChainSafe.GamingWeb3.Build;
using ChainSafe.GamingWeb3.Unity;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Prefabs.Web3AuthWallet.Interfaces;
using Prefabs.Web3AuthWallet.Services;
using Prefabs.Web3AuthWallet.Utils;
using Scripts.Web3AuthWallet;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Web3Unity.Scripts.Library.ETHEREUEM.Connect;
using Web3Unity.Scripts.Library.Ethers.Contracts;
using Web3Unity.Scripts.Library.Ethers.JsonRpc;
using Web3Unity.Scripts.Library.Ethers.Transactions;
using Web3Unity.Scripts.Library.Ethers.Web3AuthWallet;

namespace Prefabs.Web3AuthWallet.UI
{
    public class Web3AuthWalletUI : MonoBehaviour
    {
        public string nativeTokenSymbol;
        public string blockExplorerUrl;
        public string customTokenABI;
        public string customTokenCA;
        public InputField SendingToWallet;
        public InputField AmountToSend;
        public GameObject WalletCanvas;
        public GameObject CustomTokenObjPlaceholder;
        public GameObject IncomingTxObjPlaceholder;
        public GameObject CustomTokenObj;
        public GameObject IncomingTxObj;
        public Text WalletAddress;
        public Text NativeTokenBalance;
        public Text NativeTokenBalanceName;
        public Text CustomTokenBalance;
        public Text CustomTokenBalanceName;
        public Text IncomingTxAction;
        public Text IncomingTxHash;
        private bool walletOpen;
        private bool pkSet;
        private int txNumber;

        // texts for tx history
        public Text[] DatesTexts;
        public Text[] ActionsTexts;
        public Text[] AmountsTexts;
        public Text[] HashesTexts;
        private Web3AuthWalletConfig _web3AuthWalletConfig;
        private Scripts.Web3AuthWallet.Web3AuthWallet _web3AuthWallet;
        private Web3 web3;
        private SignatureService _signatureService;
        private IEthereumService ethereumService;

        async void Awake()
        {
            var projectConfig = ProjectConfigUtilities.Load();
            web3 = await new Web3Builder(projectConfig)
                .Configure(services =>
                {
                    services.UseUnityEnvironment();
                    services.UseJsonRpcProvider();
                    services.UseWeb3AuthWallet();
                })
                .BuildAsync();
            _signatureService = new SignatureService();

            // keeps the wallet alive between scene changes
            DontDestroyOnLoad(this.gameObject);
        }

        public void CloseButton()
        {
            // closes the wallet
            walletOpen = false;
            WalletCanvas.SetActive(false);
        }

        public void OpenButton()
        {
            // opens the wallet and stops wallet being used on log in scene as it isn't ready
            Scene scene = SceneManager.GetActiveScene();
            if (scene.name != "LoginWeb3Auth")
            {
                if (!pkSet)
                {
                    pkSet = true;
                }

                walletOpen = true;
                WalletCanvas.SetActive(true);
            }

            // gets balances
            GetData();
        }

        async public void GetData()
        {
            ethereumService = new EthereumService(W3AWalletUtils.PrivateKey, "https://goerli.infura.io/v3/2ea27900c8784457ac03b1cbd4e7b8f0");

            // updates the wallets balances and custom tokens if specified by dev
            Debug.Log("Updating wallet data");

            // populate wallet address
            W3AWalletUtils.Account = ethereumService.GetAddressW3A(W3AWalletUtils.PrivateKey);
            WalletAddress.text = W3AWalletUtils.Account;

            // populate native token balance
            var provider = web3.RpcProvider;
            var getBalance = await provider.GetBalance(W3AWalletUtils.Account);
            NativeTokenBalanceName.text = nativeTokenSymbol;
            NativeTokenBalance.text = (float.Parse(getBalance.ToString()) / Math.Pow(10, 18)).ToString(CultureInfo.InvariantCulture);

            // populate custom token balance if contract and abi is entered by dev
            if ((customTokenCA != string.Empty) && (customTokenABI != string.Empty))
            {
                CustomTokenObjPlaceholder.SetActive(false);
                CustomTokenObj.SetActive(true);
                var contract = new Contract(customTokenABI, customTokenCA, provider);
                var calldata = await contract.Call("balanceOf", new object[]
                {
                    W3AWalletUtils.Account,
                });
                var customTokenName = await contract.Call("symbol");
                CustomTokenBalance.text = MathF.Ceiling((float)(float.Parse(calldata[0].ToString()) / Math.Pow(10, 18))).ToString(CultureInfo.InvariantCulture);
                CustomTokenBalanceName.text = customTokenName[0].ToString();
            }

            // if incomingTx bool is true, populate data and show details
            if (W3AWalletUtils.IncomingTx)
            {
                IncomingTxObjPlaceholder.SetActive(false);
                IncomingTxObj.SetActive(true);
                IncomingTxAction.text = W3AWalletUtils.IncomingAction;

                // sets tx or message to sign (for some reason null doesnt work here but random numbers do)
                if (W3AWalletUtils.IncomingMessageData != null)
                {
                    IncomingTxHash.text = W3AWalletUtils.IncomingMessageData;
                }
                else
                {
                    IncomingTxHash.text = W3AWalletUtils.IncomingTxData;
                }
            }
        }

        public async Task<string> Symbol(string contract1, string abi)
        {
            string method = "symbol";
            var provider = web3.RpcProvider;
            var contract = new Contract(abi, contract1, provider);
            var symbol = await contract.Call(method);
            return symbol[0].ToString();
        }

        public async void AcceptTX()
        {
            // accepts a transaction and attempts to sign or broadcast
            Debug.Log("Accepting TX");
            if (W3AWalletUtils.IncomingAction == "Sign")
            {
                // attempt to sign tx
                try
                {
                    string tx = _signatureService.SignMessage(W3AWalletUtils.PrivateKey, W3AWalletUtils.IncomingMessageData);
                    IncomingTxHash.text = "Signing...";
                    await new WaitForSeconds(3);
                    W3AWalletUtils.SignedTxResponse = tx;
                    IncomingTxHash.text = "Sign Successful!";
                    UpdateTxHistory(System.DateTime.Now.ToString(), "Sign", "0", "N/A");
                }
                catch (Exception e)
                {
                    Debug.LogException(e, this);
                }

                // sets getter back to null ready for the next transaction
                W3AWalletUtils.IncomingAction = string.Empty;
                W3AWalletUtils.IncomingTxData = string.Empty;
                W3AWalletUtils.IncomingMessageData = string.Empty;
            }

            if (W3AWalletUtils.IncomingAction == "Broadcast")
            {
                // attempt to broadcast tx
                try
                {
                    string to = W3AWalletUtils.OutgoingContract;
                    string data = W3AWalletUtils.IncomingTxData;
                    string gasLimit = "100000";

                    TransactionInput txInput = new TransactionInput
                    {
                        To = to,
                        From = ethereumService.GetAddressW3A(W3AWalletUtils.PrivateKey),
                        Value = new HexBigInteger(0), // Convert the Ether amount to Wei
                        Data = data,
                        GasPrice = new HexBigInteger(100000),
                        Gas = new HexBigInteger(56000),
                    };

                    try
                    {
                        IncomingTxHash.text = "Broadcasting...";
                        await new WaitForSeconds(3);
                        var signedTransactionData = await ethereumService.CreateAndSignTransactionAsync(txInput);
                        Debug.Log($"Signed transaction data: {signedTransactionData}");
                        var transactionHash = await ethereumService.SendTransactionAsync(signedTransactionData);
                        Debug.Log($"Transaction hash: {transactionHash}");
                        W3AWalletUtils.SignedTxResponse = transactionHash;
                        UpdateTxHistory(DateTime.Now.ToString(), "Transaction", W3AWalletUtils.Amount, transactionHash);

                    }
                    catch (Exception ex)
                    {
                        Debug.Log($"An error occurred: {ex.Message}");
                    }
                    IncomingTxHash.text = "Broadcasting...";
                    await new WaitForSeconds(3);
                    IncomingTxHash.text = "Broadcast Successful!";
                }
                catch (Exception e)
                {
                    Debug.LogException(e, this);
                }

                // sets bool back to false ready for the next transaction
                W3AWalletUtils.OutgoingContract = string.Empty;
                W3AWalletUtils.IncomingAction = string.Empty;
                W3AWalletUtils.IncomingTxData = string.Empty;
                W3AWalletUtils.Amount = string.Empty;
                SendingToWallet.text = string.Empty;
                AmountToSend.text = string.Empty;
            }

            // disables tx object and uses the placeholder to keep things neat
            W3AWalletUtils.IncomingTx = false;
            IncomingTxObj.SetActive(false);
            IncomingTxObjPlaceholder.SetActive(true);
        }

        public void DeclineTx()
        {
            // declines transaction, resets data, disables tx object and uses the placeholder to keep things neat
            Debug.Log("Declining TX");
            IncomingTxObj.SetActive(false);
            IncomingTxObjPlaceholder.SetActive(true);
            W3AWalletUtils.SignedTxResponse = "Declined";
            W3AWalletUtils.IncomingAction = string.Empty;
            W3AWalletUtils.IncomingTxData = string.Empty;
            W3AWalletUtils.IncomingMessageData = string.Empty;
            W3AWalletUtils.Amount = string.Empty;
            W3AWalletUtils.IncomingTx = false;
        }

        public void SendCustomTokens()
        {
            if (SendingToWallet.text != string.Empty && AmountToSend.text != string.Empty)
            {
                // smart contract method to call
                string method = "transfer";

                // amount to send
                float eth = float.Parse((ReadOnlySpan<char>)AmountToSend.text);
                float decimals = 1000000000000000000; // 18 decimals
                float wei = eth * decimals;
                string amount = Convert.ToDecimal(wei).ToString();
                W3AWalletUtils.OutgoingContract = customTokenCA;

                // connects to user's wallet to send a transaction
                try
                {
                    // connects to user's browser wallet to call a transaction
                    var contract = new Contract(customTokenABI, customTokenCA);
                    Debug.Log("Contract: " + contract);
                    var calldata = contract.Calldata(method, new object[]
                    {
                        SendingToWallet.text,
                        BigInteger.Parse(amount),
                    });
                    Debug.Log("Contract Data: " + calldata);

                    // finds the wallet, sets sign and incoming tx conditions to true and opens
                    W3AWalletUtils.IncomingTx = true;
                    W3AWalletUtils.IncomingAction = "Broadcast";
                    W3AWalletUtils.IncomingTxData = calldata;
                    W3AWalletUtils.Amount = AmountToSend.text;
                    this.GetComponent<Web3AuthWalletUI>().OpenButton();
                }
                catch (Exception e)
                {
                    Debug.LogException(e, this);
                }
            }
        }

        private void UpdateTxHistory(string date, string action, string amount, string txHash)
        {
            // Check if we need to push transactions down
            if (txNumber >= DatesTexts.Length)
            {
                // Shift transactions up by one
                for (int i = 0; i < DatesTexts.Length - 1; i++)
                {
                    DatesTexts[i].text = DatesTexts[i + 1].text;
                    ActionsTexts[i].text = ActionsTexts[i + 1].text;
                    AmountsTexts[i].text = AmountsTexts[i + 1].text;
                    HashesTexts[i].text = HashesTexts[i + 1].text;
                }

                // Update the last slot with the new transaction
                DatesTexts[DatesTexts.Length - 1].text = date;
                ActionsTexts[DatesTexts.Length - 1].text = action;
                AmountsTexts[DatesTexts.Length - 1].text = amount;
                HashesTexts[DatesTexts.Length - 1].text = txHash;
            }
            else
            {
                // Update the next available slot with the new transaction
                DatesTexts[txNumber].text = date;
                ActionsTexts[txNumber].text = action;
                AmountsTexts[txNumber].text = amount;
                HashesTexts[txNumber].text = txHash;
            }

            txNumber++;
        }


        public void OpenBlockExplorer(int number)
        {
            // opens the given block explorer to the chosen hash if it's not null (i've used a random number here as null was failing)
            if (HashesTexts[number].text.Length > 15)
            {
                Application.OpenURL(blockExplorerUrl + "tx/" + HashesTexts[number].text);
            }
        }

        public void CopyAddress()
        {
            // copies address to clipboard (webgl needs an extension to do this)
            GUIUtility.systemCopyBuffer = WalletAddress.text;
        }

        private void Update()
        {
            // opens and closes wallet on shift + tab keypress combination
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (walletOpen)
                {
                    CloseButton();
                }
                else
                {
                    OpenButton();
                }
            }
        }
    }
}