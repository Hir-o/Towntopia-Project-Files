using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{   
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource efxSource;

    [Header("Audio Clips Music")]
    [SerializeField] private AudioClip[] clips;

    [Header("Audio Clips Effects")]
    [SerializeField] private AudioClip buildingCreate;
    [SerializeField] private AudioClip buildingDestroy;
    [SerializeField] private AudioClip officeCreate;
    [SerializeField] private AudioClip officeDestroy;
    [SerializeField] private AudioClip decorationCreate;
    [SerializeField] private AudioClip decorationDestroy;
    [SerializeField] private AudioClip forestCreate;
    [SerializeField] private AudioClip forestDestroy;
    [SerializeField] private AudioClip roadCreate;
    [SerializeField] private AudioClip roadDestroy;
    [SerializeField] private AudioClip shopCreate;

    [Header("Button SFEffects")]
    [SerializeField] private AudioClip clickSFX;
    [SerializeField] private AudioClip hoverSFX;
    [SerializeField] private AudioClip scrollSFX;

    [Header("Sliders")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider efxSlider;

    [Header("Collectable SFX")]
    [SerializeField] private AudioClip collectCash;
    [SerializeField] private AudioClip popUp;
    
    public static SoundController instance = null;

    private int currentClip = 0;

    void Awake ()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy (gameObject);
        }
    }

    void Start()
    {
        musicSource.volume = VolumeKeeper.instance.musicVolume;
        efxSource.volume = VolumeKeeper.instance.sfxVolume;

        musicSlider.value = musicSource.volume;
        efxSlider.value = efxSource.volume;

        musicSource.clip = clips[currentClip++];
        musicSource.Play();
        Invoke("PlayNextTrack", musicSource.clip.length);
    }

    private void PlayNextTrack()
    {
        musicSource.Stop();

        if (clips.Length > currentClip)
        {
            musicSource.clip = clips[currentClip++];
            musicSource.Play();
        }
        else
        {
            currentClip = 0;
            musicSource.clip = clips[currentClip++];
            musicSource.Play();
        }


       Invoke("PlayNextTrack", musicSource.clip.length); 
    }

    public void PlaySingle(AudioClip clip)
    {
        efxSource.clip = clip;
        
        efxSource.Play();
    }

#region SFXMethods

    public void PlayBuildingCreate()
    {
        efxSource.PlayOneShot(buildingCreate);
    }

    public void PlayBuildingDestroy()
    {
        efxSource.PlayOneShot(buildingDestroy);
    }

    public void PlayOfficeCreate()
    {
        efxSource.PlayOneShot(officeCreate);
    }

    public void PlayOfficeDestroy()
    {
        efxSource.PlayOneShot(officeDestroy);
    }

    public void PlayDecorationsCreate()
    {
        efxSource.PlayOneShot(decorationCreate);
    }

    public void PlayDecorationsDestroy()
    {
        efxSource.PlayOneShot(decorationDestroy);
    }

    public void PlayForestCreate()
    {
        efxSource.clip = forestCreate;
        
        efxSource.Stop();
        efxSource.Play();
    }

    public void PlayForestDestroy()
    {
        efxSource.PlayOneShot(forestDestroy);
    }

    public void PlayRoadCreate()
    {
        efxSource.clip = roadCreate;
        
        efxSource.Stop();
        efxSource.Play();
    }

    public void PlayRoadDestroy()
    {
        efxSource.PlayOneShot(roadDestroy);
    }

    public void PlayShopCreate()
    {
        efxSource.PlayOneShot(shopCreate);
    }

    public void PlayClickSFX()
    {
        efxSource.PlayOneShot(clickSFX);
    }

    public void PlayHoverSFX()
    {
        efxSource.PlayOneShot(hoverSFX);
    }

    public void PlayScrollSFX()
    {
        efxSource.PlayOneShot(scrollSFX);
    }

    public void PlayPopUpSFX()
    {
        efxSource.PlayOneShot(popUp);
    }

    public void PlayCollectSFX()
    {
        efxSource.PlayOneShot(collectCash);
    }

#endregion

    public void ChangeMusicVolume()
    {
        VolumeKeeper.instance.musicVolume = musicSlider.value;
        musicSource.volume = VolumeKeeper.instance.musicVolume;
    }

    public void ChangeSFXVolume()
    {
        VolumeKeeper.instance.sfxVolume = efxSlider.value;
        efxSource.volume = VolumeKeeper.instance.sfxVolume;
    }
}
