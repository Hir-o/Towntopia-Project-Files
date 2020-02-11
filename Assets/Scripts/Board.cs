using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Board : MonoBehaviour
{
    //70 by 70 is our largest grid size
    private Building[,] buildings = new Building[70, 70];
    private string      buildingName;
    private string      buildingType;

    private NavMeshSurface navMeshSurface;

    private City     city;
    private Animator resourceAnimator;

    private Building buildingToAdd;
    private Building buildingToRemove;

    [Header("Score Values")]
    [SerializeField] private int roadScoreValue = 100;

    [SerializeField] private int houseScoreValue      = 50;
    [SerializeField] private int farmScoreValue       = 70;
    [SerializeField] private int commercialScoreValue = 80;
    [SerializeField] private int industrialScoreValue = 90;
    [SerializeField] private int officeScoreValue     = 300;

    private GameObject createParticle;
    private GameObject destroyParticle;

    private int buildingToRemoveNumber = 0;

    void Awake()
    {
        navMeshSurface = FindObjectOfType<NavMeshSurface>();
        city           = FindObjectOfType<City>();
    }

    private void Start() { resourceAnimator = AnimatorKeeper.Instance.GetResourceAnimator(); }

    private void Update()
    {
        if (buildingToAdd != null)
        {
            if (buildingToAdd.buildingType == "Road")
            {
                if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.LeftShift) ||
                    Input.GetKeyUp(KeyCode.RightShift))
                {
                    navMeshSurface.BuildNavMesh();
                }
            }
        }

        if (buildingToRemove != null)
        {
            if (buildingToRemove.buildingType == "Road")
            {
                if (Input.GetMouseButtonUp(1) || Input.GetKeyUp(KeyCode.LeftShift) ||
                    Input.GetKeyUp(KeyCode.RightShift))
                {
                    navMeshSurface.BuildNavMesh();
                }
            }
        }
    }

    public void AddBuilding(Building building, Vector3 position, Quaternion rotation)
    {
        buildingToAdd                  = Instantiate(building, position, rotation);
        buildingToAdd.transform.parent = transform;

        if (buildingToAdd.buildingType != "RoadTutorial")
        {
            buildingToAdd.GetComponent<SaveMe_Object>().UpdateObjectNumber();
        }

        buildings[(int) position.x, (int) position.z] = buildingToAdd;

        if (buildingToAdd.buildingName == "House")
        {
            buildingToAdd.ActivateAgent();

            city.CurrentPopulation++;
            resourceAnimator.SetTrigger("gainPop");

            SoundController.instance.PlayBuildingCreate();

            city.IncreaseScore(houseScoreValue);

            //POKI CODE
            if (SceneManager.GetActiveScene().buildIndex >= 5) PokiUnitySDK.Instance.happyTime(0.2f);
        }

        if (buildingToAdd.buildingType == "Road" || buildingToAdd.buildingType == "RoadTutorial")
        {
            if (Input.GetMouseButton(0) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
            {
                SoundController.instance.PlayRoadCreate();
            }
            else
            {
                navMeshSurface.BuildNavMesh();
                SoundController.instance.PlayRoadCreate();
            }

            city.IncreaseScore(roadScoreValue);

            //POKI CODE
            if (SceneManager.GetActiveScene().buildIndex >= 5) PokiUnitySDK.Instance.happyTime(0.1f);
        }

        if (buildingToAdd.buildingName != "House" && buildingToAdd.buildingType != "Road"
                                                  && buildingToAdd.buildingName != "Decoration" &&
                                                  buildingToAdd.buildingType    != "Forest")
        {
            city.CurrentJobs++;
            resourceAnimator.SetTrigger("gainJobs");
        }

        if (buildingToAdd.buildingName == "Factory")
        {
            city.RawResources += 20f;
            resourceAnimator.SetTrigger("gainResources");

            SoundController.instance.PlayBuildingCreate();

            city.IncreaseScore(industrialScoreValue);

            //POKI CODE
            if (SceneManager.GetActiveScene().buildIndex >= 5) PokiUnitySDK.Instance.happyTime(0.4f);
        }
        else if (buildingToAdd.buildingName == "Shop")
        {
            city.RawResources -= 2f;
            resourceAnimator.SetTrigger("loseResources");

            SoundController.instance.PlayShopCreate();

            city.IncreaseScore(commercialScoreValue);

            //POKI CODE
            if (SceneManager.GetActiveScene().buildIndex >= 5) PokiUnitySDK.Instance.happyTime(0.3f);
        }
        else if (buildingToAdd.buildingName == "Office")
        {
            city.IncreaseCurrentResourceFactorLimit();

            SoundController.instance.PlayOfficeCreate();

            city.IncreaseScore(officeScoreValue);

            //POKI CODE
            if (SceneManager.GetActiveScene().buildIndex >= 5) PokiUnitySDK.Instance.happyTime(0.9f);
        }
        else if (buildingToAdd.buildingName == "Farm")
        {
            SoundController.instance.PlayBuildingCreate();

            city.IncreaseScore(farmScoreValue);

            //POKI CODE
            if (SceneManager.GetActiveScene().buildIndex >= 5) PokiUnitySDK.Instance.happyTime(0.2f);
        }
        else if (buildingToAdd.buildingName == "Decoration")
        {
            SoundController.instance.PlayDecorationsCreate();

            city.IncreaseScore(buildingToAdd.buildingScore); // get score from building itself
            //POKI CODE
            if (SceneManager.GetActiveScene().buildIndex >= 5) PokiUnitySDK.Instance.happyTime(1f);
        }
        else if (buildingToAdd.buildingType == "Forest")
        {
            SoundController.instance.PlayForestCreate();

            city.IncreaseScore(buildingToAdd.buildingScore);
            //POKI CODE
            if (SceneManager.GetActiveScene().buildIndex >= 5) PokiUnitySDK.Instance.happyTime(0.4f);
        }

        if (buildingToAdd.name != "Road")
        {
            if (SceneManager.GetActiveScene().buildIndex >= 5) POKISDKController.Instance.PlayCommercialBreak();

            AgentJobController[] agentTests = FindObjectsOfType<AgentJobController>();

            foreach (AgentJobController agent in agentTests)
            {
                if (agent.CurrentJobPlace == null)
                {
                    agent.UpdateCurrentBuildings();
                    agent.CalculateNearestJob();
                }
            }
        }

        //New Pooling Code
        createParticle                    = buildingToAdd.GetPooledCreateParticleGameObject();
        createParticle.transform.position = buildingToAdd.transform.position + buildingToAdd.createParticleOffset;
        createParticle.gameObject.SetActive(true);
        createParticle.transform.parent = null;
        createParticle.gameObject.GetComponent<ParticleSystem>().Play();

        //Old Code
        //buildingToAdd.CreateParticle.Play();
    }

    public void RemoveBuilding(Vector3 position)
    {
        buildingToRemove = buildings[(int) position.x, (int) position.z];
        buildingName     = buildingToRemove.buildingName;
        buildingType     = buildingToRemove.buildingType;

        buildingToRemove.ResetWorker();

        buildingToRemoveNumber = buildingToRemove.GetComponent<SaveMe_Object>().MyNumber;

        if (buildingType != "Road")
        {
            if (buildingName == "House")
            {
                buildingToRemove.DestroyWorker();
                city.CurrentPopulation--;
                resourceAnimator.SetTrigger("losePop");

                SoundController.instance.PlayBuildingDestroy();

                city.DecreaseScore(houseScoreValue);
            }
            else if (buildingName == "Factory")
            {
                city.RawResources -= 20f;

                resourceAnimator.SetTrigger("loseResources");
                city.CurrentJobs--;
                resourceAnimator.SetTrigger("loseJobs");

                SoundController.instance.PlayBuildingDestroy();

                city.DecreaseScore(industrialScoreValue);
            }
            else if (buildingName == "Shop")
            {
                city.RawResources += 2f;

                resourceAnimator.SetTrigger("gainResources");
                city.CurrentJobs--;
                resourceAnimator.SetTrigger("loseJobs");

                SoundController.instance.PlayBuildingDestroy();

                city.DecreaseScore(commercialScoreValue);
            }
            else if (buildingName == "Office")
            {
                city.DecreaseCurrentResourceFactorLimit();
                city.CurrentJobs--;
                SoundController.instance.PlayOfficeDestroy();

                city.DecreaseScore(officeScoreValue);
            }
            else if (buildingName == "Farm")
            {
                city.CurrentJobs--;
                resourceAnimator.SetTrigger("loseJobs");
                SoundController.instance.PlayBuildingDestroy();

                city.DecreaseScore(farmScoreValue);
            }
            else if (buildingName == "Decoration")
            {
                SoundController.instance.PlayDecorationsDestroy();

                city.DecreaseScore(buildingToRemove.buildingScore);
            }
            else if (buildingType == "Forest")
            {
                SoundController.instance.PlayForestDestroy();

                city.DecreaseScore(buildingToRemove.buildingScore);
            }
            else
            {
                city.CurrentJobs--;
                resourceAnimator.SetTrigger("loseJobs");
            }

            if (buildingToRemove.icon != null)
            {
                Destroy(buildingToRemove.icon, .1f);
            }

            Destroy(buildingToRemove.gameObject, .1f);
        }
        else if (buildingType == "Road")
        {
            if (Input.GetMouseButton(1) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
            {
                SoundController.instance.PlayRoadDestroy();

                city.DecreaseScore(roadScoreValue);
            }
            else
            {
                navMeshSurface.BuildNavMesh();
                SoundController.instance.PlayRoadDestroy();
            }

            // navMeshSurface.BuildNavMesh();
            // SoundController.instance.PlayRoadDestroy();
            Destroy(buildingToRemove.gameObject, .1f);
        }

        buildings[(int) position.x, (int) position.z] = null;

        AgentJobController[] agentTests = FindObjectsOfType<AgentJobController>();

        foreach (AgentJobController agent in agentTests)
        {
            if (agent.CurrentJobPlace == null)
            {
                agent.UpdateCurrentBuildings();
                agent.CalculateNearestJob();
            }
        }

        //New Pooling Code
        destroyParticle = buildingToRemove.GetPooledDestroyParticleGameObject();
        destroyParticle.transform.position =
            buildingToRemove.transform.position + buildingToRemove.destroyParticleOffset;
        destroyParticle.gameObject.SetActive(true);
        destroyParticle.transform.parent = null;
        destroyParticle.gameObject.GetComponent<ParticleSystem>().Play();

        SaveMe_Static.TotalObject--;
        // PlayerPrefs.SetInt("SaveMeObjNum", SaveMe_Static.TotalObject);

        foreach (Building b in buildings)
        {
            if (b != null)
            {
                if (b.GetComponent<SaveMe_Object>().MyNumber > buildingToRemoveNumber)
                {
                    b.GetComponent<SaveMe_Object>().MyNumber--;
                    //PlayerPrefs.SetInt("SaveMe" + b.GetComponent<SaveMe_Object>().MyNumber.ToString(), b.GetComponent<SaveMe_Object>().Type);
                    Debug.Log(PlayerPrefs.GetInt("SaveMeObjNum"));
                }
            }
        }

        // Original Code
        // buildingToRemove.DestroyParticle.Play();
        // buildingToRemove.DestroyParticle.transform.parent = null;
        // Destroy(buildingToRemove.DestroyParticle.gameObject, 2f);
    }

    public Building CheckForBuildingAtPosition(Vector3 position)
    {
        return buildings[(int) position.x, (int) position.z];
    }

    //Snap grid to int values
    public Vector3 CalculateGridPosition(Vector3 position)
    {
        return new Vector3(Mathf.Round(position.x), .5f, Mathf.Round(position.z));
    }

    public Building[,] GetCurrentBuildings()
    {
        if (buildings.Length > 0)
        {
            return buildings;
        }

        return null;
    }

    #region LoadBuildings

    public void LoadBuilding(Building buildingToAdd)
    {
        buildingToAdd.transform.parent = transform;

        if (buildingToAdd.buildingName == "House")
        {
            buildingToAdd.ActivateAgent();

            city.CurrentPopulation++;
            resourceAnimator.SetTrigger("gainPop");

            city.IncreaseScore(houseScoreValue);
        }

        if (buildingToAdd.buildingType == "Road" || buildingToAdd.buildingType == "RoadTutorial")
        {
            city.IncreaseScore(roadScoreValue);
        }

        if (buildingToAdd.buildingName != "House" && buildingToAdd.buildingType != "Road"
                                                  && buildingToAdd.buildingName != "Decoration" &&
                                                  buildingToAdd.buildingType    != "Forest")
        {
            city.CurrentJobs++;
            resourceAnimator.SetTrigger("gainJobs");
        }

        if (buildingToAdd.buildingName == "Factory")
        {
            city.RawResources += 20f;
            resourceAnimator.SetTrigger("gainResources");

            city.IncreaseScore(industrialScoreValue);
        }
        else if (buildingToAdd.buildingName == "Shop")
        {
            city.RawResources -= 2f;
            resourceAnimator.SetTrigger("loseResources");

            city.IncreaseScore(commercialScoreValue);
        }
        else if (buildingToAdd.buildingName == "Office")
        {
            city.IncreaseCurrentResourceFactorLimit();

            city.IncreaseScore(officeScoreValue);
        }
        else if (buildingToAdd.buildingName == "Farm")
        {
            city.IncreaseScore(farmScoreValue);
        }
        else if (buildingToAdd.buildingName == "Decoration")
        {
            city.IncreaseScore(buildingToAdd.buildingScore); // get score from building itself
        }
        else if (buildingToAdd.buildingType == "Forest")
        {
            city.IncreaseScore(buildingToAdd.buildingScore);
        }

        if (buildingToAdd.name != "Road")
        {
            AgentJobController[] agentTests = FindObjectsOfType<AgentJobController>();

            foreach (AgentJobController agent in agentTests)
            {
                if (agent.CurrentJobPlace == null)
                {
                    agent.UpdateCurrentBuildings();
                    agent.CalculateNearestJob();
                }
            }
        }

        // createParticle = buildingToAdd.GetPooledCreateParticleGameObject();
        // createParticle.transform.position = buildingToAdd.transform.position + buildingToAdd.createParticleOffset;
        // createParticle.gameObject.SetActive(true);
        // createParticle.transform.parent = null;
        // createParticle.gameObject.GetComponent<ParticleSystem>().Play();
    }

    #endregion

    public Building[,] GetBuildings() { return buildings; }

    public void RebuildNavMesh() { navMeshSurface.BuildNavMesh(); }
}