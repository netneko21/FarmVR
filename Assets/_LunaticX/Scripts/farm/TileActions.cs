
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileActions : MonoBehaviour
{
    public Animator animator;
    public Tile tile;
    public TileActionsButton closeBtn,digBtn,waterBtn,plantBtn,harvestBtn,clearBtn;

    public bool CheckQActions(Tile _tile,TileMenu.ActionType _action)
    {
        foreach (TileActionQ action in TileActionsQueue.instance.actions)
        {
            if (action.tile == _tile)
            {
                if (action.type == _action)
                {
                    return true;
                }
            }
        }

        return false;
    }
    

    public void UpdateForTile(Tile _tile)
    {
        tile = _tile;
        digBtn.gameObject.SetActive(tile.vegetable == null && tile.groundState == Tile.GroundStates.wild&&!CheckQActions(tile,TileMenu.ActionType.Dig));
        waterBtn.gameObject.SetActive(tile.groundState != Tile.GroundStates.wild&&!CheckQActions(tile,TileMenu.ActionType.Water));
        plantBtn.gameObject.SetActive(tile.vegetable == null && tile.groundState != Tile.GroundStates.wild&&!CheckQActions(tile,TileMenu.ActionType.Plant));
        harvestBtn.gameObject.SetActive(tile.vegetable != null && tile.readyForHarvest&&!CheckQActions(tile,TileMenu.ActionType.Harvest));
        clearBtn.gameObject.SetActive(tile.vegetable != null&&!CheckQActions(tile,TileMenu.ActionType.Clear));
    }

    public void ActionsKillMe()
    {
        Destroy(gameObject);
    }
    
    public void Hide()
    {
        foreach (InteractiveObject io in GetComponentsInChildren<InteractiveObject>())
        {
           io.DisableColliders();
        }
        
        TileMenu.instance.currentActionTile = null;
        animator.SetBool("show",false);
    }
}
