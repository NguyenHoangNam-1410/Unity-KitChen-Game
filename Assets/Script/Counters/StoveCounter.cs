using System;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    public event EventHandler<IHasProgress.OnProgessChangedEventArg> OnProgessChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }

    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned,
    }

    private State state;

    private float fryingTimer;

    private float burningTimer;

    private FryingRecipeSO fryingRecipeSO;

    private BurningRecipeSO burningRecipeSO;

    private void Start()
    {
        state = State.Idle;
    }

    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (state)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;

                    OnProgessChanged?.Invoke(this, new IHasProgress.OnProgessChangedEventArg
                    {
                        progressNormalize = fryingTimer / fryingRecipeSO.fryingTimerMax
                    });

                    if (fryingTimer > fryingRecipeSO.fryingTimerMax)
                    {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitChenObject(fryingRecipeSO.output, this);
                        state = State.Fried;
                        burningTimer = 0f;
                        burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });
                    }
                    break;
                case State.Fried:
                    burningTimer += Time.deltaTime;

                    OnProgessChanged?.Invoke(this, new IHasProgress.OnProgessChangedEventArg
                    {
                        progressNormalize = burningTimer / burningRecipeSO.burningTimerMax
                    });

                    if (burningTimer > burningRecipeSO.burningTimerMax)
                    {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitChenObject(burningRecipeSO.output, this);
                        state = State.Burned;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });

                        OnProgessChanged?.Invoke(this, new IHasProgress.OnProgessChangedEventArg
                        {
                            progressNormalize = 0f
                        });
                    }
                    break;
                case State.Burned:
                    break;
            }
        }
    }
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

                    fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    state = State.Frying;
                    fryingTimer = 0f;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = state
                    });

                    OnProgessChanged?.Invoke(this, new IHasProgress.OnProgessChangedEventArg
                    {
                        progressNormalize = fryingTimer / fryingRecipeSO.fryingTimerMax
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

                state = State.Idle;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    state = state
                });

                OnProgessChanged?.Invoke(this, new IHasProgress.OnProgessChangedEventArg
                {
                    progressNormalize = 0f
                });
            }
        }
    }
    private bool HasRecipeWithInput(KitchenObjectSO input)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(input);
        return fryingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO input)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(input);
        if (fryingRecipeSO)
        {
            return fryingRecipeSO.output;
        }
        Debug.LogError("Cutting recipe not found for input: " + input);
        return null;
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO input)
    {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == input)
            {
                return fryingRecipeSO;
            }
        }
        return null;
    }

    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO input)
    {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if (burningRecipeSO.input == input)
            {
                return burningRecipeSO;
            }
        }
        return null;
    }
}
