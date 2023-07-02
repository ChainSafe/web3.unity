using System;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.GamingSDK.EVM.Web3AuthWallet;
using ChainSafe.GamingWeb3;
using ChainSafe.GamingWeb3.Build;
using ChainSafe.GamingWeb3.Unity;
using Nethereum.Hex.HexTypes;
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
        private TransactionService _transactionService;
        private SignatureService _signatureService;
        private ITransactionConfig transactionConfig;
        private IHttpRequestHandler httpRequestHandler;
        private IEthereumService ethereumService;

        async void Awake()
        {
            var projectConfig = ProjectConfigUtilities.Load();
            transactionConfig = new TransactionConfig(projectConfig);
            httpRequestHandler = new HttpRequestHandler();
            ethereumService = new EthereumService();
            web3 = await new Web3Builder(projectConfig)
                .Configure(services =>
                {
                    services.UseUnityEnvironment();
                    services.UseJsonRpcProvider();
                    services.UseWeb3AuthWallet();
                })
                .BuildAsync();
            _web3AuthWalletConfig = new Web3AuthWalletConfig();
            _transactionService = new TransactionService(transactionConfig, httpRequestHandler, ethereumService);
            _signatureService = new SignatureService();

            // keeps the wallet alive between scene changes
            DontDestroyOnLoad(this.gameObject);
        }

        public void SetPK()
        {
            // sets the pk to memory and clears it from player prefs
            W3AWalletUtils.PrivateKey = PlayerPrefs.GetString("PK");
            _web3AuthWalletConfig.PrivateKey = PlayerPrefs.GetString("PK");
            PlayerPrefs.SetString("PK", string.Empty);
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
                    SetPK();
                }

                walletOpen = true;
                WalletCanvas.SetActive(true);
            }

            // gets balances
            GetData();
        }

        async public void GetData()
        {
            // updates the wallets balances and custom tokens if specified by dev
            Debug.Log("Updating wallet data");

            // populate wallet address
            W3AWalletUtils.Account = ethereumService.GetAddressW3A(_web3AuthWalletConfig.PrivateKey);
            WalletAddress.text = W3AWalletUtils.Account;

            // populate native token balance
            var provider = web3.RpcProvider;
            var getBalance = await provider.GetBalance(W3AWalletUtils.Account);
            NativeTokenBalanceName.text = nativeTokenSymbol;
            NativeTokenBalance.text = (float.Parse(getBalance.ToString()) / Math.Pow(10, 18)).ToString();

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
                CustomTokenBalance.text = MathF.Ceiling((float)(float.Parse(calldata[0].ToString()) / Math.Pow(10, 18))).ToString();
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
                    var gasPrice = await web3.RpcProvider.GetGasPrice();
                    string gasLimit = "100000";
                    var txRequest = new TransactionRequest
                    {
                        To = to,
                        Value = new HexBigInteger(0),
                        Data = data,
                        GasPrice = gasPrice,
                        GasLimit = new HexBigInteger(gasLimit),
                    };
                    EVM.Response<string> transaction = await _transactionService.CreateTransaction(txRequest, W3AWalletUtils.Account, gasPrice.ToString(), gasLimit, string.Empty);
                    string signedTx = _signatureService.SignTransaction(W3AWalletUtils.PrivateKey, transaction.response);
                    EVM.Response<string> tx = await _transactionService.BroadcastTransaction(txRequest, W3AWalletUtils.Account, signedTx, gasPrice.ToString(), gasLimit);
                    IncomingTxHash.text = "Broadcasting...";
                    await new WaitForSeconds(3);
                    W3AWalletUtils.SignedTxResponse = tx.response;
                    Debug.Log("Tranascation: " + tx.response);
                    IncomingTxHash.text = "Broadcast Successful!";
                    UpdateTxHistory(DateTime.Now.ToString(), "Transaction", W3AWalletUtils.Amount, tx.response);
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
            // updates transaction history texts
            if (txNumber == 0)
            {
                DatesTexts[0].text = date;
                ActionsTexts[0].text = action;
                AmountsTexts[0].text = amount;
                HashesTexts[0].text = txHash;
                txNumber++;
            }
            else if (txNumber == 1)
            {
                DatesTexts[1].text = date;
                ActionsTexts[1].text = action;
                AmountsTexts[1].text = amount;
                HashesTexts[1].text = txHash;
                txNumber++;
            }
            else if (txNumber == 2)
            {
                DatesTexts[2].text = date;
                ActionsTexts[2].text = action;
                AmountsTexts[2].text = amount;
                HashesTexts[2].text = txHash;
                txNumber++;
            }
            else if (txNumber == 3)
            {
                DatesTexts[3].text = date;
                ActionsTexts[3].text = action;
                AmountsTexts[3].text = amount;
                HashesTexts[3].text = txHash;
                txNumber++;
            }
            else
            {
                // push all transactions down and update
                // 1
                DatesTexts[0].text = DatesTexts[1].text;
                ActionsTexts[0].text = ActionsTexts[1].text;
                AmountsTexts[0].text = AmountsTexts[1].text;
                HashesTexts[0].text = HashesTexts[1].text;

                // 2
                DatesTexts[1].text = DatesTexts[2].text;
                ActionsTexts[1].text = ActionsTexts[2].text;
                AmountsTexts[1].text = AmountsTexts[2].text;
                HashesTexts[1].text = HashesTexts[2].text;

                // 3
                DatesTexts[2].text = DatesTexts[3].text;
                ActionsTexts[2].text = ActionsTexts[3].text;
                AmountsTexts[2].text = AmountsTexts[3].text;
                HashesTexts[2].text = HashesTexts[3].text;

                // 4
                DatesTexts[3].text = date;
                ActionsTexts[3].text = action;
                AmountsTexts[3].text = amount;
                HashesTexts[3].text = txHash;
                txNumber++;
            }
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