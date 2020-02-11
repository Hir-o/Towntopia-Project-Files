using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POKIGameplayStart : MonoBehaviour
{
    private void Start() { PokiUnitySDK.Instance.gameplayStart(); }
}
