using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ActionsIO
{
    [ComponentIdentifierAttribute(action = ItemActions.UIInputOff)]
    public class UIInputOff : ActionIO
    {
        public override void Run()
        {
            Validate();
            InputField button = io.transform.GetComponent<InputField>();
            var pointer = new PointerEventData(EventSystem.current);
            ExecuteEvents.Execute(button.gameObject, pointer, ExecuteEvents.pointerExitHandler);
        }
    }
}