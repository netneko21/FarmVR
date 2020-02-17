
using UnityEngine;
public class Hand : MonoBehaviour
{
    //public  XRController controller;
    private bool grabbed;
    [SerializeField] private HandSide hand;
    public XRController controller;
        
    // private OVRInput.Controller m_controller;
    [SerializeField]
    private Animator animator = null;
    private void Start()
    {
        animator = transform.GetComponentInChildren<Animator>();
        animator.SetFloat("Hand",(float)hand);
        grabbed = false;
        animator.SetBool("Grabbing",grabbed);
    //   controller = XRTracking.GetController(hand);
        
       
    }

    public void InitEvents()
    {
        controller.OnGripPressed += GrabOn;
        controller.OnGripReleased += GrabOff;
    }
    

    private void GrabOn(XRController _grabbed,ItemEvents _event)
    {
        grabbed = true;
        ChangeState(grabbed);

    }
    private void GrabOff(XRController _grabbed,ItemEvents _event)
    {
        grabbed = false;
        ChangeState(grabbed);

    }

    private void ChangeState(bool _grabbed)
    {
        animator.SetBool("Grabbing",_grabbed); 
    }
}