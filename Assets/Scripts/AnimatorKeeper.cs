using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorKeeper : MonoBehaviour
{
    [SerializeField] private Animator resourceAnimator;
    [SerializeField] private Camera mainCamera;

    public static AnimatorKeeper Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public Animator GetResourceAnimator()
    {
        return resourceAnimator;
    }

    public Camera GetMainCamera()
    {
        return mainCamera;
    }
}
