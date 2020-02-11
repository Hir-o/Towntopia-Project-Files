using UnityEngine;
using System.Collections;

public class ASS_ChangeColor : MonoBehaviour {
    public Transform obj1,obj2;
    Color cColor = Color.white, cyColor = Color.gray;
    int cSlot = 1;
    void Start()
    {
        obj2.GetComponent<Renderer>().material.color = cyColor;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2 - 50,Screen.height / 2 - 250,150,20),"Select Color:");
        if (GUI.Button(new Rect(Screen.width / 2 - 87,Screen.height / 2 - 220,150,20),"Change Cube"))
        {
            if (cColor == Color.white)
            {
                cColor = Color.green;
                obj1.GetComponent<Renderer>().material.color = cColor;
            }
            else if (cColor == Color.green)
            {
                cColor = Color.yellow;
                obj1.GetComponent<Renderer>().material.color = cColor;
            }
            else if (cColor == Color.yellow)
            {
                cColor = Color.red;
                obj1.GetComponent<Renderer>().material.color = cColor;
            }
            else if (cColor == Color.red)
            {
                cColor = Color.blue;
                obj1.GetComponent<Renderer>().material.color = cColor;
            }
            else
            {
                cColor = Color.white;
                obj1.GetComponent<Renderer>().material.color = cColor;
            }
        }
        if (GUI.Button(new Rect(Screen.width / 2 - 87, Screen.height / 2 - 195, 150, 20), "Change Cylinder"))
        {
            if (cyColor == Color.white)
            {
                cyColor = Color.green;
                obj2.GetComponent<Renderer>().material.color = cyColor;
            }
            else if (cyColor == Color.green)
            {
                cyColor = Color.yellow;
                obj2.GetComponent<Renderer>().material.color = cyColor;
            }
            else if (cyColor == Color.yellow)
            {
                cyColor = Color.red;
                obj2.GetComponent<Renderer>().material.color = cyColor;
            }
            else if (cyColor == Color.red)
            {
                cyColor = Color.blue;
                obj2.GetComponent<Renderer>().material.color = cyColor;
            }
            else
            {
                cyColor = Color.white;
                obj2.GetComponent<Renderer>().material.color = cyColor;
            }
        }
        if (GUI.Button(new Rect(Screen.width / 2 - 87, Screen.height / 2 - 160, 150, 30), "Change Slot" + cSlot.ToString()))
        {
            if (cSlot < 3)
                cSlot++;
            else
                cSlot = 1;
        }
        if (GUI.Button(new Rect(Screen.width / 2 - 87, Screen.height / 2 - 125, 150, 20), "Save Slot " + cSlot.ToString()))
        {
            transform.GetComponent<AdvancedSaveSystem>().variablesValue[0] = cColor.r.ToString();
            transform.GetComponent<AdvancedSaveSystem>().variablesValue[1] = cColor.g.ToString();
            transform.GetComponent<AdvancedSaveSystem>().variablesValue[2] = cColor.b.ToString();
            transform.GetComponent<AdvancedSaveSystem>().variablesValue[3] = cyColor.r.ToString();
            transform.GetComponent<AdvancedSaveSystem>().variablesValue[4] = cyColor.g.ToString();
            transform.GetComponent<AdvancedSaveSystem>().variablesValue[5] = cyColor.b.ToString();
            transform.GetComponent<AdvancedSaveSystem>().SaveData(cSlot);
        }
        if (GUI.Button(new Rect(Screen.width / 2 - 87, Screen.height / 2 - 100, 150, 20), "Load Slot " + cSlot.ToString()))
        {
            transform.GetComponent<AdvancedSaveSystem>().LoadData(cSlot);
            cColor.r = float.Parse(transform.GetComponent<AdvancedSaveSystem>().variablesValue[0]);
            cColor.g = float.Parse(transform.GetComponent<AdvancedSaveSystem>().variablesValue[1]);
            cColor.b = float.Parse(transform.GetComponent<AdvancedSaveSystem>().variablesValue[2]);
            obj1.GetComponent<Renderer>().material.color = cColor;
            cyColor.r = float.Parse(transform.GetComponent<AdvancedSaveSystem>().variablesValue[3]);
            cyColor.g = float.Parse(transform.GetComponent<AdvancedSaveSystem>().variablesValue[4]);
            cyColor.b = float.Parse(transform.GetComponent<AdvancedSaveSystem>().variablesValue[5]);
            obj2.GetComponent<Renderer>().material.color = cyColor;
        }
    }
}
