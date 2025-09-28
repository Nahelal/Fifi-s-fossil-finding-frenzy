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


using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(TrackMe))]
public class PointOfInterestInterrupt : MonoBehaviour
{
    [Tooltip("The amount to boost the need by")]
    [Range(0, 100)] public float needBoost;

    TrackMe myPoint;

    readonly HashSet<Needs> seen = new();

    void Awake()
    {
        myPoint = GetComponent<TrackMe>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (myPoint.isUsed) return;
        var needs = other.GetComponentInParent<Needs>();
        if (needs && seen.Add(needs))
        {
            foreach (var need in needs.GetUnfilteredNeeds())
            {
                if (need.value is PointOfInterest interest && interest.targetType == myPoint.type )
                {
                    var value = Random.value * 100;
                    if (value < need.weight + needBoost)
                    {
                        needs.SetState((State)null);
                        interest.target = myPoint;
                        needs.SetState(interest);
                        break;
                    }
                }
            }
        }
    }
}