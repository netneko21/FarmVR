using UnityEngine;
public class ShowArea : MonoBehaviour
{
    public bool isActive;
    public InteractiveObject io;
    public bool objectInPlace = true;
    public ShowAreaProperties properties;
    //public int paintType = free, stickToRaycast, stickToPlane; Got PlayFabID: BF0840FB73899CDC
   
    public Transform parent,pivot;
    public static ShowArea instance ;
    private RotateAround rotator;
    private MoveInBorders mover;
    private HoloZoomController scaler;
    public Renderer borders;
    public static float distanceForDrop;
    public float toPlaceTime = 5;
    public void Start()
    {
        distanceForDrop = 0.7f;
        instance = this;
        rotator = GetComponent<RotateAround>();
        mover = GetComponent<MoveInBorders>();
        scaler = GetComponent<HoloZoomController>();
    }

    public static Color currentColor,targetColor;
    public Transform marker;
    public static void UpdateBorders(InteractiveObject _io)
    {

      //  Vector3 extent = new Vector3(offset.x / bounds.extents.x, offset.y / bounds.extents.y, offset.z / bounds.extents.z);

      Vector3 midpoint = _io.GetMidPoint();
      instance.marker.position = midpoint;//midpoint;
        if (Vector3.Distance( midpoint,instance.transform.position)<distanceForDrop)
        {
            targetColor = new Color(0, 1, 0, 0.2f);
        }
        else
        {
            targetColor = new Color(1f, 0.53f, 0.24f, 0.2f);
        }

        currentColor = instance.borders.material.GetColor("_BaseColor");
        currentColor = Color.Lerp(currentColor, targetColor, 0.1f);


        instance.borders.material.SetColor("_BaseColor", currentColor);
    }

    public void ActivateForObject(InteractiveObject _interactiveObject)
    {
        io = _interactiveObject;
        isActive = true;
        io.transform.parent = parent;

        properties = io.GetComponent<ShowAreaProperties>();
        if (properties == null)
        {
            properties = io.gameObject.AddComponent<ShowAreaProperties>();
            properties.SetDefaults(io);
        }
        else
        {
            if (properties.runtimeAdded)
            {
                properties.UpdateValues(io);
            }
        }

        mover.Reset();
        
        if (properties.deactivateIO)
        {
            ActionsManager.DoActionOutside(io,ItemActions.Disable);
        }
        
        objectInPlace = false;
        toPlaceTime = 0.1f;
    }
    
  
    

    public void Deactivate(bool _grabbed = false)
    {
        if(io)
        {
            if (!_grabbed)
            {
                ActionsManager.DoActionOutside(io, ItemActions.Reset);
            }
            else
            {
                
            }
            DisableControls();
            if (properties.deactivateIO)
            {
                //      io.enabled = true;}
                //   ItemEventAction action = new ItemEventAction();
                // action.type = ItemActions.Reset;
                // action.mode = ActionMode.Run;
                // action.target = io;
                // io.FireEvent(ItemEvents.onCustom);
                ActionsManager.DoActionOutside(io, ItemActions.Enable);
                // ActionsManager.DoAction(io,action);
            }
            

            io = null;
        }
    }

    public void Reset()
    {
        
    }
  
    private void Update()
    {
    
        if(io)
        {
            if(Input.GetKeyUp(KeyCode.Space)){Deactivate();}
            if (!objectInPlace)
            {
                if (toPlaceTime < 1)
                {
                    toPlaceTime += Time.deltaTime;
                    io.transform.position = Vector3.Lerp(io.transform.position,properties.showPos,0.1f);
                    io.transform.localScale = Vector3.Lerp(io.transform.localScale,properties.startScale,0.1f);
                    io.transform.localEulerAngles = MathX.AngleLerp(io.transform.localEulerAngles,properties.startRotation,0.1f);
                }
                else
                {
                    io.transform.position = properties.showPos;
                    io.transform.localScale = properties.startScale;
                    io.transform.localEulerAngles = properties.startRotation;
                    toPlaceTime = 0;
                    objectInPlace = true;
                    EnableControls();
                }
            }
            else
            {
            
            }
        }
    }

    void EnableControls()
    {
    
        
        if (properties.rotationEnabled)
        {
            rotator.enabled = true;
            rotator.Init(properties);
        }
        else
        {
            rotator.enabled = false;
        }
        
        if (properties.movementEnabled)
        {
            mover.enabled = true;
            mover.Init(properties);
        }
        else
        {
            mover.enabled = false;
        }
        
        if (properties.scalingEnabled)
        {
            scaler.enabled = true;
            scaler.Init(properties);
        }
        else
        {
            scaler.enabled = false;
        }
    }
    
    void DisableControls()
    {
        rotator.enabled = false;
        mover.enabled = false;
        scaler.enabled = false;
    }
}