using UnityEngine;

namespace ActionsIO
{    
    [ComponentIdentifierAttribute(action = ItemActions.Disable)]
    public class Disable : ActionIO
    {
        public override void Run()
        {
            Validate();
            io.UpdateState(ItemState.Disabled,true);
            io.Disable();     
            Debug.Log("Disable");
        }
    }
}

