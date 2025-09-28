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
using UnityEngine;
using UnityEngine.AI;

public class Follow : State, ICanRotate, IActivator
{
    public Transform target;
    public Attractee attractee;
    public float range = 1.5f;
    public float activationRange = 2f;
    public float timeInRange = 3f;
    public bool canRotateWhileFollowing = true;
    int i = 0;
    float t = 0;
    bool active = false;

    protected override void Enter()
    {
        active = true;
        t = 0;
        Activate(false);
    }

    /// <summary>
    /// GameObjects to be activated when the character is near the target
    /// </summary>
    public GameObject[] activate;

    /// <summary>
    /// GameObjects to be deactivated when the character is near the target
    /// </summary>
    public GameObject[] deactivate;

    void Update()
    {
        i++;
        var targetPosition = attractee ? attractee.position : target.position;
        var vector = targetPosition - transform.position;
        if (target && Vector3.Distance(target.position, transform.position) < range)
        {
            var position = (target.position - (target.position - transform.position).normalized * range);
            NavMesh.SamplePosition(position, out var hit, 10, NavMesh.AllAreas);
            position = hit.position;
            agent.SetDestination(position);
        }
        else
        {

            if ((i & 3) == 0)
            {
                var position = (targetPosition - (targetPosition - transform.position).normalized * range);
                NavMesh.SamplePosition(position, out var hit, 10, NavMesh.AllAreas);
                position = hit.position;
                agent.SetDestination(position);

            }
        }

        if (vector.sqrMagnitude < activationRange * activationRange)
        {
            t += Time.deltaTime;
            if (t > timeInRange)
            {
                Activate(true);
            }
        }
        else
        {
            t = Math.Clamp(t - Time.deltaTime, 0, 100);
        }
    }


    public bool canRotate => canRotateWhileFollowing;

    public void Activate(bool isActive)
    {
        if (active == isActive)
        {
            return;
        }

        active = isActive;
        foreach (var go in activate)
        {
            go.SetActive(isActive);
        }

        foreach (var go in deactivate)
        {
            go.SetActive(!isActive);
        }
    }
}