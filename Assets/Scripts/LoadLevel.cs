using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    [SerializeField] private SaveMe_Manager saveMe;
    
    private void Start()
    {
        Invoke("LoadGameObjects", .01f);
    }

    private void LoadGameObjects()
    {
        saveMe.Load();
    }
}
