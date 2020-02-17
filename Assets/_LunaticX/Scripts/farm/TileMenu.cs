
using System;
using UnityEngine;
using UnityEngine.UI;

public class TileMenu : Singleton<TileMenu>
{
    public GameObject infoMenuPrefab,actionsMenuPrefab;
    public Transform menuRoot;
    public float menuDistance = 2.5f;
    public Tile currentActionTile;
    private Tile _currentSelected;
    public Tile currentSelectedTile
    {
        get { return _currentSelected;}
        set
        {
            if (_currentSelected == value) return;
            _currentSelected = value;
        }
    }
    
    public float highlighterHeight = 1;
    public GameObject tileHighlighter;
    public TileActions actions;
    public TileInfo info;
    public Sprite deadVegetable,wildGround,dryGround,wateredGround;
    public Sprite[] waterIcons,growIcons,actionsIcons;
    public SeedsSelectionMenu seeds;
    public enum MenuType
    {
        Info = 0,
        Actions = 1,
        Seeds = 2,
        Selection = 3
    }
    public enum ActionType
    {
        Plant = 0,
        Water = 1,
        Dig = 2,
        Clear = 3,
        Harvest = 4,
    }
    
    public void TrySelectTile(Tile _tile)
    {
        if (currentSelectedTile != _tile)
        {
            currentSelectedTile = _tile;
            tileHighlighter.transform.position = currentSelectedTile.transform.position + new Vector3(0, highlighterHeight, 0);
        }
    }
    
    public void TryDeSelectTile(Tile _tile)
    {
        if (!info && !actions)
        {
            if (currentSelectedTile == _tile)
            {
                currentSelectedTile = null;
                tileHighlighter.transform.position = Vector3.down * 100;
            }
        }
    }
    
    public void TryShowInfo(Tile _tile)
    {
        if (!info && !actions)
        {
            currentSelectedTile = _tile;
            menuRoot.LookAt(currentSelectedTile.transform,Vector3.up);
            GameObject go = Instantiate(infoMenuPrefab, menuRoot.position, menuRoot.rotation);
            go.transform.position +=  menuRoot.forward * menuDistance; 
            info = go.GetComponent<TileInfo>();
            info.UpdateForTile(currentSelectedTile);

            tileHighlighter.transform.position = currentSelectedTile.transform.position + new Vector3(0, highlighterHeight, 0);
        }
    }
    
    public void TryHideInfo()
    {
        if (info!=null && actions==null)
        {
            info.Hide();
            info = null;
            currentSelectedTile = null;
        }
    }
    
    public void TryShowActions()
    {
        if (currentSelectedTile!=null)
        {
            if (currentSelectedTile == currentActionTile){Debug.Log("currentInfoTile == currentActionTile in try show actions");TryHideActions();return;}
            if (actions){Debug.LogError("actions != null in try show actions");}
            GameObject go = Instantiate(actionsMenuPrefab, info.transform.position - info.transform.up * 0.4f,info.transform.rotation);
            actions = go.GetComponent<TileActions>();
            actions.UpdateForTile(currentSelectedTile);
            currentActionTile = currentSelectedTile;
        }
    }

    public void UpdateMenu()
    {
        TryUpdateInfo();
        TryUpdateActions();
    }
    
    public void TryUpdateInfoFromTile(Tile _tile)
    {
        if (_tile == currentSelectedTile)
        {
            TryUpdateInfo();
            TryUpdateActions();
        }
    }
    public void TryUpdateInfo()
    {
        info?.UpdateForTile(currentSelectedTile);
    }
    
    public void TryUpdateActions()
    {
        actions?.UpdateForTile(currentSelectedTile);
    }

    public void TryHideActions()
    {
        if (actions)
        {
            TryHideSeedMenu();
            actions.Hide();
            actions = null;
            info.Hide();
            info = null;
            currentActionTile = null;
        }
    }

    public void TryShowSeedMenu()
    {
        if (actions)
        {
            seeds.Show(actions.plantBtn.transform);
        }
    }
    
    public void TryHideSeedMenu()
    {
        if (actions)
        {
            seeds.Hide();
        }
    }
}
