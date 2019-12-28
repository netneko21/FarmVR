using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VRKeys;

namespace ActionsIO
{
    [ComponentIdentifierAttribute(action = ItemActions.VRKeyOver)]
    public class VRKeyOver : ActionIO
    {
        public override void Run()
        {
            Validate();
            Key key = io.transform.GetComponent<Key>();
//            Debug.Log("key OnHover "+key.name);
            key.OnHover();
        }
    }
}