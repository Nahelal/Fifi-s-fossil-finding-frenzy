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
/// Extends AudioPlayer to play a specified AudioClip with a fade-in effect upon activation.
/// Ensures execution order precedes standard scripts.
/// </summary>
[DefaultExecutionOrder(-1)]
public class PlayAudioOnActive : AudioPlayer
{
    /// <summary>
    /// The audio clip to play when the component becomes active.
    /// </summary>
    public AudioClip audioToPlay;
    
    /// <summary>
    /// Determines the rate at which the audio fades in (volume increase per second).
    /// </summary>
    public float speed = 1;
    
    /// <summary>
    /// Indicates whether the audio should loop continuously after playback.
    /// </summary>
    public bool loop;
    
    /// <summary>
    /// Coroutine that handles playing the audio clip with a gradual fade-in effect.
    /// </summary>
    /// <param name="former">The previously running Coroutine, if any.</param>
    /// <returns>An IEnumerator for coroutine execution.</returns>
    protected override IEnumerator Activate(Coroutine former)
    {
        if (source.clip != audioToPlay || source.volume < 1 || !source.isPlaying)
        {
            yield return base.Activate(former);
            source.volume = 0;
            if (audioToPlay)
            {
                source.clip = audioToPlay;
                source.loop = loop;
                source.Play();
                while (source.volume < 1)
                {
                    source.volume += Time.deltaTime * speed;
                    yield return null;
                }
            }
        }
    }
}