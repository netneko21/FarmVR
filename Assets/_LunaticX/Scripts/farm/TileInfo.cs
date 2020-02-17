
using System;
using UnityEngine;
using UnityEngine.UI;

public class TileInfo : MonoBehaviour
{
    public Image icon,iconBg;
    //public Image readyIcon,warningIcon,failureIcon;
    public Animator animator;
    
    public Image waterIcon, growIcon, actionIcon;
    public Image waterTimer, growTimer, actionTimer;
    public GameObject waterState,growState,actionState;
    
    public void UpdateForTile(Tile _tile)
    {
        UpdateGroundIcon(_tile);
        UpdateActionIcon(_tile);
        UpdateWaterIcon(_tile);
        UpdateGrowthIcon(_tile);
    }
    
    public void UpdateGroundIcon(Tile _tile)
    {
        if (_tile.vegetable == null)
        {
            switch (_tile.groundState)
            {
                case Tile.GroundStates.wild:
                    icon.sprite = TileMenu.instance.wildGround;
                    iconBg.color = new Color(1,0.39f,0.39f);
                    break;
                case Tile.GroundStates.dry:
                    icon.sprite = TileMenu.instance.dryGround;
                    iconBg.color = new Color(1,0.78f,0.58f);
                    break;
                case Tile.GroundStates.watered:
                    icon.sprite = TileMenu.instance.wateredGround;
                    iconBg.color = new Color(0.78f,1,0.78f);
                    break;
            }
        }
        else
        {
            switch (_tile.groundState)
            {
                case Tile.GroundStates.wild:
                    icon.sprite = TileMenu.instance.deadVegetable;
                    iconBg.color = new Color(1,0.39f,0.39f);
                    break;
                case Tile.GroundStates.dry:
                    icon.sprite = _tile.vegetable.IconSprite;
                    iconBg.color = new Color(1,0.78f,0.58f);
                    break;
                case Tile.GroundStates.watered:
                    icon.sprite = _tile.vegetable.IconSprite;
                    iconBg.color = new Color(0.78f,1,0.78f);
                    break;
            }
        }
    }
    public void UpdateActionIcon(Tile _tile)
    {
       // Destroy(gameObject);
    }
    public void UpdateWaterIcon(Tile _tile)
    {
        if (_tile.groundState == Tile.GroundStates.wild || !_tile.vegetable)
        {
            
            waterState.gameObject.SetActive(false);
        }
        else
        {
            waterState.gameObject.SetActive(true);
            if (_tile.groundState == Tile.GroundStates.watered)
            {
                waterTimer.fillAmount = _tile.dryTimer/_tile.vegetable.dryTime;
            }
            else
            {
                waterTimer.fillAmount = 0;
            }
            
            switch (_tile.waterState)
            {
                case Tile.WaterStages.water0_25:
                    waterIcon.sprite = TileMenu.instance.waterIcons[0];
                    break;
                case Tile.WaterStages.water25_50:
                    waterIcon.sprite = TileMenu.instance.waterIcons[1];
                    break;
                case Tile.WaterStages.water50_75:
                    waterIcon.sprite = TileMenu.instance.waterIcons[2];
                    break;
                case Tile.WaterStages.water75_100:
                    waterIcon.sprite = TileMenu.instance.waterIcons[3];
                    break;
                case Tile.WaterStages.dry:
                    waterIcon.sprite = TileMenu.instance.waterIcons[4]; 
                    break;
            }
        }
    }
    
    public void UpdateGrowthIcon(Tile _tile)
    {
        if (!_tile.vegetable||_tile.growState == Tile.GrowStates.deadStage){ growState.gameObject.SetActive(false);    return;}
    
            growState.gameObject.SetActive(true);
            if ( _tile.growTimer >= _tile.vegetable.growthTime)
            {
                growTimer.fillAmount = 1;
            }
            else
            {
                growTimer.fillAmount = _tile.growTimer/_tile.vegetable.growthTime;
            }
            
            switch (_tile.growState)
            {
                case Tile.GrowStates.invisible:
                    growIcon.sprite = TileMenu.instance.growIcons[0];
                    break;
                case Tile.GrowStates.firstStage:
                    growIcon.sprite = TileMenu.instance.growIcons[1];
                    break;
                case Tile.GrowStates.secondStage:
                    growIcon.sprite = TileMenu.instance.growIcons[2];
                    break;
                case Tile.GrowStates.thirdStage:
                    growIcon.sprite = TileMenu.instance.growIcons[3];
                    break;
                case Tile.GrowStates.readyForHarvestStage:
                    growIcon.sprite = TileMenu.instance.growIcons[4];
                    break;
            }
        
    }

    public void InfoKillMe()
    {
        Destroy(gameObject);
    }
    
    public void Hide()
    {
        animator.SetBool("show",false);
    }
}
