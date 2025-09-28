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
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Represents an inventory that can hold items and manage their consumption and transfer.
/// </summary>
public class Inventory : MonoBehaviour, ISendItems
{

    public class ItemInfo
    {
        public InventoryItem item;
        public string action;
        public int count;
        public int remaining;
    }

    static ItemInfo info = new ItemInfo();

    public List<InventoryItem> items = new();
    public UnityEvent updated;
    public UnityEvent<ItemInfo> changed;

    /// <summary>
    /// Counts the number of items of a specified type in the inventory.
    /// </summary>
    /// <param name="type">The type of items to count.</param>
    /// <returns>The count of items of the specified type.</returns>
    public int Count(string type)
    {
        var c = 0;
        foreach (var item in items)
        {
            if (item.type == type)
            {
                c++;
            }
        }

        return c;
    }

    /// <summary>
    /// Checks if the inventory has a specified number of items of a given type.
    /// </summary>
    /// <param name="type">The type of items to check.</param>
    /// <param name="count">The minimum number of items required.</param>
    /// <returns>True if the inventory has at least the specified count of items; otherwise, false.</returns>
    public bool Has(string type, int count = 1)
    {
        return Count(type) >= count;
    }

    /// <summary>
    /// Consumes a specified number of items of a given type from the inventory.
    /// </summary>
    /// <param name="type">The type of items to consume.</param>
    /// <param name="count">The number of items to consume.</param>
    /// <returns>The number of items actually consumed.</returns>
    public int Consume(string type, int count = 1)
    {
        var consumed = 0;
        for (var i = items.Count - 1; i >= 0 && count > 0; i--)
        {
            var item = items[i];
            if (item.type != type)
            {
                continue;
            }


            consumed++;
            count--;
            info.item = item;
            info.remaining = count;
            info.count = 1;
            info.action = "consume";
            changed?.Invoke(info);

            items.RemoveAt(i);
        }
        Changed();
        return consumed;
    }

    public void Changed()
    {
        updated?.Invoke();
        BroadcastMessage("Check", SendMessageOptions.DontRequireReceiver);
    }

    /// <summary>
    /// Adds an item to the inventory.
    /// </summary>
    /// <param name="item">The item to add to the inventory.</param>
    public void Add(InventoryItem item)
    {
        if (item != null)
        {
            item.owner = gameObject;
            items.Add(item);
            info.item = item;
            info.remaining = Count(item.type);
            info.count = 1;
            info.action = "add";
            changed?.Invoke(info);

            Changed();
        }
    }

    /// <summary>
    /// Sends a specified number of items of a given type to another inventory.
    /// </summary>
    /// <param name="target">The target inventory to send items to.</param>
    /// <param name="type">The type of items to send.</param>
    /// <param name="count">The number of items to send.</param>
    /// <returns>The number of items actually sent.</returns>
    public int SendToInventory(Inventory target, string type, int count = 1)
    {
        var consumed = 0;
        for (var i = items.Count - 1; i >= 0 && count > 0; i--)
        {
            var item = items[i];
            if (item.type != type)
            {
                continue;
            }

            consumed++;
            target.Add(item);
            count--;
            info.item = item;
            info.remaining = count;
            info.count = 1;
            info.action = "transfer";
            changed?.Invoke(info);

            items.RemoveAt(i);
        }
        Changed();
        target.Changed();
        return consumed;
    }

}
