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

public class PlayAnimationOnEnable : MonoBehaviour
{
    public string clipName;
    public Animator animator;
    public Animate animate;
    string last;

    void Awake()
    {
        animator = animator ? animator : GetComponentInParent<Animator>();
    }

    void OnEnable()
    {
        if (animate)
        {
            animate.force = clipName;
        }
        else
        {
            last = animator.GetState().name;
            animator.CrossFade(clipName, 0.2f, 0);
        }
    }

    void OnDisable()
    {
        if (animate)
        {
            animate.force = null;
        }
        else
        {
            animator.CrossFade(last, 0.2f);
        }
    }
}
