using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentMovementSpeed : MonoBehaviour
{
    [SerializeField] private int updateInterval = 3;
    private float movementSpeed;
    [SerializeField] private float minSpeed = 1f;
    [SerializeField] private float maxSpeed = 1.5f;
    [SerializeField] private float poweredMinSpeed = 2f;
    [SerializeField] private float poweredMaxSpeed = 4f;

    private bool isWalkingRoad = false;
    private bool isResetWhenOutOfFood = false;

    private NavMeshAgent agent;
    private City city;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        city = FindObjectOfType<City>();
    }

    private void Start() 
    {
        ResetSpeed();
        InvokeRepeating("CheckFoodLevel", .5f, .5f);
    }
    
    private void CheckFoodLevel()
    {
        if (city.Food <= 0f)
        {
            if (isResetWhenOutOfFood == false)
            {
                ResetSpeed();
                isResetWhenOutOfFood = true;
            }
            else
            {
                return;
            }
        }
        else
        {
            isResetWhenOutOfFood = false;
            UpdateSpeed();
        }
    }

    public void ResetSpeed()
    {
        movementSpeed = Random.Range(minSpeed, maxSpeed);
        agent.speed = 0f;
        agent.speed = movementSpeed;
    }

    private void UpdateSpeed()
    {
        NavMeshHit navMeshHit;
        if(NavMesh.SamplePosition(agent.transform.position, out navMeshHit, 1f, NavMesh.AllAreas))
        {
            if(navMeshHit.mask == 8)
            {
                if(isWalkingRoad == false)
                {
                    movementSpeed = Random.Range(poweredMinSpeed, poweredMaxSpeed);
                    agent.speed = 0f;
                    agent.speed = movementSpeed;
                    isWalkingRoad = true;
                }
            }
            else
            {
                ResetSpeed();
                isWalkingRoad = false;
            }
        }
    }
}
