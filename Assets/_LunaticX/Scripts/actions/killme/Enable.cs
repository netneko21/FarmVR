using UnityEngine;

namespace ActionsIO
{
    [ComponentIdentifierAttribute(action = ItemActions.Enable)]
    public class Enable : ActionIO
    {
        public override void Run()
        {
            Validate();
            io.UpdateState(ItemState.Disabled,false);
            io.Enable();
        }
    }
}
