using UnityEngine;

namespace ActionsIO
{
    [ComponentIdentifierAttribute(action = ItemActions.Rotate)]
    public class Rotate : ActionIO
    {
        public override void Run()
        {
            Validate();
        }
        
        void Update()
        {
            if (mode == ActionMode.Run)
            {
                transform.Rotate(0, 1, 0, Space.Self);
            }
        }
    }
}