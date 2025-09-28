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

/// <summary>
/// This class represents an attractor that can attract objects within a specified range.
/// </summary>
public class Attractor : MonoBehaviour
{
    /// <summary>
    /// The name of the objects that this attractor can attract.
    /// </summary>
    public string attracts;

    /// <summary>
    /// The maximum range within which the attractor can affect other objects.
    /// </summary>
    public float maxRange = 50f;

    /// <summary>
    /// The curve defining the strength of the attractor's effect over distance.
    /// </summary>
    public AnimationCurve attractEffect = AnimationCurve.Constant(0,1,1);

    /// <summary>
    /// The strength of the attractor's effect.
    /// </summary>
    public float strength = 1f;

    /// <summary>
    /// The weight of the attractor, used for calculations.
    /// </summary>
    [HideInInspector]
    public float weight;
}
