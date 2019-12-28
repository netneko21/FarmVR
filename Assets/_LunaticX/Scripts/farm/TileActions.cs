
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileActions : MonoBehaviour
{
    public Animator animator;
    public Tile tile;
    public TileActionsButton closeBtn,digBtn,waterBtn,plantBtn,harvestBtn,clearBtn;
    public void UpdateForTile(Tile _tile)
    {
        tile = _tile;
        closeBtn.ActivateButton(true);
        digBtn.ActivateButton(tile.groundState == Tile.GroundStates.wild && tile.vegetable == null);
        waterBtn.ActivateButton(tile.groundState == Tile.GroundStates.dry);
        plantBtn.ActivateButton(tile.vegetable == null && tile.groundState == Tile.GroundStates.watered);
        harvestBtn.ActivateButton(tile.vegetable != null && tile.readyForHarvest);
        clearBtn.ActivateButton(tile.vegetable != null && tile.groundState == Tile.GroundStates.wild);
    }

    public void ActionsKillMe()
    {
        Destroy(gameObject);
    }
    
    public void Hide()
    {
        DisableColliders();
        TileMenu.instance.currentActionTile = null;
        animator.SetBool("show",false);
    }

    public void EnableColliders()
    {
        ToggleColliders(true);
    }

    public void DisableColliders()
    {
        ToggleColliders(false);
    }
    
    public void ToggleColliders(bool _isActive)
    {
        foreach (Collider collider in GetComponentsInChildren<Collider>())
        {
            collider.enabled = _isActive;
        }
    }
}
