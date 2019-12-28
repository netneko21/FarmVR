using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

[DisallowMultipleComponent, RequireComponent(typeof(XRInputManager))]
public class XRTracking : Singleton<XRTracking>
{
    public static XRDeviceControls leftController, rightController,head;
    public static  InputDevice leftDevice, rightDevice, headDevice;
    private static readonly Dictionary<XRNode, Transform> linkedTransforms = new Dictionary<XRNode, Transform>();
    
    public static bool lerpTracked;

    protected override void OnAwake()
    {
        SetDevices();
        InitTracked();
        lerpTracked = true;
        //SetOffsets();//2DO check device and apply offset
    }
    
    //offset for Oculus rift when using openvr sdk
    private static readonly Vector3 leftHandPos = new Vector3(0.008f, -0.018f, -0.089f);
    private static readonly Vector3 leftHandRot = new Vector3(45, 0, 0);
    private static readonly Vector3 rightHandPos = new Vector3(-0.008f, -0.018f, -0.089f);
    private static readonly Vector3 rightHandRot = new Vector3(45, 0, 0);
    
    //2DO //rewrite using device.characteristic
    void InitTracked()
    {
        foreach (XRController xrController in GetComponentsInChildren<XRController>())
        {
            if (xrController.handSide == HandSide.Left)
            {
                leftController = xrController;
            }

            if (xrController.handSide == HandSide.Right)
            {
                rightController = xrController;
            }
        }

        head = GetComponentInChildren<XRHead>();
        
    }

    public static XRController GetController(HandSide _side)
    {
        if (_side == HandSide.Left){return (XRController)leftController;}
        if (_side == HandSide.Right){return (XRController)rightController;}

        return null;
    }
    

    private void Update()
    {
        SetDevicePosAndRot(XRNode.Head, head);
        SetDevicePosAndRot(XRNode.LeftHand, leftController);
        SetDevicePosAndRot(XRNode.RightHand, rightController);
    }
    
    static bool TryGetPositionFeature(XRNode _node, out Vector3 _position)
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(_node);
        if(device.isValid)
        {
            if (device.TryGetFeatureValue(CommonUsages.devicePosition, out _position))
                return true;
        }

        _position = Vector3.zero;
        return false;
    }
    
    static bool TryGetRotationFeature(XRNode _node,out Quaternion _rotation)
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(_node);
        if(device.isValid)
        {
            if (device.TryGetFeatureValue(CommonUsages.deviceRotation, out _rotation))
                return true;
        }

        _rotation = Quaternion.identity;
        return false;
    }

    private static void SetDevicePosAndRot(XRNode _trackedDevice, XRDeviceControls _device)
    {
        TryGetPositionFeature(_trackedDevice, out Vector3 position);
        TryGetRotationFeature(_trackedDevice, out Quaternion rotation);
        
        if (lerpTracked)
        {//tracked to real positions
            _device.transform.localPosition = Vector3.Lerp( _device.transform.localPosition, position,Time.deltaTime * 30);
            _device.transform.localRotation = Quaternion.Lerp( _device.transform.localRotation,rotation ,Time.deltaTime * 30);
        }
        else
        {
            _device.transform.localPosition =position;
            _device.transform.localRotation =rotation;
        }
        
        if (linkedTransforms.TryGetValue(_trackedDevice, out var linkedTransform))
        {//linked to positions with offsets
            linkedTransform.position = _device.anchor.position;
            linkedTransform.rotation = _device.anchor.rotation;
        }
    }
    
    public static void SetDevices()
    {
        headDevice = InputDevices.GetDeviceAtXRNode(XRNode.Head);
        leftDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        rightDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }
    
        
    public static void ClearLinks()
    {
        linkedTransforms.Clear();
    }
    public static void AddLinkedTransform(Transform _t,XRNode _n)
    {
        if (!linkedTransforms.ContainsValue(_t))
        {
            linkedTransforms.Add(_n,_t);
        }
    }
    
    public static void RemoveLinkedTransform(Transform _t)
    {
        foreach(KeyValuePair<XRNode, Transform> item in linkedTransforms.Where(kvp => kvp.Value == _t).ToList())
        {
            linkedTransforms.Remove(item.Key);
        }
    }
    
    private static void ShowCurrentlyAvailableXRDevices()
    {
        var inputDevices = new List<InputDevice>();
        InputDevices.GetDevices(inputDevices);
        foreach (var device in inputDevices)
        {
            Debug.Log("Device found with name: " + device.name + " and role: " + device.role);
        }
    }

    public void RecenterOnTarget(Transform _target)
    {
        InputTracking.Recenter ();
        float offsetAngle =  head.anchor.rotation.eulerAngles.y;
        transform.Rotate(0f, _target.rotation.eulerAngles.y, 0f);
              //  transform.Rotate(0f, -offsetAngle, 0f);
        Vector3 offsetPos =   head.anchor.position - transform.position;
        transform.position = _target.position - offsetPos;
    }
}

