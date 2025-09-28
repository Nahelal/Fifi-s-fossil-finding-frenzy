using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutObstacles : MonoBehaviour
{

    public Material fadedMaterial;
    public Material normalMaterial;
    public float transparentLevel = 0.3f;
    public float speed = 2f;
    readonly List<MeshRenderer> transparent = new();
    readonly List<MeshRenderer> opaque = new();

    Dictionary<MeshRenderer, Material> originals = new();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obscurer"))
        {
            var meshRenderer = other.GetComponent<MeshRenderer>();
            if (!originals.ContainsKey(meshRenderer))
            {
                originals[meshRenderer] = meshRenderer.sharedMaterial;
            }
            meshRenderer.sharedMaterial = fadedMaterial;
            opaque.Remove(meshRenderer);
            transparent.Add(meshRenderer);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Obscurer"))
        {
            var meshRenderer = other.GetComponent<MeshRenderer>();
            transparent.Remove(meshRenderer);
            opaque.Add(meshRenderer);
        }
    }

    private void Update()
    {
        foreach (var meshRenderer in transparent)
        {
            var colour = meshRenderer.material.color;
            colour.a = Mathf.MoveTowards(colour.a, transparentLevel, Time.deltaTime * speed);
            meshRenderer.material.color = colour;
        }
        for (var i = opaque.Count - 1; i >= 0; i--)
        {
            var meshRenderer = opaque[i];
            var colour = meshRenderer.material.color;
            colour.a = Mathf.MoveTowards(colour.a, 1, Time.deltaTime * speed);
            meshRenderer.material.color = colour;
            if (colour.a > 0.999)
            {
                meshRenderer.sharedMaterial = originals[meshRenderer] ?? normalMaterial;
                opaque.RemoveAt(i);
            }
        }
    }


}
