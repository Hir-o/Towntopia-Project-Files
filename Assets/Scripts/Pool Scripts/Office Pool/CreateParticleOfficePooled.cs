﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateParticleOfficePooled : MonoBehaviour
{
    [SerializeField] private float maxLifeTime = 2f;
    private ParticleSystem particleSystem;

    private void OnEnable()
    {
        StartCoroutine("CheckIfAlive", maxLifeTime);
    }

    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    IEnumerator CheckIfAlive (float timer)
	{	
        yield return new WaitForSeconds(timer);

        ParticleCreateOfficePool.Instance.ReturnToPool(this);

        StopCoroutine("CheckIfAlive");
	}
}
