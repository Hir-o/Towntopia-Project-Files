using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticleMonumentPooled : MonoBehaviour
{
    [SerializeField] private float maxLifeTime = 2f;
    private float lifeTime;
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

        ParticleDestroyMonumentPool.Instance.ReturnToPool(this);

        StopCoroutine("CheckIfAlive");
	}
}
