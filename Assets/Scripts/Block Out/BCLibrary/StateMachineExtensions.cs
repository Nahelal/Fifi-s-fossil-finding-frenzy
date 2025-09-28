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


using NUnit.Framework.Constraints;
using UnityEngine;

public static class StateMachineExtensions
{
    public static StateMachine GetMachine(this Component component)
    {
        return component.GetComponent<StateMachine>();
    }

    public static StateMachine GetMachine(this GameObject go)
    {
        return go.GetComponent<StateMachine>();
    }

    public static State GetState(this Component component)
    {
        return GetMachine(component)?.current;
    }

    public static State GetState(this GameObject gameObject)
    {
        return GetMachine(gameObject)?.current;
    }

    public static string GetStateName(this Component component)
    {
        return GetMachine(component)?.state;
    }

    public static string GetStateName(this GameObject gameObject)
    {
        return GetMachine(gameObject)?.state;
    }

    public static string SetState(this Component component, string state)
    {
        
        var machine = GetMachine(component);
        if (!machine)
        {
            return "";
        }

        var current = machine.state;
        machine.state = state;
        return current;

    }

    public static string SetState(this GameObject gameObject, string state)
    {
        var machine = GetMachine(gameObject);
        if (!machine)
        {
            return "";
        }

        var current = machine.state;
        machine.state = state;
        return current;

    }

    public static State SetState(this Component component, State state)
    {
        var machine = GetMachine(component);
        if (!machine)
        {
            return null;
        }

        var current = machine.current;

        machine.current = state;
        return current;

    }

    public static State SetState(this GameObject gameObject, State state)
    {
        var machine = GetMachine(gameObject);
        if (!machine)
        {
            return null;
        }

        var current = machine.current;
        machine.current = state;
        return current;

    }

    static readonly State[] Dummy = new State []{};

    public static State[] GetStates(this Component component)
    {
        var machine = component.GetMachine();
        return machine ? machine.GetComponents<State>() : Dummy;
    }

    public static State[] GetStates(this GameObject gameObject)
    {
        var machine = gameObject.GetMachine();
        return machine ? machine.GetComponents<State>() : Dummy;
    }
}