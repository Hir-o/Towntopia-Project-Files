using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetSave : MonoBehaviour
{
    [SerializeField] private SaveMe_Manager saveMe;

    private void Start() 
    {
        saveMe.ResetSave();
    }
}
