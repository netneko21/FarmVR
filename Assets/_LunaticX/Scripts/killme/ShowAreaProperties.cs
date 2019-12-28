
using UnityEngine;
public class ShowAreaProperties : MonoBehaviour
{
    
 
    public InteractiveObject objectShowing;
    public bool rotationEnabled=true,movementEnabled=true,scalingEnabled=true;
    public Vector3 showPos, startScale;
    public Vector3 startRotation;
    public HandSide handR,handM,handS;
    public HandButtons buttonsR,buttonsM,buttonsS;
    public Vector3 axisAllowedR,axisAllowedM,axisAllowedS;
    public bool deactivateIO;
    public bool runtimeAdded = false;
    
    public void SetDefaults(InteractiveObject _io)
    {
        runtimeAdded = true;
        rotationEnabled = true;
        UpdateValues(_io);
    }
    
    public void UpdateValues(InteractiveObject _io)
    {
        if (runtimeAdded)
        {
            Vector3 pivotOffset = _io.GetMidPoint() - _io.transform.position;
            showPos = ShowArea.instance.pivot.position - pivotOffset;
            showPos.y = _io.transform.position.y;
            
            //   startPos = _io.transform.position;//new Vector3(0,_io.transform.position.y,0);
            startScale = _io.transform.localScale;
            startRotation = _io.transform.eulerAngles;
            handR = HandSide.Right;
            axisAllowedR = Vector3.up;
            buttonsR = HandButtons.Trigger;
                
            handM = HandSide.Left;
            axisAllowedM = Vector3.one;
            buttonsM = HandButtons.Trigger;
                        
            handS = HandSide.Dual;
            buttonsS = HandButtons.Grip;
            axisAllowedS = Vector3.one;
        }



    }
}