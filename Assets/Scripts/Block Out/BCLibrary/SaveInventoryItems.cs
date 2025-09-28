using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SaveInventoryItems
{
    static Dictionary<string, string> _knownInventories = new();

    public static void StoreInventory(Inventory inventory, string name)
    {
        _knownInventories[name] = JsonUtility.ToJson(new StoredInventory { items = inventory.items });
    }

    public static void LoadInventory(Inventory inventory, string name)
    {
        if (_knownInventories.TryGetValue(name, out var stored))
        {
            var saved = JsonUtility.FromJson<StoredInventory>(stored);
            inventory.items = saved.items;

            foreach (var item in inventory.items)
                if (item != null) item.owner = inventory.gameObject;
            inventory.Changed();
        }
    }

    [Serializable]
    public class StoredInventory
    {
        public string id;
        public List<InventoryItem> items;
    }

    [Serializable]
    public class StoredInventoryItem
    {
        public string id;
        public InventoryItem item;
    }

    [Serializable]
    public class Storage
    {
        public List<StoredInventory> inventories = new();
        public List<StoredInventoryItem> addInventoryItems = new();
        public List<StoredInventoryItem> inventoryHolders = new();
    }

    public static string StoreInventories() =>
      StoreInventories(SceneManager.GetActiveScene());

    public static string StoreInventories(Scene scene)
    {
        var output = new Storage();
        var rootObjects = scene.GetRootGameObjects();

        foreach (var root in rootObjects)
        {
            foreach (var inventory in root.GetComponentsInChildren<Inventory>(true))
            {
                if (inventory.GetComponent<DontStore>()) continue;
                output.inventories.Add(new StoredInventory
                {
                    id = SaveAndLoad.GetId(inventory),
                    items = inventory.items
                });
            }

            foreach (var add in root.GetComponentsInChildren<AddInventoryItem>(true))
            {
                if (add.GetComponent<DontStore>()) continue;
                output.addInventoryItems.Add(new StoredInventoryItem
                {
                    id = SaveAndLoad.GetId(add),
                    item = add.item
                });
            }

            foreach (var holder in root.GetComponentsInChildren<InventoryItemHolder>(true))
            {
                if (holder.GetComponent<DontStore>()) continue;
                output.inventoryHolders.Add(new StoredInventoryItem
                {
                    id = SaveAndLoad.GetId(holder),
                    item = holder.item
                });
            }
        }

        return JsonUtility.ToJson(output);
    }


    public static void RestoreInventories(string json)
    {
        var data = JsonUtility.FromJson<Storage>(json);

        foreach (var stored in data.inventories)
        {
            var inv = SaveAndLoad.FindById<Inventory>(stored.id);
            if (!inv) continue;

            inv.items = stored.items;
            foreach (var item in inv.items)
                if (item != null) item.owner = inv.gameObject;
            inv.Changed();
        }

        foreach (var stored in data.addInventoryItems)
        {
            var add = SaveAndLoad.FindById<AddInventoryItem>(stored.id);
            if (!add) continue;

            add.item = stored.item;
            if (add.item != null) add.item.owner = add.gameObject;
        }

        foreach (var stored in data.inventoryHolders)
        {
            var holder = SaveAndLoad.FindById<InventoryItemHolder>(stored.id);
            if (!holder) continue;

            holder.item = stored.item;
            if (holder.item != null) holder.item.owner = holder.gameObject;
        }
    }
}
