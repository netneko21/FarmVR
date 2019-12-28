using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

[DisallowMultipleComponent]
public class XRInputManager : MonoBehaviour
{
    static Dictionary<InputFeatureUsage<bool>, List<XREventButton>> buttons = new Dictionary<InputFeatureUsage<bool>, List<XREventButton>>();
    static Dictionary<InputFeatureUsage<float>, List<XREventFloat>> touch = new Dictionary<InputFeatureUsage<float>, List<XREventFloat>>();
    static Dictionary<InputFeatureUsage<Vector2>, List<XREventAxis>> axis = new Dictionary<InputFeatureUsage<Vector2>, List<XREventAxis>>();
    
    private void LateUpdate()
    {
        UpdateInputs(XRTracking.leftDevice,XRTracking.leftController);
        UpdateInputs(XRTracking.rightDevice,XRTracking.rightController);
    }
    
    public static void AddUsageBtnEvent (InputFeatureUsage<bool> _feature, XREventButton _event)
    {
        if (buttons.TryGetValue(_feature, out List<XREventButton> value))
        {
            if (!value.Contains(_event))
            {
                value.Add(_event);
            }
        }
        else
        {
            List<XREventButton> list = new List<XREventButton>();
            list.Add(_event);
            buttons.Add(_feature, list);
        }
    }

    private void UpdateInputs(InputDevice _device,XRDeviceControls _controller)
    {
        if (_device.isValid)
        {
            if (_controller)
            {
                foreach (KeyValuePair<InputFeatureUsage<bool>, List<XREventButton>> kvp in buttons)
                {
                    foreach (XREventButton eventButton in kvp.Value)
                    {
                        if (eventButton.controller == _controller)
                        {
                            _device.TryGetFeatureValue(kvp.Key, out bool tempState);
                            if (tempState != eventButton.value)
                            {
                                eventButton.Invoke(tempState);
                                eventButton.value = tempState;
                            }
                        }
                    }
                }
            
                foreach (KeyValuePair<InputFeatureUsage<Vector2>, List<XREventAxis>> kvp in axis)
                {
                    foreach (XREventAxis eventAxis in kvp.Value)
                    {
                        if (eventAxis.controller == _controller)
                        {
                            _device.TryGetFeatureValue(kvp.Key, out Vector2 tempState);
                            if (tempState != eventAxis.value)
                            {
                                eventAxis.Invoke(tempState);
                                eventAxis.value = tempState;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            XRTracking.SetDevices();
        }
    }
}


public class XREventAxis : UnityEvent<Vector2>
{
    public Vector2 value { get; set; }
    public XRController controller { get; set; }
    public XREventAxis(Vector2 _value,XRController _controller, UnityAction<Vector2> _method)
    {
        value = _value;
        controller = _controller;
        AddListener(_method);
    }
}

public class XREventFloat : UnityEvent<float>
{
    public float value { get; set; }
    public XRController controller { get; set; }
    public XREventFloat(float _value, XRController _controller,UnityAction<float> _method)
    {
        value = _value;
        controller = _controller;
        AddListener(_method);
    }
}

public class XREventButton : UnityEvent<bool>
{
    public bool value { get; set; }
    public XRController controller { get; set; }
    public XREventButton(bool _value,XRController _controller, UnityAction<bool> _method)
    {
        value = _value;
        controller = _controller;
        AddListener(_method);
    }
}