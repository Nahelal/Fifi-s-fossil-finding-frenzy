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
using UnityEngine;

public class InventoryWatcher : MonoBehaviour
{
    public Inventory inventory;
    public string addMessage = "Collected";
    public string consumeMessage = "Used";
    public string transferMessage = "Gave";

    private void OnEnable()
    {
        inventory ??= GetComponentInParent<Inventory>();
        inventory.changed.AddListener(Notify);
    }

    private void OnDisable()
    {
        inventory.changed.RemoveListener(Notify);
    }

    void Notify(Inventory.ItemInfo info)
    {
        if (info.item.hidden) return;
        var description = info.item.description != null && info.item.description.Length > 0 ? $"{info.item.description} ({info.item.type})" : info.item.type;
        switch (info.action)
        {
            case "add":
                NotificationUI.ShowMessage($"{addMessage} {description}");
                break;
            case "transfer":
                NotificationUI.ShowMessage($"{transferMessage} {description}");
                break;
            case "consumed":
                NotificationUI.ShowMessage($"{consumeMessage} {description}");
                break;
        }
    }

}