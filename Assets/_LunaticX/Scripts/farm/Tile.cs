
using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum GroundStates
    {
        wild = 0,
        dry = 1,
        watered = 2
    }
    
    public enum GrowStates
    {
        invisible = 0,
        firstStage = 1,
        secondStage = 2,
        thirdStage = 3
    }
    
   public bool readyForHarvest { get; set; }
   public GrowStates growState{ get; set; }
   public GroundStates _groundState;
   public GroundStates groundState
   {
       get => _groundState;
       set
       {
           if(_groundState==value)return;
           wild.SetActive(value==GroundStates.wild);
           dry.SetActive(value==GroundStates.dry);
           watered.SetActive(value==GroundStates.watered);
       }
   }

    public VegetableData vegetable;
    public float dryTimer,dryTime,wildTime;
    public float actionTimer,actionCompletionTime;
    public float growTimer, growTime=100;
    public float  overGrowTime=200;

    public GameObject watered, dry, wild;

    public void Awake()
    {
        readyForHarvest = false;
        //vegetable = null;
        groundState = GroundStates.wild;
    }

    public void Update()
    {
        if (groundState == GroundStates.wild)
        {
            
        }
        else
        {
            if (groundState == GroundStates.watered)
            {
                dryTimer += Time.deltaTime;
                if (dryTime >= dryTime)
                {
                    groundState = GroundStates.dry;
                }
            }
            else
            {
                if (groundState == GroundStates.dry)
                {
                    dryTimer += Time.deltaTime;
                    if (dryTime >= wildTime)
                    {
                        groundState = GroundStates.wild;
                    }
                }
            }

            if (vegetable != null)
            {
                if (groundState == GroundStates.watered)
                {
                    growTimer += Time.deltaTime;
                }
                else
                {
                
                }
            
                readyForHarvest = growTimer >= growTime;
            }   
        }
    }

    public Sprite GetSprite()
    {
        if (groundState == GroundStates.wild)
        {
            if (vegetable != null)
            {
                return TileMenu.instance.deadVegetable;
            }
            else
            {
                return null;
            }
        }
        else
        {
            
        }
        return vegetable.IconSprite;
    }

    public void Dig()
    {
        if(groundState!=GroundStates.wild){Debug.LogError("dig on non wild tile");}
        Debug.Log("dig on tile "+transform.parent.gameObject.name);
        groundState = GroundStates.dry;
    }
    
    public void PlantSeed(VegetableData _vegetable)
    {
        if(groundState == GroundStates.wild){Debug.LogError("plant on wild tile");}
        if(vegetable != null){Debug.LogError("plant on another plant");}
        vegetable = _vegetable;
        Debug.Log("Plant on tile "+transform.parent.gameObject.name);
    }
    
    public void Harvest()
    {
        if(groundState == GroundStates.wild){Debug.LogError("harvest on wild tile");}
        if(vegetable == null){Debug.LogError("harvest on empty plant");}
        if(!readyForHarvest){Debug.LogError("harvest on not ready plant");}
        Debug.Log("Harvest on tile "+transform.parent.gameObject.name);
    }
    
    public void Water()
    {
        if(groundState == GroundStates.wild){Debug.LogError("water on wild tile");}
        if(groundState == GroundStates.watered){Debug.Log("water on watered tile, refresh");}

        Debug.Log("Water on tile "+transform.parent.gameObject.name);
    }
    
    public void Clear()
    {
        if(vegetable == null){Debug.LogError("clear on empty plant");}

        vegetable = null;
        Debug.Log("Clear on tile "+transform.parent.gameObject.name);
    }
    
    
}
