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
using UnityEngine.Serialization;


/// <summary>
/// Enables children based on inventory items
/// </summary>
[DefaultExecutionOrder(-400)]
public class HasInventoryItem : MonoBehaviour, IActivator
{
    [Serializable]
    public class Predicate
    {
        [Tooltip("The inventory to check, leave blank for the default inventory")]
        public Inventory inventory;
        [Tooltip("Whether the rule should not be fulfilled to activate children")]
        [FormerlySerializedAs("not")]
        public bool dontHave = false;
        [Tooltip("The type of the inventory item that should be counted")]
        public string item;
        [Tooltip("The MINIMUM number of items of the type that should exist")]
        public int count;

    }
    [Tooltip("The default inventory to use, or if blank then the inventory above this rule in the hierarchy")]
    public Inventory inventory;
    [Space(20)]
    [Header("Condition")]
    [FormerlySerializedAs("not")]
    [Tooltip("Whether the rule should not be fulfilled to activate children")]
    public bool dontHave = true;
    [Tooltip("The type of the inventory item that should be counted")]
    public string item;
    [Tooltip("The MINIMUM number of items of the type that should exist")]
    public int count = 1;
    [Space(10)]
    [Tooltip("Additional rules")]
    public Predicate[] and;

    void Awake()
    {
        inventory ??= GetComponentInParent<Inventory>();
        transform.ActivateChildren(false);
    }

    void OnEnable()
    {

        inventory.updated.AddListener(Check);
        foreach (var predicate in and)
        {
            if (predicate.inventory)
            {
                predicate.inventory.updated.AddListener(Check);
            }
        }
        Check();
    }

    void OnDisable()
    {
        inventory.updated.RemoveListener(Check);
        foreach (var predicate in and)
        {
            if (predicate.inventory)
            {
                predicate.inventory.updated.RemoveListener(Check);
            }
        }
    }


    void Check()
    {
        if (!enabled || !gameObject.activeInHierarchy || !gameObject.activeSelf) return;
        if (!inventory) return;
        var isActive = inventory.Has(item, count);
        if (dontHave) isActive = !isActive;
        foreach (var predicate in and)
        {
            var nextActive = (predicate.inventory ?? inventory).Has(predicate.item, predicate.count);
            if (predicate.dontHave)
            {
                nextActive = !nextActive;
            }

            isActive = isActive && nextActive;
        }
        transform.ActivateChildren(isActive);
    }

    public void Activate(bool isActive)
    {
        if (!isActive)
        {
            transform.ActivateChildren(false);
        }
    }
}
