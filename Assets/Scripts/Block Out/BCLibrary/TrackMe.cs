/************************************************************************************
// MIT License
// 
// Copyright (c) 2023 Mr EdEd Productions  
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// ************************************************************************************/


using System;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1100)]
public class TrackMe : MonoBehaviour, IDisposable
{
    static readonly List<TrackMe> Dummy = new();
    static readonly Dictionary<string, List<TrackMe>> Lists = new();
    public string type;

    
    int used = 0;
    static readonly List<TrackMe> Available = new();

    public static List<TrackMe> Get(string type, Func<TrackMe, bool> predicate = null)
    {
        Available.Clear();
        Available.AddRange(Lists.GetValueOrDefault(type, Dummy));
        for (var i = Available.Count - 1; i >= 0; i--)
        {
            if (Available[i].used > 0)
            {
                Available.RemoveAt(i);
            } else if (predicate != null && !predicate(Available[i]))
            {
                Available.RemoveAt(i);
            }
        }
        return Available;
    }

    static bool AlwaysTrue(TrackMe __)
    {
        return true;
    }

    public bool isUsed => used > 0;

    public static TrackMe Random(string type, Func<TrackMe, bool> predicate = null)
    {
        var list = Get(type, predicate);
        if (list.Count == 0) return null;
        var chosen = list[UnityEngine.Random.Range(0, list.Count)];
        return chosen;
    }

    public static TrackMe Closest(string type, Vector3 position, Func<TrackMe, bool> predicate = null)
    {
        var minimumDistance = float.MaxValue;
        TrackMe closest = null;
        foreach (var trackMe in Get(type, predicate))
        {
            var distance = Vector3.Distance(position, trackMe.transform.position);
            if (distance < minimumDistance)
            {
                minimumDistance = distance;
                closest = trackMe;
            }
        }

        return closest;
    }

    public static TrackMe Aim(string type, Vector3 position, Vector3 forward, float maximumDistance = float.MaxValue, float minAccuracy = 0.8f,  Func<TrackMe, bool> predicate = null)
    {
        var bestDot = float.MinValue;
        TrackMe best = null;
        foreach (var trackMe in Get(type, predicate))
        {
            var distance = Vector3.Distance(position, trackMe.transform.position);
            if (distance < maximumDistance)
            {
                var dot = Vector3.Dot(forward.normalized, (trackMe.transform.position - position).normalized);
                if (dot < minAccuracy) continue;
                if (dot > bestDot)
                {
                    bestDot = dot;
                    best = trackMe;
                }
            }
        }

        return best;
    }

    public TrackMe Use(bool use = true)
    {
        used = Mathf.Clamp(used + (use ? 1 : -1), 0, 10000);
        return this;
    }


    void OnEnable()
    {
        if (!Lists.TryGetValue(type, out var list))
        {
            list = new List<TrackMe>();
            Lists[type] = list;
        }
        list.Add(this);
    }

    void OnDisable()
    {
        if (!Lists.TryGetValue(type, out var list)) return;
        list.Remove(this);
        used = 0;
    }

    public void Dispose()
    {
        Use(false);
    }
}
