using UnityEngine;

[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(DontStore))]
public class SaveInventory : MonoBehaviour
{
    public string Name = "player";

    void OnEnable()
    {
        SaveInventoryItems.LoadInventory(GetComponent<Inventory>(), Name);
    }

    void OnDisable()
    {
        SaveInventoryItems.StoreInventory(GetComponent<Inventory>(), Name);
    }
}