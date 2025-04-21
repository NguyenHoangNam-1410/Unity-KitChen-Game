using System;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgessChangedEventArg> OnProgessChanged;
    public event EventHandler OnCut;

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    private int cuttingProgess;
    public override void Interact(Player player)
    {
        if (player == null)
        {
            Debug.LogError("Player is null in Interact method.");
            return;
        }

        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                KitchenObject playerKitchenObject = player.GetKitchenObject();
                if (playerKitchenObject == null)
                {
                    Debug.LogError("Player's KitchenObject is null.");
                    return;
                }

                KitchenObjectSO playerKitchenObjectSO = playerKitchenObject.GetKitchenObjectSO();
                if (playerKitchenObjectSO == null)
                {
                    Debug.LogError("Player's KitchenObjectSO is null.");
                    return;
                }

                if (HasRecipeWithInput(playerKitchenObjectSO))
                {
                    playerKitchenObject.SetKitchenObjectParent(this);
                    cuttingProgess = 0; // Reset cutting progress

                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(playerKitchenObjectSO);
                    if (cuttingRecipeSO == null)
                    {
                        Debug.LogError("CuttingRecipeSO is null for the given input.");
                        return;
                    }

                    OnProgessChanged?.Invoke(this, new IHasProgress.OnProgessChangedEventArg
                    {
                        progressNormalize = (float)cuttingProgess / cuttingRecipeSO.cuttingProgressMax
                    });
                }
            }
        }
        else
        {
            if (!player.HasKitchenObject())
            {
                KitchenObject counterKitchenObject = GetKitchenObject();
                if (counterKitchenObject == null)
                {
                    Debug.LogError("Counter's KitchenObject is null.");
                    return;
                }

                counterKitchenObject.SetKitchenObjectParent(player);
            }
            else
            {
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // Player has a plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }

                }
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (player == null)
        {
            Debug.LogError("Player is null in InteractAlternate method.");
            return;
        }

        if (HasKitchenObject())
        {
            KitchenObject counterKitchenObject = GetKitchenObject();
            if (counterKitchenObject == null)
            {
                Debug.LogError("Counter's KitchenObject is null.");
                return;
            }

            KitchenObjectSO counterKitchenObjectSO = counterKitchenObject.GetKitchenObjectSO();
            if (counterKitchenObjectSO == null)
            {
                Debug.LogError("Counter's KitchenObjectSO is null.");
                return;
            }

            if (HasRecipeWithInput(counterKitchenObjectSO))
            {
                cuttingProgess++; // Increment cutting progress

                OnCut?.Invoke(this, EventArgs.Empty);

                CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(counterKitchenObjectSO);
                if (cuttingRecipeSO == null)
                {
                    Debug.LogError("CuttingRecipeSO is null for the given input.");
                    return;
                }

                OnProgessChanged?.Invoke(this, new IHasProgress.OnProgessChangedEventArg
                {
                    progressNormalize = (float)cuttingProgess / cuttingRecipeSO.cuttingProgressMax
                });

                if (cuttingProgess >= cuttingRecipeSO.cuttingProgressMax)
                {
                    KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(counterKitchenObjectSO);
                    if (outputKitchenObjectSO == null)
                    {
                        Debug.LogError("Output KitchenObjectSO is null.");
                        return;
                    }

                    counterKitchenObject.DestroySelf();
                    KitchenObject.SpawnKitChenObject(outputKitchenObjectSO, this);
                }
                else
                {
                    // Play cut animation or sound
                    Debug.Log("Cutting progress: " + cuttingProgess);
                }
            }
        }
    }


    private bool HasRecipeWithInput(KitchenObjectSO input)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(input);
        return cuttingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO input)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(input);
        if (cuttingRecipeSO)
        {
            return cuttingRecipeSO.output;
        }
        Debug.LogError("Cutting recipe not found for input: " + input);
        return null;
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO input)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == input)
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}
