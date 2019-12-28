
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeedsSelectionMenu : MonoBehaviour
{
    public Animator animator;
    public bool isActive;
    public void Show()
    {
        animator.SetBool("show",true);
    }

    public void ActionsKillMe()
    {
        Destroy(gameObject);
    }
    
    public void Hide()
    {
        DisableColliders();
        //TileMenu.instance.currentActionTile = null;
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
