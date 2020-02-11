using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
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
    panel5State _panel5State = panel5State.enable;
    panel6State _panel6State = panel6State.disable;

    [SerializeField] private GameObject panel1;
    [SerializeField] private GameObject panel2;
    [SerializeField] private GameObject panel3;
    [SerializeField] private GameObject panel4;
    [SerializeField] private GameObject panel5;
    [SerializeField] private GameObject panel6;

    [Header("Quest Panel Image")]
    [SerializeField] private Image image;

    [Header("Colors")]
    [SerializeField] private Color startColor;
    [SerializeField] private Color finishColor;

    private Board board;
    private Building[,] buildings;

    [Header("Timers")]
    [SerializeField] private float nextLevelButtonTimer = 4f;
    [SerializeField] private float colorChangeDuration = .3f;

    private float timer = 0f;

    private void Awake()
    {
        board = FindObjectOfType<Board>();
    }

    private void Update()
    {
        buildings = board.GetCurrentBuildings();

        foreach(Building b in buildings)
        {
            if (b == null) { continue; }

            CompleteQuestPanel();
            DisableLeftClickPanel();
            EnableAllPanels();
            DisableRotatePanel();
            Invoke("EnableNextSceneButton", nextLevelButtonTimer);
        }
    }

    public void EnableRotatePanel()
    {
        if (_panel2State == panel2State.disable)
        {
            panel2.SetActive(true);
            _panel2State = panel2State.enable;
        }
        
    }

     public void DisableRotatePanel()
    {
        if (_panel2State == panel2State.enable)
        {
            panel2.SetActive(false);
            _panel2State = panel2State.disable;
        }
        
    }

    public void DisableLeftClickPanel()
    {
       if (_panel1State == panel1State.enable)
        {
            panel1.SetActive(false);
            _panel1State = panel1State.disable;
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

    public void EnableAllPanels()
    {
        if (_panel3State == panel3State.disable)
        {
            panel3.SetActive(true);
            _panel3State = panel3State.enable;
        }

        if (_panel4State == panel4State.disable)
        {
            panel4.SetActive(true);
            _panel4State = panel4State.enable;
        }
    }

    public void EnableNextSceneButton()
    {
        if (_panel6State == panel6State.disable)
        {
            panel6.SetActive(true);
            _panel6State = panel6State.enable;
        }
    }
    
}
