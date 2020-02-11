using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    //Id the buildings, so we can retrieve the right building from the array
    public int id;
    public int cost;
    public string buildingName;
    public string buildingType;
    public int buildingScore;
     
    [SerializeField] private GameObject agent;
    public GameObject currentWorker;

    private AgentJobController thisAgent;
    private City city;

    [Header("Particle Offsets")]
    public Vector3 createParticleOffset;
    public Vector3 destroyParticleOffset;

    [Header("Icon")]
    public GameObject icon;
    [SerializeField] private float nextActionTime = 10f;
    [SerializeField] private float maxActionTime = 20f;
    [SerializeField] private float addCashAmount = 50f;
    [SerializeField] private float addFoodAmount = 5f;
    [Range(0f, 100f)][SerializeField] private float chance = 99f;
    [SerializeField] private float disableTimer = 3f;
    private bool iconChanceFactor;
    private RaycastHit2D hit2D;
    private GameObject iconObject;
    
    private void Start()
    {
        city = FindObjectOfType<City>();

        if (icon != null)
        { 
            icon.transform.LookAt(AnimatorKeeper.Instance.GetMainCamera().transform.position);
            InvokeRepeating("SetIconActive", Random.Range(nextActionTime, maxActionTime), Random.Range(nextActionTime, maxActionTime)); 
        }
    }

    private void Update()
    {
        if (city.iconCounter > 0)
        {
            if(Input.GetMouseButton(0))
            {
                InteractWithIcon();
            }
            else if(Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftShift))
            {
                InteractWithIcon();
            }
        }
        else if (city.iconCounter <= 0)
        {
            CancelInvoke("SetIconActive");
        }
    }

    private void InteractWithIcon()
    {
        hit2D = Physics2D.Raycast(AnimatorKeeper.Instance.GetMainCamera().ScreenToWorldPoint(Input.mousePosition), AnimatorKeeper.Instance.GetMainCamera().transform.forward * 10);
        //out means it is sending a reference type instead of value type
        if (hit2D.collider != null)
        {
            iconObject = hit2D.collider.gameObject;

            if (iconObject != null)
            {
                if (iconObject.CompareTag("CashIcon"))
                {
                    SoundController.instance.PlayCollectSFX();
                    city.DepositCash(addCashAmount);
                    iconObject.SetActive(!iconObject.activeSelf);    
                    city.iconCounter--;         
                }

                if (iconObject.CompareTag("FoodIcon"))
                {
                    SoundController.instance.PlayCollectSFX();
                    city.AddFood(addFoodAmount);
                    iconObject.SetActive(!iconObject.activeSelf); 
                    city.iconCounter--; 
                }
            }
        }   
    }

    private void SetIconActive()
    {
        iconChanceFactor = Random.Range(0f, 100f) > chance;

        if (iconChanceFactor == true)
        {
            icon.SetActive(iconChanceFactor);
            Invoke("DisableIcon", disableTimer);
        }
    }

    private void DisableIcon()
    {
        icon.SetActive(false);
    }

    public void SetNewWorker(GameObject newWorker)
    {
        currentWorker = newWorker;
    }

    public GameObject GetWorker()
    {
        return currentWorker;
    }

    public void ResetWorker()
    {
        if (buildingName == "Farm" || buildingName == "Factory" || buildingName == "Office" || buildingName == "Shop")
        {
            if (currentWorker != null)
            {
                currentWorker.GetComponent<AgentJobController>().Reset();
            }  
        }

        if (currentWorker != null)
        {
            currentWorker.GetComponent<AgentJobController>().ResetDistance();
        }

        DeleteWorker();
    }

    public void DeleteWorker()
    {
        currentWorker = null;
    }

    public void DestroyWorker()
    {
        Destroy(agent);
    }

    public void ActivateAgent()
    {
        agent.SetActive(true);
        agent.transform.parent = null;
    }

    public GameObject GetPooledCreateParticleGameObject()
    {
        switch(buildingType)
        {
            case "Road":
                if (buildingName == "Water")
                {
                    return ParticleCreateWaterPool.Instance.Get().gameObject;
                }
                else
                {
                    return ParticleCreateRoadPool.Instance.Get().gameObject;
                }
            case "Residential":
                return ParticleCreatePool.Instance.Get().gameObject;

            case "Food":
                return ParticleCreatePool.Instance.Get().gameObject;

            case "Shop":
                return ParticleCreatePool.Instance.Get().gameObject;

            case "Industrial":
                return ParticleCreatePool.Instance.Get().gameObject;

            case "Office":
                return ParticleCreateOfficePool.Instance.Get().gameObject;

            case "Decoration":
                return ParticleCreateMonumentPool.Instance.Get().gameObject;

            case "Forest":
                return ParticleCreateForestPool.Instance.Get().gameObject;
            
            default:
                return ParticleCreatePool.Instance.Get().gameObject;;

        }
    }

    public GameObject GetPooledDestroyParticleGameObject()
    {
        switch(buildingType)
        {
            case "Road":
                if (buildingName == "Water")
                {
                    return ParticleDestroyWaterPool.Instance.Get().gameObject;
                }
                else
                {
                    return ParticleDestroyRoadPool.Instance.Get().gameObject;
                }
                return ParticleDestroyRoadPool.Instance.Get().gameObject;

            case "Residential":
                return ParticleDestroyPool.Instance.Get().gameObject;

            case "Food":
                return ParticleDestroyPool.Instance.Get().gameObject;

            case "Shop":
                return ParticleDestroyPool.Instance.Get().gameObject;

            case "Industrial":
                return ParticleDestroyPool.Instance.Get().gameObject;

            case "Office":
                return ParticleDestroyOfficePool.Instance.Get().gameObject;

            case "Decoration":
                return ParticleDestroyMonumentPool.Instance.Get().gameObject;

            case "Forest":
                return ParticleDestroyForestPool.Instance.Get().gameObject;
            
            default:
                return ParticleDestroyPool.Instance.Get().gameObject;;

        }
    }

}
