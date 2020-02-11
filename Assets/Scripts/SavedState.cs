using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SavedState : MonoBehaviour
{
    private int gameSaved;

    [SerializeField] private Color normalColor;
    [SerializeField] private Color lockedColor;

    [SerializeField] private Button continueButton;
    [SerializeField] private TextMeshProUGUI continueText;

    private void Start() 
    {
        gameSaved = PlayerPrefs.GetInt("levelSaved", 0);

        if (gameSaved == 0)
        {
            continueButton.interactable = false;
            continueText.color = lockedColor;
        }
        else
        {
            continueButton.interactable = true;
            continueText.color = normalColor;
        }
    }
}
