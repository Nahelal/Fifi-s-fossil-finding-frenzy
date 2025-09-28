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

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents an object that can be attracted by multiple attractors.
/// </summary>
public class Attractee : MonoBehaviour
{
    /// <summary>
    /// The identifier of the attractor that attracts this object.
    /// </summary>
    public string attractedBy;

    /// <summary>
    /// A list of attractors influencing this attractee.
    /// </summary>
    public List<Attractor> attractors = new();

    /// <summary>
    /// The calculated position of the attractee based on the attractors' influence.
    /// </summary>
    public Vector3 position { get; private set; }

    /// <summary>
    /// Indicates whether the attractee is dynamic and can be influenced by attractors.
    /// </summary>
    public bool dynamic = false;

    /// <summary>
    /// Updates the position of the attractee based on the attractors' weights and positions.
    /// </summary>
    void Update()
    {
        var total = 0f;
        foreach (var attractor in attractors)
        {
            if (!attractor.enabled) continue;
            var d = Vector3.Distance(attractor.transform.position, transform.position);
            if (d > attractor.maxRange)
            {
                attractor.weight = 0;
                continue;
            }

            attractor.weight = attractor.attractEffect.Evaluate(d / attractor.maxRange) * attractor.strength;
            total += attractor.weight;
        }

        position = Vector3.zero;
        foreach (var attractor in attractors)
        {
            if (!attractor.enabled) continue;
            position += attractor.transform.position * attractor.weight / total;
        }
    }

    /// <summary>
    /// Called when another collider enters the trigger collider attached to this attractee.
    /// Adds the attractor to the list if it matches the attractedBy identifier.
    /// </summary>
    /// <param name="other">The collider that entered the trigger.</param>
    void OnTriggerEnter(Collider other)
    {
        if (!dynamic) return;
        var attractor = other.GetComponentInParent<Attractor>();
        if (attractor && attractor.attracts == attractedBy)
        {
            attractors.Add(attractor);
        }
    }

    /// <summary>
    /// Called when another collider exits the trigger collider attached to this attractee.
    /// Removes the attractor from the list.
    /// </summary>
    /// <param name="other">The collider that exited the trigger.</param>
    void OnTriggerExit(Collider other)
    {
        if (!dynamic) return;
        var attractor = other.GetComponentInParent<Attractor>();
        if (attractor)
        {
            attractors.Remove(attractor);
        }
    }
}