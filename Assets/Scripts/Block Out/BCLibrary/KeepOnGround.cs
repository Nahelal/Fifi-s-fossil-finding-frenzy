
using UnityEngine;

public class KeepOnGround : MonoBehaviour
{
    public LayerMask groundLayers;
    public LayerMask blockedLayers;

    Vector3 lastGood;

    void LateUpdate()
    {
        if (Physics.Raycast(transform.position + Vector3.up * 0.2f, Vector3.down, out var hit, 10, groundLayers | blockedLayers))
        {
            if ((blockedLayers & 1 << hit.collider.gameObject.layer) != 0)
            {
                transform.position = lastGood;
            }
        }

        lastGood = transform.position;
    }
}