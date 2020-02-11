using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    public float Cash { get; set; }
    public float CurrentPopulation { get; set; }
    public float MaxPopulation { get; set; }
    public int CurrentJobs { get; set; }
    public int MaxJobs { get; set; }
    public float Food { get; set; }
    public float RawResources { get; set; }
    public int TownScore { get; set; }

    [Header("Buildings")]
    //the number amount of a specific building
    public int[] buildingCount = new int[3];
    private UIController uIController;
    public int iconCounter = 200;

    [Header("Starting Resources")]
    [SerializeField] private float startingCash = 200;
    [SerializeField] private float startingFood = 200f;
    [SerializeField] private float startingRawResources = 0;
    [SerializeField] private float officeFactorChange = .4f;
    [SerializeField] private float shopFactorChange = .4f;
    public float cashReturnValueDivider = 1.4f;

    [Header("Penalty Variables")]
    public float officeFactorIncrement = 1f;
    public float cashPenaltyFromFood = 1f;
    public float resourceGainDivider = 24f;

    [Header("UI Animator")]
    [SerializeField] private Animator resourcesAnimator;
    [SerializeField] private Animator scoreAnimator;

    private void Awake()
    {
        uIController = GetComponent<UIController>();
    }

    void Start()
    {
        Cash = startingCash;
        Food = startingFood;
        RawResources = startingRawResources;
    }

    public void DepositCash(float amount)
    {
        if (amount > 0f)
        {
            resourcesAnimator.SetTrigger("gainCash");
        }
        else if (amount < 0f)
        {
            resourcesAnimator.SetTrigger("loseCash");
        }

        Cash += amount;
        uIController.UpdateCityDataGOAP();
    }

    public void AddFood(float amount)
    {
        if (amount > 0f)
        {
            resourcesAnimator.SetTrigger("gainFood");
        }
        else if (amount < 0f)
        {
            resourcesAnimator.SetTrigger("loseFood");
        }

        Food += amount;
        uIController.UpdateCityDataGOAP();
    }

    public void IncreaseScore(int amount)
    {
        TownScore += amount;
        scoreAnimator.SetTrigger("gainScore");
        uIController.UpdateCityDataGOAP();
        //Application.ExternalCall("kongregate.stats.submit", "Town Score", TownScore);
    }

    public void DecreaseScore(int amount)
    {
        TownScore -= amount;
        scoreAnimator.SetTrigger("loseScore");
        uIController.UpdateCityDataGOAP();
        //Application.ExternalCall("kongregate.stats.submit", "Town Score", TownScore);
    }

    public void IncreaseCurrentResourceFactorLimit()
    {
        officeFactorIncrement += officeFactorChange;
    }

    public void DecreaseCurrentResourceFactorLimit()
    {
        officeFactorIncrement -= officeFactorChange;
    }
}
