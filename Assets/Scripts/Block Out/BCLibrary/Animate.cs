using UnityEngine;

public class Animate : MonoBehaviour
{
    Vector3 lastPosition, lastRotation;
    public string force;
    public string walk;
    public string idle;

    string currentAnimation;
    public Animator animator;
    public float threshold = 1;
    public float angleThreshold = 5;

    void Start()
    {
        if (!animator)
        {
            animator = GetComponentInChildren<Animator>(true);
        }
    }

    void PlayAnimation(string name)
    {
        if (name == currentAnimation) return;
        animator.CrossFade(name, 0.2f, 0);
        currentAnimation = name;
    }

    void LateUpdate()
    {
        var distance = Vector3.Distance(lastPosition, transform.position) / Time.deltaTime;
        var rotationDistance = Mathf.Abs(Mathf.DeltaAngle(lastRotation.y, transform.eulerAngles.y) / Time.deltaTime);

        lastRotation = transform.eulerAngles;
        lastPosition = transform.position;
        if (force?.Length > 0)
        {
            PlayAnimation(force);
        }
        else if (distance > threshold || rotationDistance > angleThreshold)
        {
            PlayAnimation(walk);
        }
        else
        {
            PlayAnimation(idle);
        }
    }
}