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
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the states of a game object, allowing for state transitions and management.
/// </summary>
[DefaultExecutionOrder(-100)]
public class StateMachine : MonoBehaviour
{
    [SerializeField] string currentState;
    readonly Dictionary<string, State> stateLookup = new();
    readonly List<State> stateList = new();
    string _state = "";
    State _current;

    void Awake()
    {
        var allStates = GetComponents<State>();
        State active = null;
        foreach (var scan in allStates)
        {
            stateLookup[scan.GetType().Name] = scan;
            stateList.Add(scan);
            if (scan.isDefault)
            {
                active = scan;
            }

            scan.enabled = false;
        }

        if (!active)
        {
            return;
        }

        _current = active;
        _state = active.GetType().Name;
        currentState = _state;
        StartCoroutine(Init(active));


    }

    IEnumerator Init(State activeState)
    {
        if (activeState)
        {
            yield return null;
            activeState.enabled = true;
        }
    }

    void Update()
    {
        currentState = _current?.GetType().Name ?? "None";
    }

    /// <summary>
    /// Disables all states in the state machine.
    /// </summary>
    protected void DisableAll()
    {
        foreach (var scan in stateList)
        {
            scan.enabled = false;
        }
    }

    /// <summary>
    /// Gets or sets the current state of the state machine.
    /// </summary>
    public string state
    {
        get => _state;
        set
        {
            if (value == _state) return;
            DisableAll();

            if (stateLookup.TryGetValue(value, out var next))
            {
                current = next;
            }
            else
            {
                _state = value;
                currentState = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets the current active state.
    /// </summary>
    public State current
    {
        get => _current;
        set
        {
            if (value && value == _current && (value?.enabled ?? false)) return;
            DisableAll();
            if (value)
            {
                _current = value;
                _state = value.GetType().Name;
                currentState = _state;
                value.enabled = true;
            }
            else
            {
                currentState = "";
                _current = null;
            }
        }
    }
}
