using ChainSafe.Gaming.UnityPackage;
using UnityEngine;

public class GelatoDisableContent : MonoBehaviour
{
    private GelatoSample gelato;

    private void Awake()
    {
        gelato = new GelatoSample(Web3Accessor.Web3);
        if (!gelato.GetGelatoDisabled()) return;
        Debug.Log("Gelato functionality disabled as your chain isn't supported");
        gameObject.SetActive(false);
    }
}