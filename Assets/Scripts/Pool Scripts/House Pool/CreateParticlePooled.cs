using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateParticlePooled : MonoBehaviour
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

        ParticleCreatePool.Instance.ReturnToPool(this);

        StopCoroutine("CheckIfAlive");
	}
}
