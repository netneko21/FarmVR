using UnityEngine;

namespace ActionsIO
{    
    [ComponentIdentifierAttribute(action = ItemActions.GoDisable)]
    public class GoDisable : ActionIO
    {
        public override void Run()
        {
            Validate();
            Debug.Log("go GoDisable on " + io.gameObject.name);
            io.gameObject.SetActive(false);
        }
    }
}

