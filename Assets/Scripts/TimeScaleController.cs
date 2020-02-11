using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeScaleController : MonoBehaviour
{   
    [SerializeField] private Button _playPauseButton;
    [SerializeField] private Sprite pauseIcon;
    [SerializeField] private Sprite playIcon;
    [SerializeField] private Text pausedText;

    private bool isPaused = false;

    private void Start()
    {
        pausedText.enabled = false;
        _playPauseButton.image.sprite = playIcon;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Backspace))
        {
            ChangeGameState();
        }
    }

    public void ChangeGameState()
    {
        if (isPaused == false)
        {
            isPaused = true;
            Time.timeScale = .2f;
            _playPauseButton.image.sprite = pauseIcon;
            pausedText.enabled = true;
        }
        else if (isPaused == true)
        {
            isPaused = false;
            Time.timeScale = 1f;
            _playPauseButton.image.sprite = playIcon;
            pausedText.enabled = false;
        }
    }

}
