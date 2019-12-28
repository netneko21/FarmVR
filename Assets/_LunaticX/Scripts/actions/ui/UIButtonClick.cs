using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ActionsIO
{
    [ComponentIdentifierAttribute(action = ItemActions.UIButtonClick)]
    public class UIButtonClick : ActionIO
    {
        public override void Run()
        {
            Validate();
            Button button = io.transform.GetComponent<Button>();
//            Debug.Log("UIButtonClick "+button.name);
            var pointer = new PointerEventData(EventSystem.current);
            ExecuteEvents.Execute(button.gameObject, pointer, ExecuteEvents.submitHandler);
        }
    }
}