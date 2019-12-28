using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ActionsIO
{
    [ComponentIdentifierAttribute(action = ItemActions.UIButtonOff)]
    public class UIButtonOff : ActionIO
    {
        public override void Run()
        {
            Validate();
            Button button = io.transform.GetComponent<Button>();
            var pointer = new PointerEventData(EventSystem.current);
            ExecuteEvents.Execute(button.gameObject, pointer, ExecuteEvents.pointerExitHandler);
        }
    }
}