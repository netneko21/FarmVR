using UnityEngine;


namespace ActionsIO
{
    [ComponentIdentifierAttribute(action = ItemActions.MenuToggle)]
    public class MenuToggle : ActionIO
    {
        public TileMenu.MenuType menu;
        public bool activate;
        public override void Run()
        {
            Validate();
            
            if(menu == TileMenu.MenuType.Info)
            {
                Tile tile = GetComponent<Tile>();
                if(tile==null){Debug.LogError("tile null on InfoMenu");return;}
                if (activate)
                { 
                    TileMenu.instance.TryShowInfo(tile);
                }
                else
                {
                    TileMenu.instance.TryHideInfo();
                }
            }
            
            if(menu == TileMenu.MenuType.Actions)
            {
                if (activate)
                { 
                    TileMenu.instance.TryShowActions();
                }
                else
                {
                    TileMenu.instance.TryHideActions();
                }
            }
            
            if(menu == TileMenu.MenuType.Seeds)
            {
                if (activate)
                { 
                    TileMenu.instance.TryShowSeedMenu();
                }
                else
                {
                    TileMenu.instance.TryHideSeedMenu();
                }
            }
        }
    }
}

