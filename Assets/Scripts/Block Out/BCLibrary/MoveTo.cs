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
using UnityEngine;
using UnityEngine.ProBuilder;

/// <summary>
/// Abstract class that represents a moveable object that can move to a specific location.
/// </summary>
[DefaultExecutionOrder(-1000)]
public abstract class MoveTo : MonoBehaviour
{
    /// <summary>
    /// The target location to move to.
    /// </summary>
    protected Vector3 location;
    Vector3 start;
    float distance = 1f;
    float startTime = 0;

    void Awake()
    {
        location = transform.position;
        Initialize();
    }

    protected virtual void Initialize()
    {

    }

    public void Goto(Component component)
    {
        Goto(component.transform.position);
    }

    public void Goto(GameObject gameObject)
    {
        Goto(gameObject.transform.position);
    }

    public virtual void Goto(Vector3 destination)
    {
        startTime = Time.time;
        start = transform.position;
        location = destination;
        distance = Vector3.Distance(destination, start);
    }

    protected float t
    {
        get
        {
            var d = Vector3.Distance(transform.position, start);
            return Mathf.Clamp01( float.IsNaN(d/ distance) ? 0 : d/distance);
        }
    }

    protected virtual bool AtDestination()
    {
        return false;
    }

    /// <summary>
    /// The proximity distance within which the object is considered to have reached the target location.
    /// </summary>
    public float proximity = 0.1f;

    /// <summary>
    /// Gets a value indicating whether the object is at the target location.
    /// </summary>
    public bool AtLocation
    {
        get
        {
            if (Time.time - startTime < 0.8f) return false;
            if (AtDestination()) return true;

            var l1 = location;
            var l2 = transform.position;
            l2.y = l1.y;
            return (l2 - l1).sqrMagnitude < proximity * proximity;
        }
    }
}
