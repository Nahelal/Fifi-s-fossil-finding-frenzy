using UnityEngine;

public class Flying : MoveTo
{
    public float speed = 3f;
    public AnimationCurve offsetCurve;
    public AnimationCurve speedCurve;
    public float offsetAmount = 2;
    public Transform armature;

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, location, Time.deltaTime * speedCurve.Evaluate(t) * speed);
        armature.localPosition = Vector3.up * (offsetAmount * offsetCurve.Evaluate(t));
    }

}
