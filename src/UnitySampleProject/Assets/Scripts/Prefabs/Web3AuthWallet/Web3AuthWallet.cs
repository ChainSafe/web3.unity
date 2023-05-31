using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Web3Unity.Scripts.Library.ETHEREUEM.EIP;
using Web3Unity.Scripts.Library.Ethers.Contracts;
using Web3Unity.Scripts.Library.Ethers.Web3AuthWallet;

public class Web3AuthWallet : MonoBehaviour
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

    void Awake()
    {
        // keeps the wallet alive between scene changes
        DontDestroyOnLoad(this.gameObject);
    }

    public void SetPK()
    {
        // sets the pk to memory and clears it from player prefs
        W3AWalletUtils.pk = PlayerPrefs.GetString("PK");
        PlayerPrefs.SetString("PK", "");
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
        W3AWalletUtils.account = W3AWalletUtils.GetAddressW3A(W3AWalletUtils.pk);
        WalletAddress.text = W3AWalletUtils.account;
        // populate native token balance
        var provider = RPC.GetInstance.Provider();
        var getBalance = await provider.GetBalance(W3AWalletUtils.account);
        NativeTokenBalanceName.text = nativeTokenSymbol;
        NativeTokenBalance.text = (float.Parse(getBalance.ToString()) / Math.Pow(10, 18)).ToString();
        // populate custom token balance if contract and abi is entered by dev
        if ((customTokenCA != "") && (customTokenABI != ""))
        {
            CustomTokenObjPlaceholder.SetActive(false);
            CustomTokenObj.SetActive(true);
            var contract = new Contract(customTokenABI, customTokenCA, RPC.GetInstance.Provider());
            var calldata = await contract.Call("balanceOf", new object[]
            {
                W3AWalletUtils.account
            });
            string customTokenName = await ERC20.Symbol(customTokenCA);
            CustomTokenBalance.text = MathF.Ceiling((float)(float.Parse(calldata[0].ToString()) / Math.Pow(10, 18))).ToString();
            CustomTokenBalanceName.text = customTokenName;
        }
        // if incomingTx bool is true, populate data and show details
        if (W3AWalletUtils.incomingTx)
        {
            IncomingTxObjPlaceholder.SetActive(false);
            IncomingTxObj.SetActive(true);
            IncomingTxAction.text = W3AWalletUtils.incomingAction;
            // sets tx or message to sign (for some reason null doesnt work here but random numbers do)
            if (W3AWalletUtils.incomingMessageData != null)
            {
                IncomingTxHash.text = W3AWalletUtils.incomingMessageData;
            }
            else
            {
                IncomingTxHash.text = W3AWalletUtils.incomingTxData;
            }
        }
    }

    public async void AcceptTX()
    {
        // accepts a transaction and attempts to sign or broadcast
        Debug.Log("Accepting TX");
        if (W3AWalletUtils.incomingAction == "Sign")
        {
            // attempt to sign tx
            try
            {
                string tx = W3AWalletUtils.SignMsgW3A(W3AWalletUtils.pk, W3AWalletUtils.incomingMessageData);
                IncomingTxHash.text = "Signing...";
                await new WaitForSeconds(3);
                W3AWalletUtils.signedTxResponse = tx;
                IncomingTxHash.text = "Sign Successful!";
                UpdateTxHistory(System.DateTime.Now.ToString(), "Sign", "0", "N/A");
            }
            catch (Exception e)
            {
                Debug.LogException(e, this);
            }
            // sets getter back to null ready for the next transaction
            W3AWalletUtils.incomingAction = "";
            W3AWalletUtils.incomingTxData = "";
            W3AWalletUtils.incomingMessageData = "";
        }
        if (W3AWalletUtils.incomingAction == "Broadcast")
        {
            // attempt to broadcast tx
            try
            {
                string _chain = PlayerPrefs.GetString("Chain");
                string _chainId = PlayerPrefs.GetString("ChainID");
                string _network = PlayerPrefs.GetString("Network");
                string _rpc = PlayerPrefs.GetString("RPC");
                string _to = W3AWalletUtils.outgoingContract;
                string _value = "0";
                string _data = W3AWalletUtils.incomingTxData;
                var _gasPrice = await RPC.GetInstance.Provider().GetGasPrice();
                string _gasLimit = "80000";
                //tring _gasLimit = await W3AWalletUtils.GasLimit(_chain, _network, _to, _value, _data, _rpc);
                string transaction = await W3AWalletUtils.CreateTransaction(_chain, _network, W3AWalletUtils.account, _to, _value, _data, _gasPrice.ToString(), _gasLimit, _rpc);
                string signedTx = W3AWalletUtils.SignTransactionW3A(W3AWalletUtils.pk, transaction, _chainId);
                string tx = await W3AWalletUtils.BroadcastTransactionW3A(_chain, _network, W3AWalletUtils.account, _to, _value, _data, signedTx, _gasPrice.ToString(), _gasLimit, _rpc);
                IncomingTxHash.text = "Broadcasting...";
                await new WaitForSeconds(3);
                W3AWalletUtils.signedTxResponse = tx;
                IncomingTxHash.text = "Broadcast Successful!";
                UpdateTxHistory(System.DateTime.Now.ToString(), "Transaction", W3AWalletUtils.amount, tx.ToString());
            }
            catch (Exception e)
            {
                Debug.LogException(e, this);
            }
            // sets bool back to false ready for the next transaction
            W3AWalletUtils.outgoingContract = "";
            W3AWalletUtils.incomingAction = "";
            W3AWalletUtils.incomingTxData = "";
            W3AWalletUtils.amount = "";
            SendingToWallet.text = "";
            AmountToSend.text = "";

        }
        // disables tx object and uses the placeholder to keep things neat
        W3AWalletUtils.incomingTx = false;
        IncomingTxObj.SetActive(false);
        IncomingTxObjPlaceholder.SetActive(true);
    }

    public void DeclineTx()
    {
        // declines transaction, resets data, disables tx object and uses the placeholder to keep things neat
        Debug.Log("Declining TX");
        IncomingTxObj.SetActive(false);
        IncomingTxObjPlaceholder.SetActive(true);
        W3AWalletUtils.signedTxResponse = "Declined";
        W3AWalletUtils.incomingAction = "";
        W3AWalletUtils.incomingTxData = "";
        W3AWalletUtils.incomingMessageData = "";
        W3AWalletUtils.amount = "";
        W3AWalletUtils.incomingTx = false;
    }

    public void SendCustomTokens()
    {
        if (SendingToWallet.text != "" && AmountToSend.text != "")
        {
            // smart contract method to call
            string method = "transfer";
            // amount to send
            float eth = float.Parse(AmountToSend.text);
            float decimals = 1000000000000000000; // 18 decimals
            float wei = eth * decimals;
            string amount = Convert.ToDecimal(wei).ToString();
            W3AWalletUtils.outgoingContract = customTokenCA;
            // connects to user's wallet to send a transaction
            try
            {
                // connects to user's browser wallet to call a transaction
                var contract = new Contract(customTokenABI, customTokenCA);
                Debug.Log("Contract: " + contract);
                var calldata = contract.Calldata(method, new object[]
                {
                    SendingToWallet.text,
                    BigInteger.Parse(amount)
                });
                Debug.Log("Contract Data: " + calldata);
                // finds the wallet, sets sign and incoming tx conditions to true and opens
                W3AWalletUtils.incomingTx = true;
                W3AWalletUtils.incomingAction = "Broadcast";
                W3AWalletUtils.incomingTxData = calldata;
                W3AWalletUtils.amount = AmountToSend.text;
                this.GetComponent<Web3AuthWallet>().OpenButton();
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
            Application.OpenURL((blockExplorerUrl + "tx/" + HashesTexts[number].text));
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
