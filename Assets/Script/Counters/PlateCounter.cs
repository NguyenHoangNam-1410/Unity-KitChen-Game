using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlateCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;

    [SerializeField] private float spawnPlateTimerMax = 4f;
    private float spawnPlateTimer;
    private int plateSpawnedAmount;
    private int plateSpawnedMaxAmount = 4;


    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;
    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;
        if (spawnPlateTimer > spawnPlateTimerMax)
        {
            spawnPlateTimer = 0f;
            
            if(plateSpawnedAmount < plateSpawnedMaxAmount)
            {
                plateSpawnedAmount++;
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }

        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {

            // Player is not carrying anything
            if (plateSpawnedAmount > 0)
            {
                // Least one plate is available
                plateSpawnedAmount--;
                KitchenObject.SpawnKitChenObject(plateKitchenObjectSO, player);

                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
