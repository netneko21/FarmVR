using UnityEngine;
using UnityEngine.XR;

public class XRController : XRDeviceControls
{
    public Hand playerHand;
    public HandSide handSide;
    public float triggerValue;
    public bool isGripPressed;
    public bool isTriggerPressed;
    public Vector2 thumbAxis;

    public delegate void InputEvent (XRController controller,ItemEvents type = 0);
    public event InputEvent OnGripReleased,OnGripPressed;
    public event InputEvent OnTriggerReleased,OnTriggerPressed;
    public event InputEvent OnThumbReleased,OnThumbPressed,OnThumbLeft,OnThumbRight,OnThumbUp,OnThumbDown;
    private Vector2 lastDirection;
    
    protected override void OnAwake ()
    {
        XRInputManager.AddUsageBtnEvent(CommonUsages.triggerButton,new XREventButton(isTriggerPressed,this, OnTriggerEvent));
        XRInputManager.AddUsageBtnEvent(CommonUsages.gripButton, new XREventButton(isGripPressed, this,OnGripEvent));

        //OnTriggerPressed += Focus.TriggerLast();
        //OnTriggerReleased += 
    }
    
    public void SetHand(Hand _hand)
    {
        playerHand = _hand;
        _hand.controller = this;
        _hand.InitEvents();
    }
    
    public void OnThumbAxisEvent(Vector2 axis)
    {
        if (thumbAxis == Vector2.zero && axis != Vector2.zero)
        {
            OnThumbPressed?.Invoke(this);
        }

        if (thumbAxis != Vector2.zero && axis == Vector2.zero)
        {
            OnThumbReleased?.Invoke(this);
            if(lastDirection == Vector2.up){OnThumbUp?.Invoke(this);}
            if(lastDirection == Vector2.down){OnThumbDown?.Invoke(this);}
            if(lastDirection == Vector2.left){OnThumbLeft?.Invoke(this);}
            if(lastDirection == Vector2.right){OnThumbRight?.Invoke(this);}
        }
        
        thumbAxis = axis;
        SetLastDirection(thumbAxis);
    }
    
    
    void SetLastDirection(Vector2 _axis)
    {
        lastDirection = Vector2.zero;
        if(Mathf.Abs(_axis.x)>Mathf.Abs(_axis.y))
        {//horizontal
            lastDirection.x = Mathf.Sign(_axis.x);
        }
        else
        {//vertical
            lastDirection.y = Mathf.Sign(_axis.y);
        }
    }
    
    public void OnGripEvent(bool _pressed)
    {
        if (isGripPressed&&!_pressed){OnGripReleased?.Invoke(this);}
        if (!isGripPressed&&_pressed){OnGripPressed?.Invoke(this);}
        isGripPressed = _pressed;
    }
    public void OnTriggerEvent(bool _pressed)
    {
        if (isTriggerPressed&&!_pressed){Focus.SendEvent(ItemEvents.onTriggerReleased);OnTriggerReleased?.Invoke(this);}
        if (!isTriggerPressed&&_pressed){Focus.SendEvent(ItemEvents.onTriggerPressed);OnTriggerPressed?.Invoke(this);}
        isTriggerPressed = _pressed;
    }
    
    public bool IfButtonPressed(HandButtons _btn)
    {
        if (_btn == HandButtons.Trigger)
        {
            return isTriggerPressed;
        }
        if (_btn == HandButtons.Grip)
        {
            return isGripPressed;
        }

        return false;
    }
    
    public virtual void OnTriggerValueEvent(float _value)
    {
        triggerValue = _value;
    }
}