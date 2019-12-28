using UnityEngine;
public class Grabber : MonoBehaviour
{
    //2DO hand focus priority
    private bool grabbed;
    public InteractiveObject grabbedObject;
    public XRController controller;
    //private bool grabFromShow;
    private Vector3 grabDiff;
    public Vector3 grabOffset;
    private void Start()
    {
        controller.OnGripPressed += GrabOn;
        controller.OnGripReleased += GrabOff;
    }

    public Grabber other;
    private Quaternion grabTargetRotation;
    private bool snapped = false;
    private Vector3 grabTargetPosition;
    private void Update()
    {
        /*
        if (grabbedObject)
        {
            
            ShowArea.UpdateBorders(grabbedObject);
            // grabbedObject.transform.position = transform.position + grabOffset; + grabOffset
            grabbedObject.transform.localPosition = Vector3.Lerp(grabbedObject.transform.localPosition,grabTargetPosition,0.1f);
            grabbedObject.transform.localRotation = Quaternion.Lerp(grabbedObject.transform.localRotation,grabTargetRotation,0.1f);
          
                if (grabbedObject.canDraw && snapped)
                {
                    if (controller.isTriggerPressed)
                    {
//                        grabbedObject.drawer.canDraw = true;
                        if (other.grabbedObject != null)
                        {
                      //      if (grabbedObject.drawer.lineDrawPrefab != null)
                      //      {
                               
                      //      }
                        }
                        else
                        {
                                
                        }
                    }
                    else
                    {
                    //    grabbedObject.drawer.canDraw = false;
                        if (other.grabbedObject != null)
                        {
                          //  if (grabbedObject.drawer.lineDrawPrefab != null)
                        //    {
                         //       grabbedObject.drawer.lineDrawPrefab.transform.parent = other.grabbedObject.transform;

                         //       grabbedObject.drawer.drawPoints.Clear();
                        //    }
                        }
                    }
                }

      

                //   if(grabbedObject.snapToHand)
         //   {
                if (!snapped)
                {
                    grabTargetPosition =  grabbedObject.grabOffset - grabbedObject.pivotOffset*3;
                    grabTargetRotation = Quaternion.Euler(grabbedObject.grabAngles);
                    snapped = true;
                  /*  if (Vector3.Distance(grabbedObject.transform.localPosition, grabTargetPosition) > 1)
                    {
                        Vector3 step = controller.transform.position - grabTargetPosition;
                        step *= Time.deltaTime;
                        grabTargetPosition += step;
                    }
                    else
                    {
                        if (grabbedObject.snapToHand)
                        {
                            grabTargetPosition = controller.transform.position + grabbedObject.grabOffset;
                            grabTargetRotation = controller.transform.rotation * Quaternion.Euler(grabbedObject.grabAngles);
                        }
                    
                    }*/
             /*   }
            
        }
*/
    }

    private float grabTime;
    private Transform tempParent;
    private Vector3 pos, scale;
    private Quaternion rot;
    private void TryGrab()
    {
        InteractiveObject io = Focus.GetLast();
        if (io)
        {
            if (io.UpdateState(ItemState.Grabbed, true))
            {
                snapped = false;
                grabbedObject = io;
               // grabDiff =  transform.position - io.transform.position + io.pivotOffset;

    
              //  grabTargetPosition = grabDiff;
               /* 
                grabFromShow = ShowArea.instance.io == grabbedObject;
                if (grabFromShow)
                {
                    ShowArea.instance.Deactivate(true);
                }
                if (ShowArea.instance.io == grabbedObject)
                {
                    //; 
                }
                */
             
               ///io.UpdateCut(false);
                Debug.Log("grab me me");
                tempParent = grabbedObject.transform.parent;
                
                pos = grabbedObject.transform.localPosition;
                rot = grabbedObject.transform.localRotation;
                scale  = grabbedObject.transform.localScale;
                io.SetUnHovered();
                grabbedObject.transform.parent = controller.playerHand.transform;
                grabTargetPosition = grabbedObject.transform.localPosition-grabbedObject.pivotOffset;

            }
        }
    }

    public void TryRelease()
    {
        if (grabbedObject)
        {
            if (grabbedObject.UpdateState(ItemState.Grabbed, false))
            {
             /*   if (Vector3.Distance(grabbedObject.GetMidPoint(), ShowArea.instance.transform.position) <
                    ShowArea.distanceForDrop)
                {//drop in
                    if (ShowArea.instance.io == null)
                    {
                        ShowArea.instance.ActivateForObject(grabbedObject);
                        grabbedObject.UpdateCut(true);
                        Debug.Log("add me show");
                    }
                    else
                    {
           

                        ActionsManager.DoActionOutside(grabbedObject, ItemActions.Reset);
                        grabbedObject.UpdateCut(false);
                    }
                }
                else
                {//drop outside
                    Debug.Log("drop outside");
                    if (ShowArea.instance.io == grabbedObject)
                    {
                        ShowArea.instance.Deactivate(true);
            
                    }
                    grabbedObject.transform.parent = null;
                } */
                grabbedObject.transform.parent = null;
                grabbedObject = null;
            }
        }
    }

    private void GrabOn(XRController _grabbed,ItemEvents _event)
    {
        TryGrab();
        Debug.Log("Grab On "+gameObject.name + "from "+_grabbed.gameObject);
    }
    private void GrabOff(XRController _grabbed,ItemEvents _event)
    {
        TryRelease();
        Debug.Log("Grab ff "+gameObject.name + "from "+_grabbed.gameObject);
    }

    private void ChangeState(bool _grabbed)
    {
       // animator.SetBool("Grabbing",_grabbed); 
    }
}