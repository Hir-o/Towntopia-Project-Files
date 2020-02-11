using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    int levelSaved = 0;

    [SerializeField] private GameObject saveWarningPanel;
    [SerializeField] private GameObject saveWarningPanelTutorial;
    [SerializeField] private GameObject playTutorialPanel;
    [SerializeField] private Animator animator;

    public void LoadNormalMap()
    {
        SceneManager.LoadScene(LevelConstants.NORMAL_LEVEL);
    }

    public void LoadSmallMap()
    {
        levelSaved = PlayerPrefs.GetInt("levelSaved");

        if (levelSaved == 1)
        {
            saveWarningPanel.SetActive(true);
            playTutorialPanel.SetActive(false);
            return;
        }

        animator.SetTrigger("fade");
        StartCoroutine(LoadLevel(LevelConstants.SMALL_LEVEL,2f));
    }

    public void LoadSmallMapInstant()
    {
        StartCoroutine(LoadLevel(LevelConstants.SMALL_LEVEL,2f));
    }

    public void ContinueGame()
    {
        StartCoroutine(LoadLevel(LevelConstants.CONTINUE_GAME,2f));
    }

    public void LoadBigMap()
    {
        StartCoroutine(LoadLevel(LevelConstants.BIG_LEVEL,2f));
    }

    public void LoadMenu()
    {
        StartCoroutine(LoadLevel(LevelConstants.MAIN_MENU,2f));
    }

    //For Tutorials
    public void LoadTutorial_1Instant()
    {
        StartCoroutine(LoadLevel(LevelConstants.TUTORIAL_1,2f));
    }
    public void LoadTutorial_1()
    {
        levelSaved = PlayerPrefs.GetInt("levelSaved");

        if (levelSaved == 1)
        {
            saveWarningPanelTutorial.SetActive(true);
            playTutorialPanel.SetActive(false);
            return;
        }

        animator.SetTrigger("fade");
        StartCoroutine(LoadLevel(LevelConstants.TUTORIAL_1,2f));
    }
    public void LoadTutorial_2()
    {
        StartCoroutine(LoadLevel(LevelConstants.TUTORIAL_2,2f));
    }
    public void LoadTutorial_3()
    {
        StartCoroutine(LoadLevel(LevelConstants.TUTORIAL_3,2f));
    }
    public void LoadTutorial_4()
    {
        StartCoroutine(LoadLevel(LevelConstants.TUTORIAL_4,2f)); 
    }

    private IEnumerator LoadLevel(string level, float time)
    {
        yield return new WaitForSeconds(time);

        if (SceneManager.GetActiveScene().buildIndex != 0)
            PokiUnitySDK.Instance.gameplayStop();
        
        SceneManager.LoadScene(level);
    }
}
