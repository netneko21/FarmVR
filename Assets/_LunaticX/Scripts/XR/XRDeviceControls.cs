using UnityEngine;

public class XRDeviceControls : MonoBehaviour
{
    public Transform anchor;

    void Awake ()
    {
        anchor = transform.Find("offset/anchor");
        OnAwake();
    }

    protected virtual void OnAwake(){ }
    
}
