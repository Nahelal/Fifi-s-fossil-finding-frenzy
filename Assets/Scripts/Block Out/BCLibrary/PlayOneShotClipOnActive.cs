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
/// Extends AudioPlayer to play a single audio clip once when activated.
/// Ensures execution order precedes standard scripts.
/// </summary>
[DefaultExecutionOrder(-1)]
public class PlayOneShotClipOnActive : AudioPlayer
{
    /// <summary>
    /// The audio clip to be played as a one-shot when the component becomes active.
    /// </summary>
    public AudioClip audioToPlay;

    /// <summary>
    /// The volume at which the audio clip will be played.
    /// </summary>
    public float volume = 1;
    
    /// <summary>
    /// Plays the audio clip once at the specified volume immediately upon activation.
    /// </summary>
    /// <param name="former">The previously running Coroutine, if any.</param>
    /// <returns>An IEnumerator for coroutine execution.</returns>
    protected override IEnumerator Activate(Coroutine former)
    {
        source.PlayOneShot(audioToPlay, volume);
        yield break;
    }
}