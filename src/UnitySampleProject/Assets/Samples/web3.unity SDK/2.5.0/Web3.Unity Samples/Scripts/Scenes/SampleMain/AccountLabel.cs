using ChainSafe.Gaming.UnityPackage;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class AccountLabel : MonoBehaviour
{
    private TMP_Text label;

    private void Awake()
    {
        label = GetComponent<TMP_Text>();
        label.text = Web3Accessor.Web3.Signer.PublicAddress;
    }
}
