using UnityEngine;
using UnityEngine.UI;

public class FossilDetector : MonoBehaviour
{
    public Transform player;
    public Image sprite;
    public Color from;
    public Color to;
    public Color close;
    public Color off;

    float speed = 1;
    float amount = 0.5f;
    float time = 0;
    public float range = 12f;
     
    void Update()
    {
        var closest = TrackMe.Closest("digsite", player.position);
        if (!closest)
        {
            sprite.enabled = false;
        }
        else
        {
            var d = Vector3.Distance(player.position, closest.transform.position);
            var activeColor = Color.Lerp(from, to, 1 - d / range);
            if (d < 2.5)
            {
                activeColor = close;
                amount = (d / 2.5f) * 0.5f;
                speed = 5;
            }
            else
            {
                amount = 0.5f;
                speed = Mathf.Lerp(0.3f, 5f, 1- Mathf.Clamp01(d / range));
            }
            time = (time + Time.deltaTime * speed) % 1;
            if (time < amount)
            {
                sprite.color = off;
            }
            else
            {
                sprite.color = activeColor;
            }
        }

    }
}
