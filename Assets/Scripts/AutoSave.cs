using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AutoSave : MonoBehaviour
{
    [SerializeField] private SaveMe_Manager saveMe;
    [SerializeField] private TextMeshProUGUI autoSaveText;

    private void Start()
    {
        InvokeRepeating("SaveGame", 900f, 900f);
    }

    private void SaveGame()
    {
        saveMe.Save();
        autoSaveText.enabled = true;

        Invoke("DisableText", 3f);
    }

    private void DisableText()
    {
        autoSaveText.enabled = false;
    }
}
