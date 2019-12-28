
using System;
using UnityEngine;
using UnityEngine.UI;

public class TileInfo : MonoBehaviour
{
    public Image icon;
    public Animator animator;

    public void UpdateForTile(Tile _tile)
    {
        icon.sprite = _tile.GetSprite();
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
