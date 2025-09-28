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

/// <summary>
/// Manages audio playback by initializing and activating an AudioSource.
/// Inherits from Enabler to control activation.
/// </summary>
public class AudioPlayer : Enabler
{
    /// <summary>
    /// Reference to the AudioSource component used for audio playback.
    /// </summary>
    public AudioSource source;

    protected static Coroutine Current;

    /// <summary>
    /// Initializes the AudioSource by attempting to get it from the current GameObject,
    /// falling back to the main camera if not present.
    /// </summary>
    protected virtual void Awake()
    {
        source ??= GetComponent<AudioSource>() ?? Camera.main?.GetComponent<AudioSource>();
    }

    /// <summary>
    /// Starts the audio activation coroutine, ensuring proper handling of concurrent activations.
    /// </summary>
    protected override void Enable()
    {
        Current = StartCoroutine(Activate(Current));
    }

    /// <summary>
    /// Coroutine that handles the activation process for the audio playback.
    /// </summary>
    /// <param name="former">The previously running Coroutine, if any.</param>
    /// <returns>An IEnumerator for coroutine execution.</returns>
    protected virtual IEnumerator Activate(Coroutine former)
    {
        yield return former;
    }

}