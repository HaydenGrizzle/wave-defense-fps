using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootItem
{
    public GameObject item;   // prefab to drop
    public float weight;      // drop chance weight
}

public class BoxDrop : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public float price = 45;
    public LootItem[] lootPool;   // include the “NothingObject” prefab here
    public Transform spawnLocation;

    public float launchForce = 5f;
    public float upwardForce = 2f;
    public float spreadRadius = 0.5f;

    public void DropItem()
    {
        playerHealth.Pay(price);

        if (lootPool.Length == 0) return;

        GameObject itemToDrop = GetRandomLoot();

        Vector3 randomOffset = new Vector3(
            Random.Range(-spreadRadius, spreadRadius),
            0,
            Random.Range(-spreadRadius, spreadRadius)
        );

        // Instantiate the selected item (including the "NothingObject" if chosen)
        GameObject dropped = Instantiate(itemToDrop, spawnLocation.position + randomOffset, Random.rotation);

        Rigidbody rb = dropped.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Base direction: mostly forward
            Vector3 launchDir = spawnLocation.forward + Vector3.up * upwardForce;

            // Clamp the vertical component so it never goes fully straight up
            float maxY = 0.7f; // 0 = horizontal, 1 = straight up
            launchDir.y = Mathf.Min(launchDir.y, maxY);

            // Normalize and apply force
            launchDir.Normalize();
            rb.AddForce(launchDir * launchForce, ForceMode.Impulse);
        }
    }

    private GameObject GetRandomLoot()
    {
        float totalWeight = 0f;
        foreach (LootItem loot in lootPool)
            totalWeight += loot.weight;

        float randomValue = Random.Range(0f, totalWeight);
        float cumulative = 0f;

        foreach (LootItem loot in lootPool)
        {
            cumulative += loot.weight;
            if (randomValue <= cumulative)
                return loot.item;
        }

        // fallback (shouldn’t happen)
        return lootPool[lootPool.Length - 1].item;
    }
}