
using UnityEngine;

public class MoveRelativeOnActive : Enabler
{
    bool active = false;
    float t = 0;
    public Vector3 offset;
    public Transform target;
    public float speed = 1;
    Vector3 start;

    void Awake()
    {
        start = (target ?? transform).localPosition;
    }
    public AnimationCurve curve;
    protected override void Enable()
    {
        start = target.localPosition;
        active = true;
        t = 0;
    }

    void Update()
    {
        if (active)
        {
            t += Time.deltaTime * speed;
            target.localPosition = Vector3.Lerp(start, start + offset, curve.Evaluate(t));
        }
    }
}