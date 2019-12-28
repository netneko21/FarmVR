
using System;
using UnityEngine;
using UnityEngine.UI;

public class TileMenu : Singleton<TileMenu>
{
    public GameObject infoMenuPrefab,actionsMenuPrefab;
    public Transform menuRoot;
    public float menuDistance = 2.5f;
    public Tile currentInfoTile,currentActionTile;
    public float highlighterHeight = 1;
    public GameObject tileHighlighter;
    public TileActions actions;
    public TileInfo info;
    public Sprite deadVegetable;
    public enum MenuType
    {
        Info = 0,
        Actions = 1,
        Seeds = 2,
    }
    public enum ActionType
    {
        Plant = 0,
        Water = 1,
        Dig = 2,
        Clear = 3,
        Harvest = 4,
    }

    public void TryShowInfo(Tile _tile)
    {
        if (info == null && actions == null)
        {
            currentInfoTile = _tile;
            //TileMenu.instance.menuRoot.LookAt(transform,TileMenu.instance.menuRoot.up);
            menuRoot.LookAt(currentInfoTile.transform,Vector3.up);
            GameObject go = Instantiate(infoMenuPrefab, menuRoot.position, menuRoot.rotation);
            go.transform.position +=  menuRoot.forward * menuDistance; 
            info = go.GetComponent<TileInfo>();
            info.UpdateForTile(currentInfoTile);
            //currentInfoTile.info = info;
            tileHighlighter.transform.position = currentInfoTile.transform.position + new Vector3(0, highlighterHeight, 0);
        }
    }
    
    public void TryHideInfo()
    {
        if (info != null && actions == null)
        {
            info.Hide();
            info = null;
        }
    }
    
    public void TryShowActions()
    {
        if (currentInfoTile != null)// &&;// )
        {
            if (currentInfoTile == currentActionTile){Debug.Log("currentInfoTile == currentActionTile in try show actions");TryHideActions();return;}
            
                Debug.Log("showing");
                if (actions != null){Debug.LogError("actions != null in try show actions");}

                GameObject go = Instantiate(actionsMenuPrefab, info.transform.position,info.transform.rotation);
                //go.transform.position +=  menuRoot.forward * menuDistance; 
                actions = go.GetComponent<TileActions>();
                actions.UpdateForTile(currentInfoTile);
                currentActionTile = currentInfoTile;
        }
    }
    
    public void TryHideActions()
    {
        Debug.Log("TryHideActions");
        if (actions != null)
        {
            Debug.Log("hide act");
            actions.Hide();
            actions = null;
            info.Hide();
            info = null;
            currentActionTile = null;
        }
    }

    private SeedsSelectionMenu seedMenu;
    
    public void TryShowSeedMenu()
    {
        seedMenu.Show();
    }
    
    public void TryHideSeedMenu()
    {
        seedMenu.Hide();
    }
}
