using System.Collections.Generic;
using UnityEngine;

public class Focus : MonoBehaviour
{
    private InteractiveObject currentInteractiveObject;
    static List<Focus> focusOrder = new List<Focus>();
    public void AddToOrderingList(InteractiveObject _interactiveObject)
    {
        if (focusOrder.Count > 0)
        {
            focusOrder[focusOrder.Count - 1].currentInteractiveObject.SetHovered();
        }
        focusOrder.Add(this);
        currentInteractiveObject = _interactiveObject;
    }
    
    public bool RemoveFromOrderingList(InteractiveObject _interactiveObject)
    {
        bool removeFocus = true;
        focusOrder.Remove(this);
        if (focusOrder.Count > 0)
        {
            foreach (Focus focus in focusOrder)
            {
                if (focus.currentInteractiveObject == _interactiveObject)
                {
                    removeFocus = false;
                }
                focus.currentInteractiveObject.SetHovered();
            }

            focusOrder[focusOrder.Count - 1].currentInteractiveObject.SetHoveredLast();   
        }

        return removeFocus;

    }
    public static InteractiveObject GetLast()
    {
        if (focusOrder.Count > 0)
        {
            return focusOrder[focusOrder.Count - 1].currentInteractiveObject;
        }
        
        return null;
    }
    
    public static void SendEvent(ItemEvents _event)
    {
        if (GetLast())
        {
            GetLast().FireEvent(_event);
        }
    }
    
    public static void SendTrigger(ActivationTriggers _trigger)
    {
        if (GetLast())
        {
            GetLast().FireTrigger(_trigger);
        }
    }
}


