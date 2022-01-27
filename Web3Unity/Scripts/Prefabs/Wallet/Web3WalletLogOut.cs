using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Web3WalletLogOut : MonoBehaviour
{
   public void OnLogOut()
    {
         PlayerPrefs.SetString("Account", "");
         SceneManager.LoadScene(0);
    } 
}
