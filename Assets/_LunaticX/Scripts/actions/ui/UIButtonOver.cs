using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ActionsIO
{
    [ComponentIdentifierAttribute(action = ItemActions.UIButtonOver)]
    public class UIButtonOver : ActionIO
    {
        public override void Run()
        {
            Validate();
            Button button = io.transform.GetComponent<Button>();
//            Debug.Log("UIButtonOver "+button.name);
            var pointer = new PointerEventData(EventSystem.current);
            ExecuteEvents.Execute(button.gameObject, pointer, ExecuteEvents.pointerEnterHandler);
        }
    }
}