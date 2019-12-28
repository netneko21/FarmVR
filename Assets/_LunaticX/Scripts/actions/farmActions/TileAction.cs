using UnityEngine;

namespace ActionsIO
{
    [ComponentIdentifierAttribute(action = ItemActions.TileAction)]
    public class TileAction : ActionIO
    {
        public TileMenu.ActionType action; public VegetableData vegetable;

        public override void Run()
        {
            Validate();
            Debug.Log("Tile Action ");
            switch (action)
            {
                case TileMenu.ActionType.Clear:
                    TileManager.instance.currentTile.Clear();
                    break;
                
                case TileMenu.ActionType.Dig:
                    TileManager.instance.currentTile.Dig();
                    break;
                
                case TileMenu.ActionType.Harvest:
                    TileManager.instance.currentTile.Harvest();
                    break;
                
                case TileMenu.ActionType.Plant:
                    TileManager.instance.currentTile.PlantSeed(vegetable);
                    break;
                
                case TileMenu.ActionType.Water:
                    TileManager.instance.currentTile.Water();
                    break;
            }
        }
    }
}
