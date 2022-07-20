using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateListItemWebWallet : MonoBehaviour
{
    private string chain = "ethereum";

    private string network = "goerli";
    
    // Start is called before the first frame update
    async void Start()
    {
        var response = await EVM.CreateListNftTransaction(chain, network, PlayerPrefs.GetString("Account"), "26", "10000000000000000", "721");
        Debug.Log("Response: " + response);

    }

    public async void CreateMintItem()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
