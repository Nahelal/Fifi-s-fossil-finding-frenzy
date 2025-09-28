/************************************************************************************
MIT License

Copyright (c) 2023 Mr EdEd Productions  

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
************************************************************************************/

using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// Footstep or other regular movement script
/// </summary>
public class SoundEffectOnMoving : AudioPlayer
{
    [Range(0, 1)]
    public float volume = 1;
    [Range(0, 5)]
    [Tooltip("Distance moved for a clip to be played")]
    public float movementDistance = 1f;

    [Serializable]
    public class ZoneEffect
    {
        public string zone;
        public AudioClip[] effects;
        [HideInInspector]
        public int index;
    }
    public ZoneEffect[] zones;

    ZoneEffect currentEffect;
    float d = 0;
    Vector3 lastPosition;
    List<EffectZone> zoneStack = new();


    void Start()
    {
        lastPosition = transform.position;
        lastPosition.y = 0;
    }

    void Update()
    {
        var current = transform.position;
        current.y = 0;
        var distance = Vector3.Distance(current, lastPosition);
        lastPosition = current;
        d += distance;
        if (d > movementDistance)
        {
            if (currentEffect != null)
            {
                var clip = currentEffect.effects[currentEffect.index++ % currentEffect.effects.Length];
                source.PlayOneShot(clip, volume);
            }
            d %= movementDistance;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        var zone = other.transform.GetComponentInParent<EffectZone>();
        if (zone)
        {
            zoneStack.Add(zone);
            SetEffect();
        }
    }

    void OnTriggerExit(Collider other)
    {
        var zone = other.transform.GetComponentInParent<EffectZone>();
        if (zone)
        {
            zoneStack.Remove(zone);
            SetEffect();
        }
    }

    void SetEffect()
    {
        if (zoneStack.Count > 0)
        {
            var zone = zoneStack[^1];
            foreach (var effect in zones)
            {
                if (effect.zone == zone.zone)
                {
                    currentEffect = effect;

                }
            }
        }
    }

}
