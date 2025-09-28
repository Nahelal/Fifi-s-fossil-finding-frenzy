using UnityEngine;

public class TalkingAnimation : MonoBehaviour
{
    public string talkingAnimation;
    Animate animate;

    void Start()
    {
        animate = GetComponentInChildren<Animate>();
    }

    void Talking()
    {
        animate.force = talkingAnimation;
    }

    void NotTalking()
    {
        animate.force = null;
    }
}