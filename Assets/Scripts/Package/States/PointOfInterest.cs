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
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

/// <summary>
/// Represents a point of interest (POI) state that handles targeting, activation/deactivation of associated GameObjects, and timing behavior for interactions at a point of interest.
/// </summary>
public class PointOfInterest : State, ICanRotate, ICanTalk
{
    /// <summary>
    /// The type of target that this point of interest will focus on.
    /// </summary>
    [Space(2)]
    [Header("Point Of Interest")]
    [Tooltip("The type of POI to use")]
    public string targetType = "poi";

    [Space(2)]
    [Header("Filter")]
    [Tooltip("Maximum distance in meters for the poi from the character")]
    public float maxRange = 100;
    [Tooltip("Number of recent locations not to revisit")]
    public int skipRecentLocations = 2;

    /// <summary>
    /// An array of GameObjects to activate when the point of interest is engaged.
    /// </summary>
    [Space(15)]
    [Header("--------------------------------------------------------------------------------------------------------------------------------------------\"")]
    [Header("On Arrival")]
    [Tooltip("GameObjects to activate on arrival, they will be deactivated when leaving")]
    public GameObject[] activate;

    /// <summary>
    /// An array of GameObjects to deactivate when the point of interest is engaged.
    /// </summary>
    [Tooltip("GameObjects to deactivate on arrival, they will be reactivated when leaving")]
    public GameObject[] deactivate;

    [Space(10)]
    [Tooltip("Will the dialog option be available if the NPC is performing the POI task")]
    public bool canTalkWhileAtPOI = true;

    /// <summary>
    /// The minimum duration for which the point of interest will be active.
    /// </summary>
    [Space(15)]
    [Header("--------------------------------------------------------------------------------------------------------------------------------------------\"")]
    [Header("Duration")]
    [Range(1, 120)] public float minDuration = 5f;

    /// <summary>
    /// The maximum duration for which the point of interest will be active.
    /// </summary>
    [Range(1, 120)] public float maxDuration = 15f;

    float duration;

    /// <summary>
    /// The target TrackMe object that this point of interest will interact with.
    /// </summary>
    [HideInInspector] public TrackMe target;


    /// <summary>
    /// The speed at which the object will turn to face the target.
    /// </summary>
    [Space(15)]
    [Header("Facing after arrival")]
    [Tooltip("How fast should the character turn to the desired angle")]
    [Range(0, 360)] public float turnSpeed = 120;

    /// <summary>
    /// The acceptable angle difference for the object to consider itself facing the target.
    /// </summary>
    [Tooltip("How close to the target angle should the character rotate on arrival")]
    [Range(0, 45)] public float angleDifference = 5f;

    /// <summary>
    /// The satisfaction level associated with completing this point of interest.
    /// </summary>
    [Space(15)]
    [Header("Need Update")]
    [Tooltip("How much to reduce the need by for each activation")]
    [Range(0, 100)] public float satisfaction = 0;

    /// <summary>
    /// A timer variable used for tracking the duration of the point of interest activity.
    /// </summary>
    [HideInInspector] public float t = 0;

    readonly LimitedList<TrackMe> lastLocations = new(2);


    /// <summary>
    /// Determines whether the specified TrackMe object can be considered for this point of interest.
    /// </summary>
    /// <param name="track">The TrackMe object to evaluate.</param>
    /// <returns>True if the TrackMe object is not in the last locations list; otherwise, false.</returns>
    bool PoiPredicate(TrackMe track)
    {
        if (lastLocations.Contains(track)) return false;
        if ((track.transform.position - transform.position).sqrMagnitude > maxRange * maxRange) return false;
        return true;
    }

    /// <summary>
    /// Can only enter this state if there is a valid POI
    /// </summary>
    /// <returns></returns>
    public override bool CanEnter()
    {
        return TrackMe.Random(targetType, PoiPredicate);
    }

    /// <summary>
    /// Coroutine that manages the flow of actions for the point of interest, including targeting, activation, and timing.
    /// </summary>
    /// <returns>An enumerator that can be used to iterate through the coroutine.</returns>
    protected override IEnumerator Flow()
    {
        canRotate = true;
        canTalk = true;
        if (!target)
        {
            target = TrackMe.Random(targetType, PoiPredicate);
        }

        if (!target)
        {
            needs.Action();
            yield break;
        }
        DisposeOf(target.Use());
        need.Reduce(satisfaction);

        lastLocations.Add(target);

        var agent = GetComponent<NavMeshAgent>();

        duration = Random.Range(minDuration, maxDuration);
        moveTo.Goto(target);
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => moveTo.AtLocation);
        if (agent)
        {
            agent.enabled = false;
        }

        canRotate = false;
        var desiredRotation = target.transform.rotation;

        float angleDiff;
        do
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                desiredRotation,
                turnSpeed * Time.deltaTime
            );

            yield return null;
            angleDiff = Quaternion.Angle(transform.rotation, desiredRotation);
        } while (angleDiff > angleDifference);

        canTalk = canTalkWhileAtPOI;

        foreach (var item in activate)
        {
            if (item)
            {
                item.SetActive(true);
                item.BroadcastMessage("POI", this, SendMessageOptions.DontRequireReceiver);
            }
        }

        foreach (var item in deactivate)
        {
            if (item)
            {
                item.SetActive(false);
            }
        }

        DisposeOf(() =>
        {
            foreach (var item in activate)
            {
                item.BroadcastMessage("POIFinished", this, SendMessageOptions.DontRequireReceiver);
                item.SetActive(false);
            }

            foreach (var item in deactivate)
            {
                item.SetActive(true);
                item.BroadcastMessage("POIComplete", this, SendMessageOptions.DontRequireReceiver);
            }
            if (agent)
            {
                agent.enabled = true;
            }

        });

        t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            yield return null;
        }

        yield return null;
        target = null;
        Dispose();
        needs.Action();
    }

    public override string GetLabel()
    {
        return $"{targetType} (POI)";
    }

    void Start()
    {
        lastLocations.maximum = skipRecentLocations;
        if (activate != null)
        {
            foreach (var item in activate)
            {
                if (item) item.SetActive(false);
            }
        }

    }

    public bool canRotate { get; private set; }

    public bool canTalk { get; private set; }
}