using UnityEngine;

namespace ActionsIO
{    
    [ComponentIdentifierAttribute(action = ItemActions.GoEnable)]
    public class GoEnable : ActionIO
    {
        public override void Run()
        {
            Validate();
            Debug.Log("go enable on " + io.gameObject.name);
            //io.gameObject.SetActive(true);
        }
    }
}

