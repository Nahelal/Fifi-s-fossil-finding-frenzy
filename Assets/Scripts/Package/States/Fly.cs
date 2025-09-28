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

public class Fly : State
{
    public string target = "perch";
    public float satisfaction = 100;
    public float minimumRange = 5;

    bool PerchPredicate(TrackMe track)
    {
        return (track.transform.position - transform.position).sqrMagnitude > minimumRange * minimumRange;
    }

    
    protected override IEnumerator Flow()
    {
        need?.Reduce(satisfaction);
        using(var perch = TrackMe.Random(target, PerchPredicate)?.Use())
        {
            if (perch)
            {
                moveTo.Goto(perch.transform.position);
                yield return new WaitUntil(() => moveTo.AtLocation);
            }
        }

        yield return null;
        needs?.Action();
    }
}
