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


using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

/// <summary>
/// Provides extension methods for convenient manipulation and querying of Transform objects.
/// </summary>
public static class TransformExtensions
{
    /// <summary>
    /// Activates or deactivates all immediate child GameObjects of a Transform.
    /// </summary>
    /// <param name="transform">The Transform whose children will be activated or deactivated.</param>
    /// <param name="isActive">True to activate, false to deactivate.</param>
    public static void ActivateChildren(this Transform transform, bool isActive)
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<Ignore>()) continue;
            child.gameObject.SetActive(isActive);
        }
    }

    static readonly List<Transform> Results = new();

    /// <summary>
    /// Retrieves all immediate child Transforms that are currently active in the hierarchy.
    /// </summary>
    /// <param name="transform">The Transform whose active children are returned.</param>
    /// <returns>An IEnumerable of active child Transforms.</returns>
    public static IEnumerable<Transform> ActiveChildren(this Transform transform)
    {
        Results.Clear();
        foreach (Transform child in transform)
        {
            if (child.GetComponent<Ignore>()) continue;
            if (child.gameObject.activeInHierarchy)
            {
                Results.Add(child);
            }
        }

        return Results;
    }

    /// <summary>
    /// Retrieves components of a specified type from immediate active child GameObjects, optionally searching deeper if none are found initially.
    /// </summary>
    /// <param name="component">The component whose child GameObjects will be searched.</param>
    /// <param name="noRedirect">If false, searches recursively in grandchildren if no components are found initially.</param>
    /// <typeparam name="T">The type of component to retrieve.</typeparam>
    /// <returns>A List of components found.</returns>
    public static List<T> GetComponentsInDirectChildren<T>(this Component component, bool noRedirect = false) where T : Component
    {
        var list = new List<T>();

        foreach (Transform child in component.transform)
        {
            if (child.GetComponent<Ignore>()) continue;
            if (child.gameObject.activeSelf)
            {
                var target = child.GetComponent<T>();
                if (target)
                {
                    list.Add(target);
                }
            }
        }

        if (list.Count == 0 && !noRedirect)
        {
            foreach (Transform child in component.transform)
            {
                if (child.gameObject.activeSelf)
                {
                    list = child.GetComponentsInDirectChildren<T>();
                    if (list.Count > 0)
                    {
                        return list;
                    }
                }
            }
        }

        return list;
    }
}
