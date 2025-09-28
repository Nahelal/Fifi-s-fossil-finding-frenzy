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
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Represents a collection of needs for a GameObject.
/// </summary>
[Serializable]
public class Needs : MonoBehaviour
{
    private static Need dummyNeed = new();
    /// <summary>
    /// Represents a single need with its properties and behaviors.
    /// </summary>
    [Serializable]
    public class Need
    {
        /// <summary>
        /// The name of the need.
        /// </summary>
        public string name;

        /// <summary>
        /// The current weight of the need, which influences its importance.
        /// </summary>
        [Range(0, 100)]
        [SerializeField]
        public float weight = 50f;

        /// <summary>
        /// The maximum weight the need can have.
        /// </summary>
        [Range(0, 100)]
        public float maxWeight = 100f;

        /// <summary>
        /// The minimum weight the need can have.
        /// </summary>
        [Range(0, 100)]
        public float minWeight = 0f;

        /// <summary>
        /// The amount by which the weight increases per second.
        /// </summary>
        [Range(0, 10)]
        public float increasePerSecond = 0.01f;

        /// <summary>
        /// An object that represents the target of the need, might be a state component or an item
        /// </summary>
        public UnityEngine.Object value;
        /// <summary>
        /// A string value for the need
        /// </summary>
        public string stringValue;
        /// <summary>
        /// A numeric value for the need
        /// </summary>
        public float numericValue;

        /// <summary>
        /// Boosts the weight of the need by a specified amount, clamped to min and max weights.
        /// </summary>
        /// <param name="amount">The amount to boost the weight by.</param>
        public void Increase(float amount)
        {
            weight = Mathf.Clamp(weight + amount, minWeight, maxWeight);
        }

        /// <summary>
        /// Reduces the weight of the need by a specified amount, clamped to min and max weights.
        /// </summary>
        /// <param name="amount">The amount to reduce the weight by.</param>
        public void Reduce(float amount)
        {
            weight = Mathf.Clamp(weight - amount, minWeight, maxWeight);
        }
    }

    /// <summary>
    /// All the needs being managed
    /// </summary>
    public Need[] needs;

    List<Need> _allNeeds = new();
    bool rescanNeeds = true;

    /// <summary>
    /// Gets a need by its name.
    /// </summary>
    /// <param name="need">The name of the need to retrieve.</param>
    /// <returns>The need with the specified name, or null if not found.</returns>
    public Need Get(string need)
    {
        foreach (var selection in _allNeeds)
        {
            if (selection.name == need)
            {
                return selection;
            }
        }

        return null;
    }

    /// <summary>
    /// Gets a need by its value
    /// </summary>
    /// <param name="needValue"></param>
    /// <returns></returns>
    public Need GetByValue(UnityEngine.Object needValue)
    {
        foreach (var selection in _allNeeds)
        {
            if (selection.value == needValue)
            {
                return selection;
            }
        }

        return null;
    }

    /// <summary>
    /// Gets a need by its value
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public Need GetByValue(string value)
    {
        foreach (var selection in _allNeeds)
        {
            if (selection.stringValue == value)
            {
                return selection;
            }
        }

        return null;
    }

    /// <summary>
    /// Retrieves a random need based on their weights.
    /// </summary>
    /// <returns>A randomly selected need.</returns>
    public Need GetRandom()
    {
        var max = 0f;
        foreach (var selection in allNeeds)
        {
            max += selection.weight;
        }

        var value = Random.value * max;
        foreach (var selection in allNeeds)
        {
            value -= selection.weight;
            if (value <= 0)
            {
                return selection;
            }
        }
        if (needs.Length > 0)
        {
            return needs[^1];
        }
        return dummyNeed;

    }

    void Update()
    {
        foreach (var scan in allNeeds)
        {
            scan.Increase(scan.increasePerSecond * Time.deltaTime);
        }
    }

    /// <summary>
    /// The currently selected random need.
    /// </summary>
    public Need need => GetRandom();

    public void Action()
    {
        var machine = GetComponent<StateMachine>();
        var next = need;
        if (!machine || next == null)
        {
            return;
        }

        machine.current = null;

        if (next.value is State state)
        {
            machine.current = state;
        }
        else if (next.stringValue is { Length: > 0 })
        {
            machine.state = next.stringValue;
        }
    }


    List<Need> _returnNeeds = new();

    public IEnumerable<Need> allNeeds
    {
        get
        {
            if (rescanNeeds || true)
            {
                rescanNeeds = false;
                _allNeeds.Clear();
                _allNeeds.AddRange(needs);
                foreach (var needProvider in GetComponentsInChildren<NeedProvider>(false))
                {
                    if (needProvider.gameObject.activeInHierarchy)
                    {
                        _allNeeds.Add(needProvider.need);
                    }
                }


            }

            _returnNeeds.Clear();
            _returnNeeds.AddRange(_allNeeds);
            for (var i = _allNeeds.Count - 1; i >= 0; i--)
            {
                var currentNeed = _allNeeds[i];
                if (currentNeed.value is State state)
                {
                    if (!state.CanEnter())
                    {
                        _returnNeeds.RemoveAt(i);
                    }
                }
            }

            return _returnNeeds;
        }
    }

    public IEnumerable<Need> GetUnfilteredNeeds()
    {
        if (rescanNeeds)
        {
            rescanNeeds = false;
            _allNeeds.Clear();
            _allNeeds.AddRange(needs);
            foreach (var needProvider in GetComponentsInChildren<NeedProvider>(false))
            {
                _allNeeds.Add(needProvider.need);
            }


        }

        return _allNeeds;
    }

    void NeedsChanged()
    {
        rescanNeeds = true;
    }

    void OnTransformChildrenChanged()
    {
        NeedsChanged();
    }
}
