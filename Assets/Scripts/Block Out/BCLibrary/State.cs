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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder.MeshOperations;

[DefaultExecutionOrder(300)]
public class State : MonoBehaviour, IDisposable
{

    public bool isDefault;
    protected StateMachine machine;
    protected Needs needs;
    protected MoveTo moveTo;
    protected NavMeshAgent agent;

    Coroutine flow;
    [HideInInspector] public bool preserve;

    public virtual string GetLabel()
    {
        return $"{GetType().Name}";
    }

    readonly List<IDisposable> toDispose = new();
    readonly List<Action> onDispose = new();

    protected T DisposeOf<T>(T disposable) where T:  IDisposable
    {
        if (disposable != null)
        {
            toDispose.Add(disposable);
        }

        return disposable;
    }

    protected void DisposeOf(Action disposer)
    {
        onDispose.Add(disposer);
    }

    protected Needs.Need need
    {
        get
        {
            if (needs)
            {
                return needs.GetByValue(this);
            }

            return null;
        }
    }

    public virtual bool CanEnter()
    {
        return true;
    }

    protected virtual void OnEnable()
    {
        if (preserve) return;
        agent = agent ? agent : GetComponent<NavMeshAgent>();
        machine ??= GetComponent<StateMachine>();
        needs ??= GetComponent<Needs>();
        moveTo ??= GetComponent<MoveTo>();
        Enter();
        flow = StartCoroutine(InternalFlow());

    }

    protected virtual void Enter()
    {

    }

    protected virtual void Exit()
    {

    }

    protected virtual void OnDisable()
    {
        if (!preserve)
        {
            if (flow != null)
            {
                StopCoroutine(flow);
                flow = null;
            }

            Exit();
            Dispose();
        }

    }

    IEnumerator InternalFlow()
    {
        var inner = Flow();
        var looping = true;
        while (looping)
        {
            if (!enabled)
            {
                yield return null;
            }
            else
            {
                looping = inner.MoveNext();
                if (looping)
                {
                    yield return inner.Current;
                }
            }
        }
    }

    public Action Pause()
    {
        var agent = GetComponent<NavMeshAgent>();
        var isEnabled = agent?.isStopped ?? false;
        var acceleration = agent?.acceleration ?? 3;
        preserve = true;
        enabled = false;
        if (agent)
        {
            agent.acceleration = Single.MaxValue;
            agent.isStopped = true;
            agent.updateRotation = false;
            agent.updatePosition = false;
        }                
        return () =>
        {

            if (agent)
            {
                agent.acceleration = acceleration;
                agent.isStopped = isEnabled;
                agent.updateRotation = true;
                agent.updatePosition = true;
                
            }

            if (machine.current == this)
            {
                preserve = true;
                enabled = true;
            }

            preserve = false;
        };
    }

    protected virtual IEnumerator Flow()
    {
        yield break;
    }

    protected virtual void CleanUp()
    {

    }

    public void Dispose()
    {
        foreach (var dispose in toDispose)
        {
            dispose.Dispose();
        }

        foreach (var disposer in onDispose)
        {
            disposer();
        }
        toDispose.Clear();
        onDispose.Clear();
        CleanUp();
    }
}
