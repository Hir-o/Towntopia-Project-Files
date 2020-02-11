using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial4 : MonoBehaviour
{
    enum panel1State {enable, disable}
    enum panel2State {enable, disable}
    enum panel3State {enable, disable}
    enum panel4State {enable, disable}
    enum panel5State {enable, disable}
    enum panel6State {enable, disable}

    panel1State _panel1State = panel1State.enable;
    panel2State _panel2State = panel2State.disable;
    panel3State _panel3State = panel3State.disable;
    panel4State _panel4State = panel4State.disable;
    panel5State _panel5State = panel5State.disable;
    panel6State _panel6State = panel6State.disable;

    [SerializeField] private GameObject panel1;
    [SerializeField] private GameObject panel2;
    [SerializeField] private GameObject panel3;
    [SerializeField] private GameObject panel4;
    [SerializeField] private GameObject panel5;
    [SerializeField] private GameObject panel6;

    private Board board;
    private City city;
    private BuildingController buildingController;
    private Building[,] buildings;

    [SerializeField] private Building office;
    [SerializeField] private Building forest;
    [SerializeField] private Building monument;
    [SerializeField] private Building water;

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

    [Header("Forest Positions")]
    [SerializeField] private Vector3 forest1Position;
    [SerializeField] private Vector3 forest2Position;
    [SerializeField] private Vector3 forest3Position;

    [Header("Monument Position")]
    [SerializeField] private Vector3 monumentPosition;

    [Header("Water Position")]
    [SerializeField] private Vector3 water1Position;
    [SerializeField] private Vector3 water2Position;
    [SerializeField] private Vector3 water3Position;
    [SerializeField] private Vector3 water4Position;
    [SerializeField] private Vector3 water5Position;
    [SerializeField] private Vector3 water6Position;

    [Header("Button")]
    [SerializeField] private Button officeButton;

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

            if (b.buildingType == "Office")
            {
                if (_panel2State == panel2State.disable)
                {
                    officeButton.interactable = false;
                    Invoke("EnableOfficePanel", 2f);
                    _panel2State = panel2State.enable;
                }
            }

            if (_panel6State == panel6State.enable)
            {
                CompleteQuestPanel();
            }
        }
    }

    private void AddHouse()
    {
        Build(forest, forest1Position, rotation);
        Build(forest, forest2Position, rotation);
        Build(forest, forest3Position, rotation);

        Build(monument, monumentPosition, rotation);

        Build(water, water1Position, rotation);
        Build(water, water2Position, rotation);
        Build(water, water3Position, rotation);
        Build(water, water4Position, rotation);
        Build(water, water5Position, rotation);
        Build(water, water6Position, rotation);
    }

    private void Build(Building building, Vector3 position, Quaternion rotation)
    {
        city.buildingCount[forest.id]++;
        board.AddBuilding(building, position, rotation);
    }

    public void EnableOfficePanel()
    {
        panel2.SetActive(true);
    }

    public void EnableDecorationPanel()
    {
        panel2.SetActive(false);
        panel3.SetActive(true);
    }

    public void EnableWaterPanel()
    {
        panel3.SetActive(false);
        panel4.SetActive(true);
    }

    public void EnableDestroyBuildingPanel()
    {
        panel4.SetActive(false);
        panel5.SetActive(true);
    }
    

    public void EnableButtonPanel()
    {
        panel5.SetActive(false);
        panel6.SetActive(true);

        _panel6State = panel6State.enable;
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
