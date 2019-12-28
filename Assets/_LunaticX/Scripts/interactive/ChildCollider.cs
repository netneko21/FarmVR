
using UnityEngine;

public class ChildCollider : MonoBehaviour
{
    public InteractiveObject io;

    private void Awake()
    {
        io = GetComponentInParent<InteractiveObject>();
        if(io == null){Debug.LogError("no io on child collider?");}
    }

    public void OnTriggerEnter(Collider _collider)
    {
        if (_collider.CompareTag("IOTrigger"))
        {
            io.TryAddFocus(_collider.GetComponent<Focus>());
        }
    }

    public void OnTriggerExit(Collider _collider)
    {
        if (_collider.CompareTag("IOTrigger"))
        {
            io.TryRemoveFocus(_collider.GetComponent<Focus>());
        }
    }
}





