using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveRendererOnStart : MonoBehaviour
{
    void Start()
    {
        MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
        if (renderer)
        {
            renderer.enabled = false;
        }
    }
}
