using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private City city;

    [Header("Building Panels")]
    [SerializeField] private GameObject pathsPanel;
    [SerializeField] private GameObject decorationsPanel;
    [SerializeField] private GameObject treesPanel;

    [Header("Options Panel")]
    [SerializeField] private GameObject optionsPanel;

    [Header("Tutorial Panel")]
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private GameObject resourcesPanel;
    [SerializeField] private GameObject commandsPanel;

    [Header("Controls Panel Hide/Show Images")]
    [SerializeField] private GameObject hideShowPanel;
    [SerializeField] private Animator controlsAnimator;

    //New system variables start here

    [Header("Current Options")]
    [SerializeField] private Text cashText;
    [SerializeField] private Text foodText;
    [SerializeField] private Text rawResourcesText;
    [SerializeField] private Text workersText;
    [SerializeField] private Text populationText;
    [SerializeField] private Text scoreText;

    [Header("Hide/Show Options")]
    [SerializeField] private GameObject grid;

    private void Start() {
        city = GetComponent<City>();

        UpdateCityDataGOAP();
    }

    private void Update()
    {   
        if (Input.GetKey(KeyCode.M) && Input.GetKey(KeyCode.LeftShift))
        {
            city.DepositCash(5000f);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            optionsPanel.SetActive(!optionsPanel.activeSelf);
            
            if (optionsPanel.activeSelf)
                PokiUnitySDK.Instance.gameplayStop();
            else
                PokiUnitySDK.Instance.gameplayStart();
        }

        if (optionsPanel.activeSelf == true && Time.timeScale == 1f)
        {
            Time.timeScale = 0f;
        }
        else if (optionsPanel.activeSelf == false && Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }
    }

    public void UpdateCityDataGOAP()
    {
        cashText.text = Mathf.Round(city.Cash).ToString();
        foodText.text = Mathf.Round(city.Food).ToString();
        rawResourcesText.text = Mathf.Round(city.RawResources).ToString();
        workersText.text = city.CurrentJobs.ToString();
        populationText.text = city.CurrentPopulation.ToString();
        scoreText.text = city.TownScore.ToString();
    }

    public void SetGrid()
    {
        grid.SetActive(!grid.activeSelf);
    }

    public void EnablePathsPanel()
    {
        pathsPanel.SetActive(!pathsPanel.active);
    }

    public void DisablePathsPanel()
    {
        if (pathsPanel.active == true)
        {
            pathsPanel.SetActive(false);
        }
    }

    public void EnableDecorationsPanel()
    {
        decorationsPanel.SetActive(!decorationsPanel.active);
    }

    public void DisableDecorationsPanel()
    {
        if (decorationsPanel.active == true)
        {
            decorationsPanel.SetActive(false);
        }
    }

    public void EnableTreesPanel()
    {
        treesPanel.SetActive(!treesPanel.active);
    }

    public void DisableTreesPanel()
    {
        if (treesPanel.active == true)
        {
            treesPanel.SetActive(false);
        }
    }

    public void DisableTutorialPanel()
    {
        tutorialPanel.SetActive(false);
    }

    public void DisableCommandsPanel()
    {
        commandsPanel.SetActive(false);
    }

    public void DisableResourcesPanel()
    {
        resourcesPanel.SetActive(false);
    }

    public void ControlsDisplay()
    {
        hideShowPanel.transform.localScale = new Vector3(hideShowPanel.transform.localScale.x, -hideShowPanel.transform.localScale.y, hideShowPanel.transform.localScale.z);

        if (controlsAnimator.GetBool("isHidden") == true)
        {
            controlsAnimator.SetBool("isHidden", false);
        }
        else
        {
           controlsAnimator.SetBool("isHidden", true); 
        }
    }
}
