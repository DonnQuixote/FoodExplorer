using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class StokeCounters : BaseCounter,IHasProgress
{
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;
    private float fryingTimer;
    private float burningTimer;
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public event EventHandler<IHasProgress.OnProcessChangedEventArgs> OnProcessChanged;
    public event EventHandler<OnSpawnFireArgs> OnSpawnFire;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }

    public class OnSpawnFireArgs : EventArgs
    {
        public Vector3 positionSpawnFire;
    }
    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned,
    }

    private State state;

    //anothor way to heat the thing,Coroutine;
    //private void Start()
    //{
    //    StartCoroutine(HandleFryTimer());
    //}

    //private IEnumerator HandleFryTimer()
    //{
    //    yield return new WaitForSeconds(fryingRecipeSO[0].fryingTimerMax);
    //}

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
                    OnProcessChanged?.Invoke(this, new IHasProgress.OnProcessChangedEventArgs
                    {
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                    });
                    if (fryingTimer >= fryingRecipeSO.fryingTimerMax)
                  {
                      //Fried
                      //Debug.Log("Frying");
                      GetKitchenObject().DestroySelf();
                      KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                      state = State.Fried;
                      burningTimer = 0f;
                      burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectsSO());

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        }) ;
                    }
                    break;
                case State.Fried:
                    burningTimer+= Time.deltaTime;
                    OnProcessChanged?.Invoke(this, new IHasProgress.OnProcessChangedEventArgs
                    {
                        progressNormalized = burningTimer / burningRecipeSO.burningTimerMaxs
                    });
                    if (burningTimer >= burningRecipeSO.burningTimerMaxs)
                    {
                        //Fried
                        //Debug.Log("Fried");

                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);
                        state = State.Burned;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });

                        OnProcessChanged?.Invoke(this, new IHasProgress.OnProcessChangedEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }
                    break;
                case State.Burned:
                    Debug.Log("Burned");
                    OnSpawnFire?.Invoke(this, new OnSpawnFireArgs
                    {
                        positionSpawnFire = transform.position
                    });

                    //
                    //
                    state = State.Idle;
                    break;
            }
        }
    }
    public override void Interaction(Player player)
    {
        if (!HasKitchenObject())
        {
            //There is not anything on the KitchenCounter
            if (player.HasKitchenObject())
            {
                //player is carrying something
                if (HasFryingRecipeObject(player.GetKitchenObject().GetKitchenObjectsSO()))
                {
                    //Playing is carrying som ething that can be fried
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectsSO());

                    state = State.Frying;
                    fryingTimer = 0f;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = state
                    });

                    OnProcessChanged?.Invoke(this, new IHasProgress.OnProcessChangedEventArgs
                    {
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                    }); 
                }
                //player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
                //player is not carrying
            }
        }
        else
        {
            //There is something on the KitchenCounter
            if (!player.HasKitchenObject())
            {
                //player is not carrying
                this.GetKitchenObject().SetKitchenObjectParent(player);
                state = State.Idle;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    state = state
                });

                OnProcessChanged?.Invoke(this, new IHasProgress.OnProcessChangedEventArgs
                {
                    progressNormalized = 0f
                });
            } 
            else
            {
                //player is carrying something
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    //player is carrying a  plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectsSO()))
                    {
                        //the top of the ClearCounter is what we want the slices
                        GetKitchenObject().DestroySelf();

                        state = State.Idle;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });

                        OnProcessChanged?.Invoke(this, new IHasProgress.OnProcessChangedEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }
                }
            }
        }
    }

    public override void Interaction_Cut(Player player)
    {

    }

    private bool HasFryingRecipeObject(KitchenObjectsSO input)
    {
        
        //return FindKitchenObjectSO(input) == null ? false : true;
        return FindKitchenObjectSO(input) != null;
    }

    private KitchenObjectsSO FindKitchenObjectSO(KitchenObjectsSO input)
    {
        FryingRecipeSO fryingRecipeSOTemp = GetFryingRecipeSOWithInput(input);
        if (fryingRecipeSOTemp != null)
        {
            return fryingRecipeSOTemp.output;
        }
        else
        {
            return null;
        }
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectsSO inputKitchenObjectSO)
    {
        //Debug.LogFormat("input is {0}", inputKitchenObjectSO.name);

        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == inputKitchenObjectSO)
            {
                //Debug.LogFormat("output is {0}", frying.name);
                return fryingRecipeSO;
            }
        }

        return null;
    }


    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectsSO inputKitchenObjectSO)
    {

        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if (burningRecipeSO.input == inputKitchenObjectSO)
            {
                //Debug.LogFormat("output is {0}", frying.name);
                return burningRecipeSO;
            }
        }

        return null;
    }
}
