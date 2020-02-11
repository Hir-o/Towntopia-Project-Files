using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveMe_Object : MonoBehaviour {
    int number;
    public int MyNumber;
    public int Type;

    public void UpdateObjectNumber () {
        SaveMe_Static.TotalObject++;
        //PlayerPrefs.SetInt("SaveMeObjNum", SaveMe_Static.TotalObject);
        number = SaveMe_Static.TotalObject;
        MyNumber = number - 1;
    }

    public void Save()
    {
        if (SaveMe_Static.SceneManager.GetComponent<SaveMe_Manager>().ShowDebug == true)
        {
            //Debug.Log("Rot: " + PlayerPrefs.GetFloat("SaveMe" + SaveMe_Static.TotalObject + "rx") + " " + PlayerPrefs.GetFloat("SaveMe" + SaveMe_Static.TotalObject + "ry") + " " + PlayerPrefs.GetFloat("SaveMe" + SaveMe_Static.TotalObject + "rz"));
            //  Debug.ClearDeveloperConsole();
            //Debug.Log("(SaveMe) Saved with number :" + MyNumber.ToString());
        }
        PlayerPrefs.SetInt("SaveMe" + MyNumber.ToString(), Type);
        PlayerPrefs.SetFloat("SaveMe" + MyNumber + "x", Mathf.RoundToInt(transform.position.x));
        PlayerPrefs.SetFloat("SaveMe" + MyNumber + "y", transform.position.y);
        PlayerPrefs.SetFloat("SaveMe" + MyNumber + "z", Mathf.RoundToInt(transform.position.z));

        PlayerPrefs.SetFloat("SaveMe" + MyNumber + "rx", transform.eulerAngles.x);
        PlayerPrefs.SetFloat("SaveMe" + MyNumber + "ry", transform.eulerAngles.y);
        PlayerPrefs.SetFloat("SaveMe" + MyNumber + "rz", transform.eulerAngles.z);

        PlayerPrefs.SetFloat("SaveMe" + MyNumber + "sx", 1f);
        PlayerPrefs.SetFloat("SaveMe" + MyNumber + "sy", 1f);
        PlayerPrefs.SetFloat("SaveMe" + MyNumber + "sz", 1f);
        PlayerPrefs.SetString("SaveMe" + MyNumber + "Name", gameObject.name);

        PlayerPrefs.SetInt("levelSaved", 1);

        //PlayerPrefs.Save();
    }

}
