using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class POKISDKController : MonoBehaviour
{
    public static POKISDKController Instance;

    [SerializeField] private Animator        rewardButtonAnimator;
    [SerializeField] private TextMeshProUGUI tmpRewardTimer;
    [SerializeField] private float           nextRewardTimer = 300f;

    private float _tempNextRewardTimer;

    private string minutes, seconds;

    private City city;

    private bool isRewardButtonPressed;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        city = FindObjectOfType<City>();

        _tempNextRewardTimer = nextRewardTimer;
    }

    private void Update()
    {
        if (isRewardButtonPressed)
        {
            _tempNextRewardTimer -= Time.deltaTime;

            if (_tempNextRewardTimer <= 0f)
            {
                isRewardButtonPressed = false;
                _tempNextRewardTimer  = nextRewardTimer;
                rewardButtonAnimator.SetTrigger("show");
                
                minutes = Mathf.Floor(_tempNextRewardTimer / 60).ToString("00");
                seconds = (_tempNextRewardTimer % 60).ToString("00");

                tmpRewardTimer.text = string.Format("{0}:{1}", minutes, seconds);
            }
            else
            {
                minutes = Mathf.Floor(_tempNextRewardTimer / 60).ToString("00");
                seconds = (_tempNextRewardTimer % 60).ToString("00");

                tmpRewardTimer.text = string.Format("{0}:{1}", minutes, seconds);
            }
        }
    }

    public void PlayCommercialBreak()
    {
        if (PokiUnitySDK.Instance.adsBlocked()) return;

        PokiUnitySDK.Instance.gameplayStop();

        AudioListener.volume = 0f;

        PokiUnitySDK.Instance.commercialBreakCallBack = commercialBreakComplete;
        PokiUnitySDK.Instance.commercialBreak();
    }

    public void PlayRewardedBreak()
    {
        if (PokiUnitySDK.Instance.adsBlocked()) return;
        if (isRewardButtonPressed) return;

        rewardButtonAnimator.SetTrigger("hide");
        PokiUnitySDK.Instance.gameplayStop();

        AudioListener.volume = 0f;

        PokiUnitySDK.Instance.rewardedBreakCallBack = rewardedBreakComplete;
        PokiUnitySDK.Instance.rewardedBreak();
    }

    public void commercialBreakComplete()
    {
        PokiUnitySDK.Instance.gameplayStart();
        AudioListener.volume = 1f;
    }

    public void rewardedBreakComplete(bool withReward)
    {
        PokiUnitySDK.Instance.gameplayStart();
        city.DepositCash(5000f);
        AudioListener.volume = 1f;

        isRewardButtonPressed = true;
    }

    public void StopGameplay() { PokiUnitySDK.Instance.gameplayStop(); }
}