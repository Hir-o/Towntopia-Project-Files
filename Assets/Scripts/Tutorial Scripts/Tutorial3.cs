using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial3 : MonoBehaviour
{
    enum panel1State {enable, disable}
    enum panel2State {enable, disable}
    enum panel3State {enable, disable}
    enum panel4State {enable, disable}
    enum panel5State {enable, disable}
    enum panel6State {enable, disable}
    enum panel7State {enable, disable}
    enum panel8State {enable, disable}

    panel1State _panel1State = panel1State.enable;
    panel2State _panel2State = panel2State.disable;
    panel3State _panel3State = panel3State.disable;
    panel4State _panel4State = panel4State.disable;
    panel5State _panel5State = panel5State.disable;
    panel6State _panel6State = panel6State.disable;
    panel7State _panel7State = panel7State.disable;
    panel8State _panel8State = panel8State.disable;

    [SerializeField] private GameObject panel1;
    [SerializeField] private GameObject panel2;
    [SerializeField] private GameObject panel3;
    [SerializeField] private GameObject panel4;
    [SerializeField] private GameObject panel5;
    [SerializeField] private GameObject panel6;
    [SerializeField] private GameObject panel7;
    [SerializeField] private GameObject panel8;

    private Board board;
    private City city;
    private BuildingController buildingController;
    private Building[,] buildings;

    [SerializeField] private Building house;
    [SerializeField] private Building farm;
    [SerializeField] private Building road;

    [Header("Quest Panel Image")]
    [SerializeField] private Image image;

    [Header("Colors")]
    [SerializeField] private Color startColor;
    [SerializeField] private Color finishColor;
    
    [Header("Timers")]
    [SerializeField] private float nextLevelButtonTimer = 4f;
    [SerializeField] private float colorChangeDuration = .3f;

    private float timer = 0f;

    private Quaternion rotation = Quaternion.Euler(0f, 180f, 0f);

    [Header("House Positions")]
    [SerializeField] private Vector3 house1Position;
    [SerializeField] private Vector3 house2Position;
    [SerializeField] private Vector3 house3Position;

    [Header("Farm Position")]
    [SerializeField] private Vector3 farmPosition;

    [Header("Road Positions")]
    [SerializeField] private Vector3 road1Position;
    [SerializeField] private Vector3 road2Position;
    [SerializeField] private Vector3 road3Position;

    [Header("Building Buttons")]
    [SerializeField] private Button roadButton;
    [SerializeField] private Button shopButton;
    [SerializeField] private Button factoryButton;

    private void Awake()
    {
        board = FindObjectOfType<Board>();
        city = FindObjectOfType<City>();
        buildingController = FindObjectOfType<BuildingController>();
    }

    private void Start()
    {
        Invoke("AddHouse", .001f);
    }

    private void Update()
    {
        buildings = board.GetCurrentBuildings();

        foreach(Building b in buildings)
        {
            if (b == null) { continue; }

            if (b.buildingType == "Shop")
            {
                if (_panel2State == panel2State.disable)
                {
                    shopButton.interactable = false;
                    buildingController.SetSelectedBuilding(null);
                    Invoke("EnableShopsPanel", 2f);
                    _panel2State = panel2State.enable;
                }
            }

            if (b.buildingType == "Industrial")
            {
                if(_panel6State == panel6State.disable)
                {
                    factoryButton.interactable = false;
                    buildingController.SetSelectedBuilding(null);
                    Invoke("EnableRoadsPanel", 2f);
                    _panel6State = panel6State.enable;
                }
            }

            if (b.buildingType == "RoadTutorial")
            {
                if(_panel7State == panel7State.disable)
                {
                    Invoke("EnbaleButtonPanel", 2f);
                    _panel7State = panel7State.enable;
                }
            }

            if (_panel7State == panel7State.enable)
            {
                CompleteQuestPanel();
            }
        }
    }

    private void AddHouse()
    {
        Build(house, house1Position, rotation);
        Build(house, house2Position, rotation);
        Build(house, house3Position, rotation);

        Build(farm, farmPosition, rotation);

        Build(road, road1Position, rotation);
        Build(road, road2Position, rotation);
        Build(road, road3Position, rotation);
    }

    private void Build(Building building, Vector3 position, Quaternion rotation)
    {
        city.buildingCount[house.id]++;
        board.AddBuilding(building, position, rotation);
    }

    public void EnableShopsPanel()
    {
         panel2.SetActive(true);
         EnableResourcePanel();
    }

    public void EnableResourcePanel()
    {
        if (_panel3State == panel3State.disable)
        {
            panel3.SetActive(true);
            panel8.SetActive(true);
        }
    }

    public void EnableResourceInfoPanel()
    {
        panel2.SetActive(false);

        if (_panel4State == panel4State.disable)
        {
            panel4.SetActive(true);
            _panel4State = panel4State.enable;
        }
    }

    public void EnableFactoryPanel()
    {
        panel4.SetActive(false);

        factoryButton.interactable = true;
        
        if (_panel5State == panel5State.disable)
        {
            panel5.SetActive(true);
        }
    }

    public void EnableRoadsPanel()
    {
        panel5.SetActive(false);
        panel6.SetActive(true);

        factoryButton.interactable = false;
        roadButton.interactable = true;
    }

    public void EnbaleButtonPanel()
    {
        panel6.SetActive(false);
        panel7.SetActive(true);
        roadButton.interactable = false;
    }

    public void CompleteQuestPanel()
    {
        image.color = Color.Lerp(startColor, finishColor, timer);

        if (timer < 1f)
        {
            timer += Time.deltaTime / colorChangeDuration;
        }
    }
}
