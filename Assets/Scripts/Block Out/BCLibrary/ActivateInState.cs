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
using System.Collections;
using UnityEngine;

[DefaultExecutionOrder(10000)]
[RequireComponent(typeof(StateMachine))]
public class ActivateInState : MonoBehaviour, IActivator
{
    /// <summary>
    /// Name of the state that triggers activation or deactivation.
    /// </summary>
    [Space(5)]
    public string state;
    /// <summary>
    /// GameObjects to activate when entering the specified state.
    /// </summary>
    [Space(20)]
    [Header("Activation")]
    public GameObject[] activate;
    /// <summary>
    /// Components (Behaviours) to enable when entering the specified state.
    /// </summary>
    public Behaviour[] activateComponents;
    
    /// <summary>
    /// GameObjects to deactivate when entering the specified state.
    /// </summary>
    [Space(20)]
    [Header("Deactivation")]
    public GameObject[] deactivate;
    /// <summary>
    /// Components (Behaviours) to disable when entering the specified state.
    /// </summary>
    public Behaviour[] deactivateComponents;
    string lastState;
    StateMachine machine;

    void Awake()
    {
        machine = GetComponent<StateMachine>();
    }

    /// <summary>
    /// Coroutine that manages activation and deactivation based on state changes.
    /// </summary>
    IEnumerator Switch()
    {
        if (machine.state != state)
        {
            Activate(false);
        }
        yield return new WaitForEndOfFrame();
        if (machine.state == state)
        {
            Activate(true);
        }
    }

    /// <summary>
    /// Checks for state changes every frame and initiates the activation coroutine.
    /// </summary>
    void Update()
    {
        if (machine.state != lastState)
        {
            lastState = machine.state;
            StartCoroutine(Switch());
        }
    }

    /// <summary>
    /// Activates or deactivates specified GameObjects and components depending on the current state.
    /// </summary>
    /// <param name="isActive">Whether to activate (true) or deactivate (false) the items.</param>
    public void Activate(bool isActive)
    {
        foreach (var go in activate)
        {
            go.SetActive(isActive);
        }
        foreach (var go in deactivate)
        {
            go.SetActive(!isActive);
        }
        foreach (var behaviour in activateComponents)
        {
            behaviour.enabled = isActive;
        }
        foreach (var behaviour in deactivateComponents)
        {
            behaviour.enabled = !isActive;
        }
    }
}
