using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList;

    private List<KitchenObjectSO> kitchenObjectSOList;

    private void Awake()
    {
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }
    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if(validKitchenObjectSOList.Contains(kitchenObjectSO) == false)
        {
            Debug.Log("Invalid ingredient.");
            return false;
        }
        if (kitchenObjectSOList.Contains(kitchenObjectSO))
        {
            Debug.Log("Ingredient already added.");
            return false;
        }
        else
        {
            kitchenObjectSOList.Add(kitchenObjectSO);
            return true;
        }
    }
}
