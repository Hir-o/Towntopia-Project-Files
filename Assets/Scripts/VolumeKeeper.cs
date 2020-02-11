using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeKeeper : MonoBehaviour
{
    public float musicVolume = .3f;
    public float sfxVolume = .4f;

    public static VolumeKeeper instance = null;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        VolumeKeeper[] volumeKeepers = FindObjectsOfType<VolumeKeeper>();

        if (volumeKeepers.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
