using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveMe_Manager : MonoBehaviour {

    [Header("Game Elements")]
    [SerializeField] private Board board;
    [SerializeField] private City city;
    [SerializeField] private UIController uIController;

    [Header("UI Elements")]
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private GameObject canvasElements;
    [SerializeField] private Transform canvas;

    [Header("All UI Elements")]
    [SerializeField] private List<GameObject> uIs = new List<GameObject>();
    [SerializeField] private RectTransform bgLoadingPanel;

    public int ObjNumber;
    [Tooltip("Please watch the tutorial")]
    public Building[] AllObjects;
    [Header("Debug")]
    public bool ShowDebug = true;
     int acheived;
     int total;
    [Header("Speed Between 2 Loads (in second)")]
    public float SpeedBt;
    [Space(10)]
    [Header("------Extra------")]
    [Header("Loading Screen")]
    public bool HaveLoadingScreen;
    [Tooltip("Do not put if Have Loading Screen = false")]
    public GameObject LoadingScreen;
    bool isLoading;

    private Building[] all;
    private Building inst;


    string currentSceneName;

    private void Awake()
    {
        total = PlayerPrefs.GetInt("SaveMeObjNum");
        //acheived++;

        currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == LevelConstants.CONTINUE_GAME)
        {
            SaveMe_Static.TotalObject = PlayerPrefs.GetInt("SaveMeObjNum");
        }
    }

    void Start () {
        SaveMe_Static.SceneManager = gameObject;
        if (ShowDebug == true)
        {
            Debug.Log(PlayerPrefs.GetInt("SaveMeObjNum").ToString());
        }

        //uiElements = canvasElements.GetComponentsInChildren<GameObject>();
	}
	
	void Update () {
        ObjNumber = SaveMe_Static.TotalObject;
        if(HaveLoadingScreen == true && isLoading == true)
        {
            LoadingScreen.transform.Find("Text").GetComponent<Text>().text = acheived + "/" + total.ToString();
            if(acheived == total)
            {
                isLoading = false;
            }
        }
        if(HaveLoadingScreen == true && isLoading == false)
        {
            LoadingScreen.SetActive(false);
        }
	}

   public void Save()
    {
        PlayerPrefs.SetInt("ObjectNumber", SaveMe_Static.TotalObject);
        PlayerPrefs.SetInt("SaveMeObjNum", SaveMe_Static.TotalObject);
        PlayerPrefs.SetFloat("Cash", city.Cash);
        PlayerPrefs.SetFloat("Food", city.Food);
        PlayerPrefs.SetFloat("Popups", city.iconCounter);

        if (ShowDebug == true)
        {
            Debug.Log(PlayerPrefs.GetInt("SaveMeObjNum").ToString());
        }
        all = UnityEngine.Object.FindObjectsOfType<Building>();
        foreach(Building alls in all)
        {
            alls.GetComponent<SaveMe_Object>().Save();
        }

        //PlayerPrefs.Save();
    }

    public void Load()
    {
        if(HaveLoadingScreen == true)
        {
            LoadingScreen.SetActive(true);
            isLoading = true;
        }
        StartCoroutine(LoadEnum());   
    }

    public IEnumerator LoadEnum()
    {
        for (int i = 0; i < total; i++)
        {
            inst = Instantiate(AllObjects[PlayerPrefs.GetInt("SaveMe"+ i)]) as Building;
            inst.GetComponent<SaveMe_Object>().MyNumber = i;
            inst.transform.position = new Vector3(PlayerPrefs.GetFloat("SaveMe" + i + "x"), PlayerPrefs.GetFloat("SaveMe" + i + "y"), PlayerPrefs.GetFloat("SaveMe" + i + "z"));
            inst.transform.rotation = Quaternion.Euler(PlayerPrefs.GetFloat("SaveMe" + i + "rx"), PlayerPrefs.GetFloat("SaveMe" + i + "ry"), PlayerPrefs.GetFloat("SaveMe" + i + "rz"));

            board.GetBuildings()[Mathf.RoundToInt(PlayerPrefs.GetFloat("SaveMe" + i + "x")), Mathf.RoundToInt(PlayerPrefs.GetFloat("SaveMe" + i + "z"))] = inst;
            
            if (inst.buildingType == "Road")
            {
                inst.transform.localScale = new Vector3(.1f, 1f, .1f);
            }
            else
            {
                inst.transform.localScale = new Vector3(1f, 1f, 1f);
            }

            board.LoadBuilding(inst);

            inst.name = PlayerPrefs.GetString("SaveMe" + i + "Name");
            acheived+=1;
            yield return new WaitForSeconds(SpeedBt);
        }

        board.RebuildNavMesh();
        
        city.Cash = PlayerPrefs.GetFloat("Cash");
        city.Food = PlayerPrefs.GetFloat("Food");
        city.iconCounter = PlayerPrefs.GetInt("Popups");
        ObjNumber = PlayerPrefs.GetInt("ObjectNumber");

        loadingPanel.SetActive(false);
        canvasElements.SetActive(true);

        foreach (GameObject uiObject in uIs)
        {
            uiObject.transform.parent = canvas;
        }
        
        uIController.UpdateCityDataGOAP();

        canvasElements.SetActive(false);

        bgLoadingPanel.SetAsLastSibling();
    }

    public void ResetSave()
    {
        PlayerPrefs.SetInt("SaveMeObjNum", 0);
        PlayerPrefs.SetInt("ObjectNumber", 0);
        PlayerPrefs.SetInt("levelSaved", 0);
        SaveMe_Static.TotalObject = 0;

        //PlayerPrefs.Save();
    }

}
