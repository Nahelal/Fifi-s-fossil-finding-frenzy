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

public class Rest : State, ICanRotate
{
    public string target = "";
    public float actionCheckDelayInSeconds = 5f;
    public bool moveOnCharacterProximity = true;
    public string triggerState = "";
    float t = 0;


    bool RestPredicate(TrackMe candidate)
    {
        // Only get targets within 2 metres
        return (transform.position - candidate.transform.position).sqrMagnitude < 2 * 2;
    }

    protected override void Enter()
    {
        if (!DisposeOf(TrackMe.Closest(target, transform.position, RestPredicate)?.Use()))
        {
            needs?.Action();
        }
    }

    void Update()
    {
        t += Time.deltaTime;
        if (t > actionCheckDelayInSeconds)
        {
            t = 0;
            needs?.Action();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (moveOnCharacterProximity && other.GetComponent<CharacterMarker>())
        {
            machine.state = triggerState;
        }
    }

    public bool canRotate => true;
}


