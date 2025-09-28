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
/// Extends AudioPlayer to fade out audio gradually when activated.
/// Ensures execution order precedes most other scripts.
/// </summary>
[DefaultExecutionOrder(-2)]
public class FadeAudioOnActive : AudioPlayer
{
    /// <summary>
    /// If set to a value, ensures that fading only happens for the specified clip
    /// </summary>
    public AudioClip clip;
    /// <summary>
    /// Determines the rate at which the audio fades out (volume reduction per second).
    /// </summary>
    public float speed = 1;

    /// <summary>
    /// Coroutine that handles audio fade-out upon activation, reducing volume until silent, then stops playback.
    /// </summary>
    /// <param name="former">The previously running Coroutine, if any.</param>
    /// <returns>An IEnumerator for coroutine execution.</returns>
    protected override IEnumerator Activate(Coroutine former)
    {
        if (source.clip && (source.clip == clip || !clip) && source.volume > 0)
        {
            yield return base.Activate(former);
            while (source.volume > 0)
            {
                source.volume -= Time.deltaTime * speed;
                yield return null;
            }

            source.Stop();
        }
    }
}