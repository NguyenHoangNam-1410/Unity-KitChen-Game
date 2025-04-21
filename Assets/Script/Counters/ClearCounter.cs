using UnityEngine;

public class ClearCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }

            else
            {
                Debug.Log("Player does not have a kitchen object.");
            }

        }
        else
        {
            // Player has a kitchen object
            if (!player.HasKitchenObject())
            {
                // Player does not have a kitchen object
                GetKitchenObject().SetKitchenObjectParent(player);
            }
            else
            {

                // Player has a kitchen object
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // Player has a plate
                    if(plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                    
                }
                else
                {
                    // Player has a kitchen object but it's not a plate
                    if(GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        // Counter has a plate
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            }
        }
    }
}
