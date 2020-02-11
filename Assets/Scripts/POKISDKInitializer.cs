using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POKISDKInitializer : MonoBehaviour
{
    private void Awake() { PokiUnitySDK.Instance.init(); }

    // POKI CODE
    public void PlayCommercialBreak()
    {
        if (PokiUnitySDK.Instance.adsBlocked()) return;

        //PokiUnitySDK.Instance.gameplayStop();
        AudioListener.volume = 0f;

        PokiUnitySDK.Instance.commercialBreakCallBack = commercialBreakComplete;
        PokiUnitySDK.Instance.commercialBreak();
    }

    //POKI CODE
    public void commercialBreakComplete()
    {
        //PokiUnitySDK.Instance.gameplayStart();
        AudioListener.volume = 1f;
    }
}