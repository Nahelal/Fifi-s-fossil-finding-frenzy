using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


[Serializable]
public class SpawnItem
{
    public GameObject prefab;
    public float minYOverlap = -0.1f;
    public float maxYOverlap = -0.05f;
}

public class Spawner : MonoBehaviour
{

    public SpawnItem[] items;
    public int numberToSpawn = 100;
    public int seed = 12345;
    public LayerMask groundLayers;
    public LayerMask testLayers;


    // Start is called before the first frame update
    void Start()
    {
        var state = Random.state;
        Random.InitState(seed);

        var shapeRenderer = GetComponent<MeshRenderer>();
        if (shapeRenderer)
        {
            shapeRenderer.enabled = false;
        }
        if (items.Length == 0) return;
        var spawned = new List<Transform>();

        for (var i = 0; i < numberToSpawn; i++)
        {
            var point = Random.insideUnitCircle;
            point.x *= transform.localScale.x / 2;
            point.y *= transform.localScale.z / 2;
            var castFrom = transform.position;
            castFrom.x += point.x;
            castFrom.z += point.y;
            castFrom.y += 30;
            if (Physics.Raycast(castFrom, Vector3.down, out var hit, 60, testLayers))
            {
                if ((groundLayers.value & (1 << hit.transform.gameObject.layer)) != 0)
                {
                    var location = hit.point;
                    var item = items[Random.Range(0, items.Length)];
                    location.y += Random.Range(item.minYOverlap, item.maxYOverlap);
                    var instance = Instantiate(item.prefab, location,
                        transform.rotation * item.prefab.transform.rotation, hit.transform);
                    instance.isStatic = true;
                    spawned.Add(instance.transform);
                }
            }
        }

        Random.state = state;
    }


}
