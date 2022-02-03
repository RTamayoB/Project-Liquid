using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{
    [SerializeField] private Material solidMaterial;
    [SerializeField] private Material transparentMaterial;
    private MeshRenderer renderer;

    private void Start()
    {
        renderer = gameObject.GetComponent<MeshRenderer>();
    }


    internal void MakeTransparent()
    {
        renderer.material = transparentMaterial;
    }

    internal void MakeSolid()
    {
        renderer.material = solidMaterial;
    }

    private void OnDestroy()
    {
        
    }
}
