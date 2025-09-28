using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(
    fileName = "New AnimationFrames",    // default name for new assets
    menuName = "Animation/Animation Frames",  // the menu path
    order = 1                             // optional ordering
)]
public class AnimationFrames : ScriptableObject
{
    [Serializable]
    public class AnimationFrame
    {
        public Sprite sprite;
        public float duration = 0.1f;
    }
    public AnimationFrame[] frames;
    public IEnumerator Animate(Image image)
    {
        var frameNumber = 0;
        while (true)
        {
            var frame = frames[frameNumber % frames.Length];
            if (frame == null) throw new Exception("No frame");
            image.sprite = frame.sprite;
            yield return new WaitForSeconds(frame.duration);
            frameNumber++;
        }
    }
}