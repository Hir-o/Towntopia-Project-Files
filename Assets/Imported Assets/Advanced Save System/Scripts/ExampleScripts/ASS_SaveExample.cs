using UnityEngine;
using System.Collections;

public class ASS_SaveExample : MonoBehaviour
{
    int HealthPoints = 80;
    int Cash = 4000;
    void OnGUI()
    {
        if (GUI.Button(new Rect(10,20, 150, 25), "Save"))
        {
            transform.GetComponent<AdvancedSaveSystem>().variablesValue[0] = HealthPoints.ToString(); //Get health variable
            transform.GetComponent<AdvancedSaveSystem>().variablesValue[1] = Cash.ToString(); //Get cash variable
            transform.GetComponent<AdvancedSaveSystem>().SaveData(1); //Save at slot 1
        }

        if (GUI.Button(new Rect(10, 50, 150, 25), "Load"))
        {
            transform.GetComponent<AdvancedSaveSystem>().LoadData(1); //Load slot 1
            HealthPoints = int.Parse(transform.GetComponent<AdvancedSaveSystem>().variablesValue[0]); //Get loaded health points
            Cash = int.Parse(transform.GetComponent<AdvancedSaveSystem>().variablesValue[1]); //Get loaded cash
            //MAKE SURE YOU CONVERT THE LOADED VARIABLES SINCE IT WOULD BE ALWAYS AS STRING
        }
    }
}
