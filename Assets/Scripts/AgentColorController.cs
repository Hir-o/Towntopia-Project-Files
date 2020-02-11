using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentColorController : MonoBehaviour
{
    [SerializeField] private Color[] colors;
    private MeshRenderer meshRenderer;
    private MeshRenderer capsuleRenderer;
    private float outlineSize;

    private void Awake() 
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        meshRenderer.material.color = colors[Random.Range(0, colors.Length)];
    }

}
