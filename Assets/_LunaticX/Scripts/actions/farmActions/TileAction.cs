using UnityEngine;

namespace ActionsIO
{
    [ComponentIdentifierAttribute(action = ItemActions.TileAction)]
    public class TileAction : ActionIO
    {
        public TileMenu.ActionType action; public VegType vegetable;

        public override void Run()
        {
            Validate();
            Debug.Log("Tile Action ");
            switch (action)
            {
                case TileMenu.ActionType.Clear:
                    TileMenu.instance.currentSelectedTile.ClearAdd();
                    break;
                
                case TileMenu.ActionType.Dig:
                    TileMenu.instance.currentSelectedTile.DigAdd();
                    break;
                
                case TileMenu.ActionType.Harvest:
                    TileMenu.instance.currentSelectedTile.HarvestAdd();
                    break;
                
                case TileMenu.ActionType.Plant:
                    TileMenu.instance.currentSelectedTile.PlantSeedAdd(vegetable);
                    break;
                
                case TileMenu.ActionType.Water:
                    TileMenu.instance.currentSelectedTile.WaterAdd();
                    break;
            }
        }
    }
}
