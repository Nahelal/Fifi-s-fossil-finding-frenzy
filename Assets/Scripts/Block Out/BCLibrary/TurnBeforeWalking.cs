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


using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Rotates the GameObject to face its NavMeshAgent steering target before moving towards it.
/// Ensures smoother and more realistic navigation.
/// </summary>
public class TurnBeforeWalking : MonoBehaviour
{
    /// <summary>
    /// Rotation speed in degrees per second.
    /// </summary>
    public float turnSpeed = 120f; // Degrees per second
    
    /// <summary>
    /// Angle threshold in degrees below which rotation is considered complete.
    /// </summary>
    public float angleThreshold = 5f; // Angle threshold to consider turning complete
    
    /// <summary>
    /// Divides the agent's speed by this value during turning.
    /// </summary>
    public float speedDivisor = 5;

    private NavMeshAgent agent;
    Walk walk;
    float normalSpeed;
    float normalAcceleration;

    /// <summary>
    /// Initializes references and stores the agent's original speed and acceleration.
    /// </summary>
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        normalSpeed = agent.speed;
        normalAcceleration = agent.acceleration;
    }

    /// <summary>
    /// Called every frame to manage rotation and movement based on the agent's steering target.
    /// </summary>
    void Update()
    {
        if (!agent.enabled || !agent.isOnNavMesh || agent.isStopped) return;
        var direction = (agent.steeringTarget - transform.position);
        direction.y = 0f; // Keep the rotation strictly horizontal
        if (direction.sqrMagnitude == 0)
        {
            direction = transform.forward;
        }

        var desiredRotation = Quaternion.LookRotation(direction);

        var angleDiff = Quaternion.Angle(transform.rotation, desiredRotation);

        // If the angle is larger than the threshold, rotate manually
        if (angleDiff > angleThreshold && agent.remainingDistance > 0.1f)
        {
            agent.acceleration = normalAcceleration * 5;
            agent.updateRotation = false;
            // Smoothly rotate toward the target
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                desiredRotation,
                turnSpeed * Time.deltaTime
            );

            // Stop the agent from moving while turning
            agent.speed = normalSpeed / speedDivisor;
        }
        else
        {
            // Rotation is close enough: allow the agent to move
            agent.acceleration = normalAcceleration;
            agent.updateRotation = true;
            agent.speed = normalSpeed;
        }
    }
}