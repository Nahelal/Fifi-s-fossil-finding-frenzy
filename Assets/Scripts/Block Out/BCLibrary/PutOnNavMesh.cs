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
using UnityEngine.AI;

/// <summary>
/// Positions the GameObject onto the nearest valid NavMesh surface at the start.
/// </summary>
public class PutOnNavMesh : MonoBehaviour
{
    /// <summary>
    /// Maximum distance to search for a valid position on the NavMesh.
    /// </summary>
    public float maxDistance = 10f;

    /// <summary>
    /// Called on the frame when the script is enabled, initiating the placement of the object on the NavMesh.
    /// </summary>
    void Start()
    {
        PlaceObjectOnNavMesh();
    }

    /// <summary>
    /// Moves the object to the nearest point on the NavMesh within the specified maximum distance.
    /// </summary>
    public void PlaceObjectOnNavMesh()
    {
        // Search for a valid position on the NavMesh near the current position.
        if (NavMesh.SamplePosition(transform.position, out var hit, maxDistance, NavMesh.AllAreas))
        {
            transform.position = hit.position;
        }
        else
        {
            Debug.LogWarning("No NavMesh found near the object.");
        }
    }
}