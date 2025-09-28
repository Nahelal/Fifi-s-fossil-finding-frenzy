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


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : State, ICanRotate
{
    public string target = "wander";
    [Range(0, 100)]
    public float satisfaction = 100;
    [Range(0, 100)]
    public float minimumRange = 4;
    [Range(0, 100)]
    public float maximumRange = 25;
    public int revisitCount = 3;

    readonly LimitedList<TrackMe> lastLocations = new(3);

    void Awake()
    {
        lastLocations.maximum = revisitCount;
    }

    bool WanderPredicate(TrackMe track)
    {
        if (lastLocations.Contains(track)) return false;
        var distanceSq = (track.transform.position - transform.position).sqrMagnitude;
        return distanceSq > minimumRange * minimumRange && distanceSq < maximumRange * maximumRange;
    }


    protected override IEnumerator Flow()
    {
        need?.Reduce(satisfaction);
        using (var poi = TrackMe.Random(target, WanderPredicate)?.Use())
        {

            if (poi)
            {
                lastLocations.Add(poi);
                moveTo.Goto(poi.transform.position);
                yield return new WaitUntil(() => moveTo.AtLocation);
            }
        }

        yield return null;
        needs?.Action();
    }

    public bool canRotate => true;
}
