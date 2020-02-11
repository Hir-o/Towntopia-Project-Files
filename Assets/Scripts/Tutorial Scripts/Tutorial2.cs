using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial2 : MonoBehaviour
{
    enum panel1State {enable, disable}
    enum panel2State {enable, disable}
    enum panel3State {enable, disable}
    enum panel4State {enable, disable}
    enum panel5State {enable, disable}
    enum panel6State {enable, disable}
    enum panel7State {enable, disable}

    panel1State _panel1State = panel1State.enable;
    panel2State _panel2State = panel2State.disable;
    panel3State _panel3State = panel3State.disable;
    panel4State _panel4State = panel4State.disable;
    panel5State _panel5State = panel5State.disable;
    panel6State _panel6State = panel6State.disable;
    panel7State _panel7State = panel7State.disable;

    [SerializeField] private GameObject panel1;
    [SerializeField] private GameObject panel2;
    [SerializeField] private GameObject panel3;
    [SerializeField] private GameObject panel4;
    [SerializeField] private GameObject panel5;
    [SerializeField] private GameObject panel6;
    [SerializeField] private GameObject panel7;

    private Board board;
    private City city;
    private Building[,] buildings;

    [SerializeField] private Building house;

    [Header("Quest Panel Image")]
    [SerializeField] private Image image;

    [Header("Colors")]
    [SerializeField] private Color startColor;
    [SerializeField] private Color finishColor;
    
    [Header("Timers")]
    [SerializeField] private float nextLevelButtonTimer = 4f;
    [SerializeField] private float colorChangeDuration = .3f;

    private float timer = 0f;

    private void Awake()
    {
        board = FindObjectOfType<Board>();
        city = FindObjectOfType<City>();
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

            if (b.buildingType == "Food")
            {
                if (_panel2State == panel2State.disable)
                {
                    Invoke("EnableJobsPanel", 2f);
                    _panel2State = panel2State.enable;
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
        city.buildingCount[house.id]++;
        board.AddBuilding(house, new Vector3(1.994637f, 0.534f, 2.946f), Quaternion.Euler(0f, 90f, 0f));
    }

    public void EnableJobsPanel()
    {
        panel2.SetActive(true);
    }

    public void EnablePodPeoplePanel()
    {
        panel2.SetActive(false);

        if (_panel4State == panel4State.disable)
        {
            panel4.SetActive(true);
        }
    }

    public void EnableCameraPanel()
    {
        panel3.SetActive(false);
        panel4.SetActive(false);

        if (_panel5State == panel5State.disable)
        {
            panel5.SetActive(true);
        }
    }

    public void EnableCameraZoomPanel()
    {
        panel5.SetActive(false);

        if (_panel6State == panel6State.disable)
        {
            panel6.SetActive(true);
        }
    }

    public void EnableButtonPanel()
    {
        panel6.SetActive(false);

        if (_panel7State == panel7State.disable)
        {
            panel7.SetActive(true);
            _panel7State = panel7State.enable;
        }
    }

    public void EnableResourcePanel()
    {
        if (_panel3State == panel3State.disable)
        {
            panel3.SetActive(true);
        }
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
