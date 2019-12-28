using UnityEngine;

namespace ActionsIO
{
    [ComponentIdentifierAttribute(action = ItemActions.SeedsSelection)]
    public class SeedsSelection : ActionIO
    {
       
        public override void Run()
        {
            Validate();
            Debug.Log("TryShowSeedMenu");
            TileMenu.instance.TryShowSeedMenu();
        }
    }
}
