using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VRKeys;

namespace ActionsIO
{
    [ComponentIdentifierAttribute(action = ItemActions.VRKeyClick)]
    public class VRKeyClick : ActionIO
    {
        public override void Run()
        {
            Validate();
            Key key = io.transform.GetComponent<Key>();
//            Debug.Log("key OnPress "+key.name);
            key.OnPress();
        }
    }
}