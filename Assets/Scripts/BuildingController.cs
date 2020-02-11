using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BuildingController : MonoBehaviour
{
    [SerializeField] private City city;
    [SerializeField] private UIController uIController;
    [SerializeField] private Building[] buildings;
    [SerializeField] private Board board;
    [SerializeField] private Animator resourceAnimator;
    private Building selectedBuilding;
    private Building buildingToSpawn;
    private GameObject particle; 

    private int min; // assigned only from OnClick() events
    private int max = -1; // assigned only from OnClick() events
    private int tempCash;

    private void Update() 
    {
        if(buildingToSpawn != null)
        {
            if (buildingToSpawn.buildingName != selectedBuilding.buildingName)
            {
                Destroy(buildingToSpawn.gameObject);
                MoveCurrentPlaceableObjectToMouse();
                RotateBuilding();
            }
        }

        if(selectedBuilding != null)
        {
            MoveCurrentPlaceableObjectToMouse();
            RotateBuilding();
        }

        if(Input.GetMouseButton(0) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && selectedBuilding != null)
        {
            InteractWithBoard(0);
        }
        else if(Input.GetMouseButtonDown(0) && selectedBuilding != null)
        {
            InteractWithBoard(0);
        }

        if(Input.GetMouseButton(1) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
        {
            if (buildingToSpawn != null)
            {
                Destroy(buildingToSpawn.gameObject);
                selectedBuilding = null;
                return;
            }

            InteractWithBoard(1);
        }
        else if(Input.GetMouseButtonDown(1))
        {
            if (buildingToSpawn != null)
            {
                Destroy(buildingToSpawn.gameObject);
                selectedBuilding = null;
                return;
            }

            InteractWithBoard(1);
        }
    }

    private void InteractWithBoard(int action)
    {
        //create a ray using the mouse position in Pixel Coordinates (ScreenPoint) and turn them
        // to a ray at the position of the mouse and the direction the player is looking for
        Ray ray = AnimatorKeeper.Instance.GetMainCamera().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        //out means it is sending a reference type instead of value type
        if (Physics.Raycast(ray, out hit))
        {
            //the point where the ray hit the collider in world space
            Vector3 gridPosition = board.CalculateGridPosition(hit.point);

            //if there is no building at that position build a building
            //if the player isn't clicking over an UI object
            if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                if (action == 0 && board.CheckForBuildingAtPosition(gridPosition) != null)
                {
                    if (selectedBuilding.CompareTag("Road") && board.CheckForBuildingAtPosition(gridPosition).CompareTag("Road"))
                    {
                        if (city.Cash >= selectedBuilding.cost)
                        {
                            if (selectedBuilding.buildingName == board.CheckForBuildingAtPosition(gridPosition).buildingName)
                            {
                                return;
                            }

                            city.buildingCount[selectedBuilding.id]++;

                            board.CheckForBuildingAtPosition(gridPosition).GetComponentInChildren<MeshFilter>().mesh = selectedBuilding.GetComponentInChildren<MeshFilter>().sharedMesh;
                            board.CheckForBuildingAtPosition(gridPosition).GetComponentInChildren<MeshRenderer>().materials = selectedBuilding.GetComponentInChildren<MeshRenderer>().sharedMaterials;
                            
                            particle = board.CheckForBuildingAtPosition(gridPosition).GetPooledCreateParticleGameObject();
                            particle.transform.position = board.CheckForBuildingAtPosition(gridPosition).transform.position + board.CheckForBuildingAtPosition(gridPosition).createParticleOffset;
                            particle.gameObject.SetActive(true);
                            particle.transform.parent = null;
                            particle.gameObject.GetComponent<ParticleSystem>().Play();
                            board.CheckForBuildingAtPosition(gridPosition).buildingName = selectedBuilding.buildingName;

                            SoundController.instance.PlayRoadCreate();
                            resourceAnimator.SetTrigger("loseCash");
                            return;
                        }
                    }
                    else if (selectedBuilding.CompareTag("Forest") && board.CheckForBuildingAtPosition(gridPosition).CompareTag("Forest"))
                    {
                        if (city.Cash >= selectedBuilding.cost)
                        {
                            if (selectedBuilding.buildingName == board.CheckForBuildingAtPosition(gridPosition).buildingName)
                            {
                                return;
                            }
                            
                            city.buildingCount[selectedBuilding.id]++;

                            if (selectedBuilding.cost > board.CheckForBuildingAtPosition(gridPosition).cost)
                            {
                                tempCash = selectedBuilding.cost - board.CheckForBuildingAtPosition(gridPosition).cost;
                                if (city.Cash >= tempCash)
                                {
                                    city.DepositCash(-tempCash);
                                }
                                else
                                {
                                    return;
                                }
                            }
                            else if (selectedBuilding.cost < board.CheckForBuildingAtPosition(gridPosition).cost)
                            {
                                tempCash = board.CheckForBuildingAtPosition(gridPosition).cost - selectedBuilding.cost;
                                city.DepositCash(tempCash / city.cashReturnValueDivider);
                            }
                            Destroy(board.CheckForBuildingAtPosition(gridPosition).gameObject);
                            board.AddBuilding(selectedBuilding, gridPosition, buildingToSpawn.transform.rotation);

                            SoundController.instance.PlayForestCreate();
                            resourceAnimator.SetTrigger("loseCash");
                            return;
                        }
                    }
                }

                if (action == 0 && board.CheckForBuildingAtPosition(gridPosition) == null)
                {
                    if(city.Cash >= selectedBuilding.cost)
                    {
                        city.DepositCash(-selectedBuilding.cost);
                        city.buildingCount[selectedBuilding.id]++;

                        board.AddBuilding(selectedBuilding, gridPosition, buildingToSpawn.transform.rotation);

                        if (max != -1)
                        {
                            selectedBuilding = buildings[Random.Range(min,max)];

                            if (buildingToSpawn != null)
                            {
                                selectedBuilding.transform.rotation = buildingToSpawn.transform.rotation;
                                Destroy(buildingToSpawn.gameObject);
                            }
                        }

                        resourceAnimator.SetTrigger("loseCash");
                        uIController.UpdateCityDataGOAP();
                    }
                }
                else if (action == 1 && board.CheckForBuildingAtPosition(gridPosition) != null)
                {
                    if (SceneManager.GetActiveScene().name == LevelConstants.TUTORIAL_1
                        || SceneManager.GetActiveScene().name == LevelConstants.TUTORIAL_2
                        || SceneManager.GetActiveScene().name == LevelConstants.TUTORIAL_3
                        )
                    {
                        return;
                    }

                    city.DepositCash(board.CheckForBuildingAtPosition(gridPosition).cost / city.cashReturnValueDivider);
                    board.RemoveBuilding(gridPosition);  

                    resourceAnimator.SetTrigger("gainCash");
                    uIController.UpdateCityDataGOAP();
                }
            }
        }
    }

    public void EnableBuilder(int building)
    {   
        if (max != -1)
        {
            max = -1;
        }

        if (buildingToSpawn != null)
        {
            Destroy(buildingToSpawn.gameObject);
        }

        selectedBuilding = buildings[building];
    }

    public void SetMinBuildingIndex(int newMin)
    {
        min = newMin;
    }
    public void EnableBuilderForTheSameType(int newMax)
    {
        max = newMax;
        selectedBuilding = buildings[Random.Range(min,max)];
    }

    private void MoveCurrentPlaceableObjectToMouse()
    {
        Ray ray = AnimatorKeeper.Instance.GetMainCamera().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (buildingToSpawn == null)
            {
                buildingToSpawn = Instantiate(selectedBuilding);
            }

            if (hit.collider.gameObject.CompareTag("Board"))
            {
                buildingToSpawn.gameObject.transform.position = new Vector3(Mathf.Round(hit.point.x), hit.point.y - 0.5f, Mathf.Round(hit.point.z));

                buildingToSpawn.gameObject.SetActive(true);
            }
        }
        else
        {
            if (buildingToSpawn != null)
            {
                buildingToSpawn.gameObject.SetActive(false);
            }
        }
    }

    private void RotateBuilding()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            buildingToSpawn.transform.Rotate(Vector3.up, 90f, Space.World);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            buildingToSpawn.transform.Rotate(Vector3.up, -90f, Space.World);
        }
    }

    public Building GetSelectedBuilding()
    {
        return selectedBuilding;
    }

    public void SetSelectedBuilding(Building _selectedBuilding)
    {
        selectedBuilding = _selectedBuilding;
    }
}
