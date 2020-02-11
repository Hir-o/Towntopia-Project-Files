using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMe_SceneManager_Example : MonoBehaviour {

    [Header("Object to spawn")]
    public GameObject Object;
    [Space(10)]
    [Header("Where spawn?")]
    public float X;
    public float x;
    public float Y;
    public float y; 
    public float Z;
    public float z;
    int whatnumber;

    void Start () {
		
	}
	
	void Update () {
		
	}

    public void Spawn()
    {
        GameObject sphere = Instantiate(Object) as GameObject;
        sphere.transform.position = new Vector3(Random.Range(X, x), Random.Range(Y, y), Random.Range(Z,z));
        sphere.name = "sphere" + whatnumber.ToString();
        sphere.transform.localScale = new Vector3(Random.Range(1, 5), Random.Range(1, 2), Random.Range(1, 2));
        sphere.transform.rotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        whatnumber++;
    }

    public void DeletePrefs()
    {
        PlayerPrefs.DeleteAll();
    }

}
