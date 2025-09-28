using UnityEngine;

public class SetPlayerInventory : MonoBehaviour
{
    public bool to = true;
    public bool from = false;

    void Start()
    {
        var player = FindAnyObjectByType<PlayerMarker>();
        if (!player) return;
        var inventory = player.GetComponent<Inventory>();
        if (!inventory) return;
        var adds = GetComponents<AddInventoryItem>();
        foreach (var add in adds)
        {
            if (to)
            {
                add.inventory = inventory;
            }
        }

        var transfers = GetComponents<TransferInventoryItem>();
        foreach (var transfer in transfers)
        {
            if (to)
            {
                transfer.toInventory = inventory;
            }
            if (from)
            {
                transfer.fromInventory = inventory;
            }
        }
    }
}