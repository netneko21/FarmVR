using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ActionsIO
{
    [ComponentIdentifierAttribute(action = ItemActions.UIInputClick)]
    public class UIInputClick : ActionIO
    {
        public override void Run()
        {
            Validate();
            InputField button = io.transform.GetComponent<InputField>();
            Debug.Log("UIButtonInput "+button.name);
            var pointer = new PointerEventData(EventSystem.current);
            ExecuteEvents.Execute(button.gameObject, pointer, ExecuteEvents.submitHandler);
        }
    }
}