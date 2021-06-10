﻿using System.Numerics;
using UnityEngine;

public class MoonbeamVerifyExample : MonoBehaviour
{
    async void Start()
    {
        string message = "YOUR_MESSAGE";
        string signature = "0x94bdbebbd0180195b89721a55c3a436a194358c9b3c4eafd22484085563ff55e49a4552904266a5b56662b280757f6aad3b2ab91509daceef4e5b3016afd34781b";

        string account = await Moonbeam.Verify(message, signature);
        
        print (account);
    }
}
