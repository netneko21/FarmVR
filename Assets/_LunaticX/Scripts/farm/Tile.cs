
using System;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

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
        invisible = 0,//0 25
        firstStage = 25,//25 50
        secondStage = 50,//50 75
        thirdStage = 75,//75 100
        readyForHarvestStage = 100,//75 100
        deadStage = 200,
                
    }
    public enum WaterStages
    {
        water0_25 = 0,
        water25_50 = 25,
        water50_75 = 50,
        water75_100 = 75,
        dry = 200,
    }
    
   public bool readyForHarvest { get; set; }
   public GrowStates growStateObject;
   public GrowStates _growState;
   public GrowStates growState
   {
       get { return _growState; }
       set
       {
           if(_growState==value)return;
           _growState = value;
           readyForHarvest = growState == GrowStates.deadStage || growState == GrowStates.readyForHarvestStage;
           TryUpdateInfo();
           TryUpdateVegetableObject();
       }
   }
   
   public GroundStates _groundState;
   public GroundStates groundState
   {
       get { return _groundState; }
       set
       {
           if(_groundState==value)return;
           Debug.Log("ground state set from " + _groundState + " to " + value);
           wild.SetActive(value == GroundStates.wild);
           dry.SetActive(value == GroundStates.dry);
           watered.SetActive(value == GroundStates.watered);
           _groundState = value;
           TryUpdateInfo();
       }
   }
   
   public WaterStages _waterState;
   public WaterStages waterState
   {
       get { return _waterState; }
       set
       {
           if(_waterState==value)return;
           _waterState = value;
           TryUpdateInfo();
       }
   }
   
    public VegetableData vegetable;
    public float dryTimer;
    public float actionTimer,actionCompletionTime;
    public float growTimer,dryToWildTime;
    public InteractiveObject io;
    public Transform[] slots4;
    public VegetableObject[] slots4Go;
    public Vector3[] slots4Pos;
    public GameObject watered, dry, wild;
    public void Awake()
    {
        slots4Pos = new Vector3[4];
        
        for(int i=0;i<slots4.Length;i++)
        {
            slots4Pos[i] = slots4[i].localPosition;
        }

        slots4Go = new VegetableObject[4];
        io = GetComponent<InteractiveObject>();
        readyForHarvest = false;
        growState = GrowStates.deadStage;
        groundState = GroundStates.wild;
        dryToWildTime = 500;
    }

    void Start()
    {
       DigAdd();
     
        PlantSeedAdd((VegType)Random.Range(1, 6));
        WaterAdd();
    }
    
    private TileInfo info;
    public void TryUpdateInfo()
    {
        TileMenu.instance.TryUpdateInfoFromTile(this);
    }
    
    public void Update()
    {//todo change to random interval calls instead, move everything outside tile class
        
        if (groundState == GroundStates.wild)
        {
            growState = GrowStates.deadStage;
        }
        else
        {
            dryTimer -= Time.deltaTime;
            if (groundState == GroundStates.watered)
            {//dry if watered
                if (dryTimer < 0)
                {
                    groundState = GroundStates.dry;
                    waterState = WaterStages.dry;
                    dryTimer = dryToWildTime;
                }
                else
                {
                    waterState = (WaterStages) ((int) dryTimer / 25 * 25);
                }
            }
            else
            {
                //disable water timer?
                if (groundState == GroundStates.dry)
                {//dry to wild 
                    if (dryTimer <= 0)
                    {//make ground wild
                        if (vegetable)
                        {//if have vegetable with life longer than ground - kill it.  bug? ground should dry longer than any vegetable? possible vegetables that can survive dry land?
                            growState = GrowStates.deadStage;
                        }
                        groundState = GroundStates.wild;
                    }
                }
            }

            if (vegetable != null)
            {
                if (growState != GrowStates.deadStage)
                {//if plant not dead
                    if (groundState == GroundStates.watered)
                    {//grow only if watered
                        growTimer += Time.deltaTime;
                    }
                    
                    if (growTimer <= vegetable.growthTime)
                    {//update grow timer only until full grown
                        growState = (GrowStates)((int) growTimer / 25 * 25);
                        for(int i=0;i<slots4Go.Length;i++)
                        {
                            if (slots4Go[i] != null)
                            {
                                slots4Go[i].UpdateMe();
                            }
                        }
                    }
                    else
                    {
                        if (growTimer < vegetable.overGrowTime)
                        {
                            growState = GrowStates.readyForHarvestStage;
                        }
                        else
                        {
                            growState = GrowStates.deadStage;
                        }
                    }
                }
                else
                {
                    
                }
            }
            else
            {
                
            }
        }
    }

    public void DigAdd()
    {
        TileActionsQueue.instance.AddAction(this,TileMenu.ActionType.Dig);
        TileMenu.instance.UpdateMenu();
    }
    
    public void DigExecute()
    {
        if(groundState != GroundStates.wild){Debug.LogError("dig on non wild tile");}
        dryTimer = dryToWildTime;
        groundState = GroundStates.dry;
        TileMenu.instance.UpdateMenu();
    }
    
    public void PlantSeedAdd(VegType _vegetable)
    {
        TileActionsQueue.instance.AddAction(this,TileMenu.ActionType.Plant,_vegetable);
        TileMenu.instance.TryHideSeedMenu();
        TileMenu.instance.UpdateMenu();
    }

    public void PlantSeedExecute(VegType _vegetable)
    {
        Debug.Log("_vegetable "+_vegetable);
        if(groundState == GroundStates.wild){Debug.LogError("plant on wild tile");}
        if(vegetable != null){Debug.LogError("plant on another plant");}
        vegetable = VegetableManager.instance.Get(_vegetable);
        readyForHarvest = false;
        growStateObject = GrowStates.deadStage;
        growState = GrowStates.invisible;
        growTimer = 0;
        TileMenu.instance.TryHideSeedMenu();
        TileMenu.instance.UpdateMenu();
    }
    

    public void HarvestAdd()
    {
        TileActionsQueue.instance.AddAction(this,TileMenu.ActionType.Harvest);
        TileMenu.instance.TryHideInfo();
    }
    
    public void HarvestExecute()
    {
        if(groundState == GroundStates.wild){Debug.LogError("harvest on wild tile");}
        if(vegetable == null){Debug.LogError("harvest on empty plant");}
        if(!readyForHarvest){Debug.LogError("harvest on not ready plant");}
        CollectVegetable();
        ClearExecute();

        TileMenu.instance.TryHideInfo();
    }
    
    public void WaterAdd()
    {
        TileActionsQueue.instance.AddAction(this,TileMenu.ActionType.Water);
        TileMenu.instance.UpdateMenu();
        TileMenu.instance.TryHideInfo();
    }
    
    public void WaterExecute()
    {
        if(groundState == GroundStates.wild){Debug.LogError("water on wild tile");}
        if(groundState == GroundStates.watered){Debug.Log("water on watered tile, refresh");}
        Debug.Log("Water on tile "+transform.parent.gameObject.name);
        dryTimer = 100;
        waterState = WaterStages.water75_100;
        groundState = GroundStates.watered;
        TileMenu.instance.UpdateMenu();
        TileMenu.instance.TryHideInfo();
    }
    
    public void ClearAdd()
    {
        TileActionsQueue.instance.AddAction(this,TileMenu.ActionType.Clear);

        TileMenu.instance.UpdateMenu();
        TileMenu.instance.TryHideInfo();
    }
    
    public void ClearExecute()
    {
        if(vegetable == null){Debug.LogError("clear on empty plant");}
        vegetable = null;
        
        for(int i=0;i<slots4.Length;i++)
        {
            if(slots4Go[i]!=null){slots4Go[i].Hide();}
        }
        
        TileMenu.instance.UpdateMenu();
        TileMenu.instance.TryHideInfo();
    }

    public void CollectVegetable()
    {
        if(vegetable == null){Debug.LogError("CollectVegetable on empty plant");}
    }

    public void ReplaceVegetableObject(GameObject _prefab,int _growStage,int _timer)
    {
        for(int i=0;i<slots4.Length;i++)
        {//4 slots in tile, fill or replace with 4 vegetable prefabs
            if(slots4Go[i]!=null){slots4Go[i].Hide();}//clear if not empty
            slots4Go[i] = Instantiate(_prefab,slots4[i]).GetComponent<VegetableObject>();
            slots4Go[i].Show(vegetable.bushStartScale[_growStage],_timer);

            if (vegetable.bushOffset.Length > 0)
            {
                slots4[i].localPosition = slots4Pos[i] + vegetable.bushOffset[_growStage];
            }
        }
    }

    public void TryUpdateVegetableObject()
    {
        if(growState == growStateObject){return;}
        
        switch (growState)
        {
            case GrowStates.invisible:
                growStateObject = GrowStates.invisible;
                ReplaceVegetableObject(vegetable.early, 0, 50);
                break;
            case GrowStates.firstStage:
               
                break;
            case GrowStates.secondStage:
                growStateObject = GrowStates.secondStage; 
                ReplaceVegetableObject(vegetable.mid, 1, 25);
  
                break;
            case GrowStates.thirdStage:
                growStateObject = GrowStates.thirdStage;
                ReplaceVegetableObject(vegetable.late, 2, 25);
                break;
            case GrowStates.readyForHarvestStage:
              
                break;
        }
    }

}
