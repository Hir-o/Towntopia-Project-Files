using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AgentJobController : MonoBehaviour
{
    private Building[,] buildings;
    private City city;
    private Board board;
    private NavMeshAgent agent;
    private Inventory inventory;

    private float oldDistance = 999999999f;
    private float newDistance;

    private bool isFirstTime = true;

    public Building CurrentJobPlace;
    [SerializeField] private Building tempBuilding;

    [SerializeField] private float lifeTime;
    [SerializeField] private bool completed = false;
    [SerializeField] private bool isRunning = false;
    public bool isReset = false;

    private Animator resourceAnimator;

    [Header("Destinations")]
    [SerializeField] private GameObject targetDestination;
    [SerializeField] private Transform home;

    [Header("Resource Variables")]
    [SerializeField] private float foodConsumedAmount = 1f;
    [SerializeField] private float workerUpkeep = 1f;
    [SerializeField] private float foodProducedAmount = 8f;
    [SerializeField] private float cashGeneratedAmount = 10f;
    [SerializeField] private float cashGeneratedAmountFromShop = 5f;
 
    [Header("Work, Rest & Upkeep Variables")]
    [SerializeField] private float workDuration = 2f; // in seconds
    [SerializeField] private float restDuration = 4f;
    [SerializeField] private float upkeepTime = 4f;

    private Vector3 toTarget;
    private float turnAngle;

    private void Awake() 
    {
        agent = GetComponent<NavMeshAgent>();
        inventory = GetComponent<Inventory>();
        board = FindObjectOfType<Board>();
        buildings = board.GetCurrentBuildings();
        city = FindObjectOfType<City>();
        resourceAnimator = AnimatorKeeper.Instance.GetResourceAnimator();
    }

    private void Start()
    {
        InvokeRepeating("PayUpkeep", upkeepTime, upkeepTime);

        lifeTime = Random.Range(3f, 5f);
    }

    private void Update() 
    {
        if (agent.hasPath)
		{
			toTarget = agent.steeringTarget - this.transform.position;
         	turnAngle = Vector3.Angle(this.transform.forward,toTarget);
         	agent.acceleration = turnAngle * agent.speed;
		}

        if (isFirstTime == true && targetDestination != null)
        {
            agent.SetDestination(targetDestination.transform.position);
            isFirstTime = false;
        }

        if (targetDestination == null)
        {
            if (agent.transform.position.x != home.position.x && agent.transform.position.z != home.position.z)
            {
                Reset();
            }
            return;
        }

        // if (targetDestination == null) { return; }

        if (completed == false)
        {
            if (isRunning == false)
            {
                if (agent.transform.position.x == targetDestination.transform.position.x
                && agent.transform.position.z == targetDestination.transform.position.z)
                {
                    StartCoroutine(PerformAction());
                }
            }
        }
        else if (completed == true) 
        {
            if (isRunning == true)
            {
                if (agent.transform.position.x == home.position.x && agent.transform.position.z == home.position.z)
                {
                    if (isReset == true)
                    {
                        isReset = false;
                    }
                    StartCoroutine(RestAtHome());
                }
            }
        }

        // if (agent.transform.position.x == home.position.x && agent.transform.position.z == home.position.z)
        // {
        //     if (agent.destination != null)
        //     {
        //         if (lifeTime > 0f)
        //         {
        //             lifeTime -= Time.deltaTime;
        //         }
        //         else if (lifeTime < 0)
        //         {
        //             isRunning = true;
        //             completed = true;
        //         }
        //     }
        // }

        // if (CurrentJobPlace == null && 
        //     agent.transform.position.x != home.position.x && agent.transform.position.z != home.position.z)
        // {
        //     Reset();
        // }
    }

    public void CalculateNearestJob()
    {
        foreach (Building b in buildings)
        {
            if (b == null) { continue; }
            if (b.buildingName == "House" || b.buildingType == "Road" || b.buildingName == "Decoration" || b.buildingType == "Forest") { continue; }
            if (b.GetWorker() != null) { continue; }
            if (targetDestination != null) { continue; }
            if (tempBuilding != null && b == tempBuilding) { continue; }

            newDistance = Vector3.Distance(b.transform.position, transform.position);

            if(newDistance < oldDistance)
            {
                oldDistance = newDistance;
                Transform[] childrens = b.GetComponentsInChildren<Transform>();

                targetDestination = childrens[2].gameObject;

                if(CurrentJobPlace == null)
                {
                    CurrentJobPlace = b;
                }
                else
                {
                    CurrentJobPlace.DeleteWorker();
                    CurrentJobPlace = b;
                }

                // CurrentJobPlace = b;
                CurrentJobPlace.SetNewWorker(this.gameObject);

                //PerformAction();
            }
        }
    }

    public IEnumerator PerformAction()
    {
        isRunning = true;

        yield return new WaitForSeconds(workDuration);

        completed = true;

        if (CurrentJobPlace.buildingName == "Farm")
        {
            inventory.AddFood(foodProducedAmount);
            resourceAnimator.SetTrigger("gainFood");
        }
        else if (CurrentJobPlace.buildingName == "Factory")
        {
            if (city.Food <= 0f)
            {
                inventory.AddCash(cashGeneratedAmount - city.cashPenaltyFromFood);
                if (cashGeneratedAmount - city.cashPenaltyFromFood <= 0f)
                {
                    resourceAnimator.SetTrigger("loseCash");
                }
                else
                {
                    resourceAnimator.SetTrigger("gainCash");
                }
            }
            else
            {
                inventory.AddCash(cashGeneratedAmount);
                resourceAnimator.SetTrigger("gainCash");
            }
        }
        else if (CurrentJobPlace.buildingName == "Shop")
        {
            if (city.Food <= 0f)
            {
                if (city.RawResources < 0f)
                {
                    inventory.AddCash(((cashGeneratedAmountFromShop * city.officeFactorIncrement) - city.cashPenaltyFromFood) + city.RawResources/city.resourceGainDivider);

                    if (((cashGeneratedAmountFromShop * city.officeFactorIncrement) - city.cashPenaltyFromFood) + city.RawResources/city.resourceGainDivider <= 0f)
                    {
                        resourceAnimator.SetTrigger("loseCash");
                    }
                    else
                    {
                        resourceAnimator.SetTrigger("gainCash");
                    }
                }
                else
                {
                    inventory.AddCash((cashGeneratedAmountFromShop * city.officeFactorIncrement) - city.cashPenaltyFromFood);

                    if ((cashGeneratedAmountFromShop * city.officeFactorIncrement) - city.cashPenaltyFromFood <= 0f)
                    {
                        resourceAnimator.SetTrigger("loseCash");
                    }
                    else
                    {
                        resourceAnimator.SetTrigger("gainCash");
                    }
                }   
            }
            else if (city.RawResources < 0f)
            {
                inventory.AddCash((cashGeneratedAmountFromShop * city.officeFactorIncrement) + city.RawResources/city.resourceGainDivider);
                
                if ((cashGeneratedAmountFromShop * city.officeFactorIncrement) + city.RawResources/city.resourceGainDivider <= 0f)
                {
                    resourceAnimator.SetTrigger("loseCash");
                }
                else
                {
                    resourceAnimator.SetTrigger("gainCash");
                }
                
            }
            else
            {
                inventory.AddCash(cashGeneratedAmountFromShop * city.officeFactorIncrement);
                resourceAnimator.SetTrigger("gainCash");
            }
        }

        agent.GetComponent<AgentMovementSpeed>().ResetSpeed();
        agent.SetDestination(home.position);
    }

    public IEnumerator RestAtHome()
    {   
        if (isReset == false)
        {
            isRunning = false;
        }

        yield return new WaitForSeconds(restDuration);

        if (isReset == false)
        {
            completed = false;
        }

        if (targetDestination != null)
        {
            agent.SetDestination(targetDestination.transform.position);
        }
    }

    public void UpdateCurrentBuildings()
    {
        buildings = board.GetCurrentBuildings();
    }

    public void ResetDistance()
    {
        oldDistance = 99999999999f;
    }
    
    public void Reset()
    {
        tempBuilding = CurrentJobPlace;
        agent.SetDestination(home.position);
        completed = true;
        isRunning = true;
        isReset = true;
        targetDestination = null;
        ResetDistance();
        FireAgentFromJob();
    }

    public void FireAgentFromJob()
    {
        if (CurrentJobPlace != null)
        {
            CurrentJobPlace.DeleteWorker();
            CalculateNearestJob();
        }
    }

    private void PayUpkeep()
    {
        if (city.Food >= 0f)
        {
            inventory.DecreaseFood(foodConsumedAmount);
            inventory.DecreaseCash(workerUpkeep);

            resourceAnimator.SetTrigger("loseCash");
            resourceAnimator.SetTrigger("loseFood");
        }
    }

}
