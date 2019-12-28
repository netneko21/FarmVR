using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VRKeys;

namespace ActionsIO
{
    [ComponentIdentifierAttribute(action = ItemActions.VRKeyOff)]
    public class VRKeyOff : ActionIO
    {
        public override void Run()
        {
            Validate();
            Key key = io.transform.GetComponent<Key>();
//            Debug.Log("key OnUnHover "+key.name);
            key.OnUnHover();
        }
    }
}