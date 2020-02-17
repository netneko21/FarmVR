using System.Collections.Generic;
using UnityEngine;
using ActionsIO;

public class InteractiveObject : MonoBehaviour
{
    #region Properties
    
    [SerializeField, EnumFlag]public ItemState state;
    [SerializeField, EnumFlag]public ActivationTriggers activationTriggers;
    [SerializeField, EnumFlag]public ItemProperties itemProperties;
    [SerializeField, EnumFlag]public FocusTriggers focusTriggers;
    
    public List<ItemEventAction>  actions = new List<ItemEventAction>();

    List<ActionIO> activeActions = new List<ActionIO>();

    public List<Focus> currentColliders = new List<Focus>();
    private Vector3 grabOffset,grabScale,grabAngles;
    private bool canGrab,snapToHand,canDraw;
//    public Draw drawer;
    private Vector3 basePos,baseLocalPos,baseScale;
    private Vector3 baseRot,baseLocalRot;
    public Transform baseParent;
    public bool isActive { get; set; }

    #endregion
    
    private void SaveStats()
    {
        basePos = transform.position;
        baseLocalPos = transform.localPosition;
        baseScale = transform.localScale;
        baseRot = transform.eulerAngles;
        baseLocalRot = transform.localEulerAngles;
        baseParent = transform.parent;
    }

    public void ResetIO()
    {
        transform.parent = baseParent;
        transform.localPosition = baseLocalPos;
        transform.localScale = baseScale;
        transform.eulerAngles = baseRot;
        Debug.Log("reset");
    }

    public Vector3 GetMidPoint()
    {
        if (col)
        {
            Bounds combinedBounds = col.bounds;
            foreach (Collider cc in colliders)
            {
                if(cc!=col)
                {
                    combinedBounds.Encapsulate(cc.bounds);
                }
            }
            return combinedBounds.center;
        }
        return  Vector3.zero;
    }
      
    public void UpdateCut(bool _needToCut)
    {
        if (rend)
        {
            rend.material.SetInt("_NeedToCut", _needToCut ? 1 : 0);
        }

        foreach (Renderer r in GetComponentsInChildren<Renderer>(true))
        {
            r.material.SetInt("_NeedToCut", _needToCut ? 1 : 0);
        }
    }

   public Vector3 pivotOffset;

    public List<Renderer> renderers = new List<Renderer>();
    private Renderer rend;
    
    public List<Collider> colliders = new List<Collider>();
    private Collider col;
    private void Start()
    {
        SaveStats();
        
        rend = GetComponent<Renderer>();
        col = GetComponent<Collider>();
        if (!colliders.Contains(col)&&col){colliders.Add(col);}
        if (!renderers.Contains(rend)&&rend){renderers.Add(rend);}
        
        foreach (Transform t in GetComponentsInChildren<Transform>(true))
        {
            if (t.parent == transform)
            {
                Collider c = t.GetComponent<Collider>();
                if (c)
                {
                    if (!t.GetComponent<ChildCollider>())
                    {
                        t.gameObject.AddComponent<ChildCollider>();
                        if (!colliders.Contains(c))
                        {
                            colliders.Add(c);
                        }
                    }
                }
                
                Renderer r = t.GetComponent<Renderer>();
                if (r)
                {
                    if (!renderers.Contains(r))
                    {
                        renderers.Add(r);
                    }
                }
            }
        }
        
        if(!col&&colliders.Count>0) col = colliders[0];
        if(!rend&&renderers.Count>0)rend = renderers[0];
        UpdateCut(false);
        pivotOffset = GetMidPoint()-transform.position;
    }

    #region Actions
    public void AddAction(ActionIO action)
    {
        //Debug.Log("adding action on "+gameObject.name + action);
        if (!activeActions.Contains(action))
        {
            activeActions.Add(action);
        }
    }
    
    public void ClearActions()
    {
        foreach (ActionIO action in activeActions)
        {
            action.Remove();
        }
        
        activeActions.Clear();
    }
    
    public void RemoveAction(ActionIO _action)
    {
        if (activeActions.Contains(_action))
        {
            activeActions.Remove(_action);
        }
    }
    
    public void Disable() { isActive = false; }
    
    public void Enable() { isActive = true; }
    
    public void DisableColliders()
    {
        foreach (Collider collider in colliders) { collider.enabled = false; }
    }
    
    public void EnableColliders()
    {
         foreach (Collider collider in colliders) { collider.enabled = true; }
    }

    public void FireTrigger(ActivationTriggers _trigger)
    {
        if (!activationTriggers.Has(_trigger))
        {
            FireEvent(ItemEvents.onActivation);
        }
    }

    public void FireEvent(ItemEvents _event)
    {//event triggers
//        Debug.Log("FireEvent "+_event);
        foreach (ItemEventAction action in actions)
        {
            bool allowAction = true; 
            if (action.requireFocus)
            {
             /*   if (lastFocused.Contains(this))
                {
                    if (lastFocused[lastFocused.Count-1] == this)
                    {
                        
                    }
                    else
                    {
                        allowAction = false;
                    }
                }
                else
                {
                    allowAction = false;
                }*/
            }
            
            
            
            if (action.onEvent == _event && allowAction)
            {
                ActionsManager.DoAction(this,action);
            }
        }
    }
    
    #endregion

    #region States
                  
                      
                      public bool UpdateState(ItemState _state,bool _enable)
                      {
                          bool stateChanged = false;
                          if (_enable)
                          {
                              if (!state.Has(_state))
                              {
                                  state.Add(_state);
                                  stateChanged = true;
                              }
                          }
                          else
                          {
                              if (state.Has(_state))
                              {
                                  state.Remove(_state);
                                  stateChanged = true;
                              }
                          }
                           
                          if (stateChanged)
                          {
                              if(_enable)
                              {
                                  if (_state == ItemState.Focused)
                                  {
                                      FireEvent(ItemEvents.onFocusOn);
                                  }
                                   
                                  if (_state == ItemState.Activated)
                                  {
                                      FireEvent(ItemEvents.onActivation);
                                  }
                                  
                                  if (_state == ItemState.Grabbed)
                                  {
                                      FireEvent(ItemEvents.onGrab);
                                  }
                              }
                              else
                              {
                                  if (_state == ItemState.Focused)
                                  {
                                      FireEvent(ItemEvents.onFocusOff);
                                  }
                                   
                                  if (_state == ItemState.Activated)
                                  {
                                      FireEvent(ItemEvents.onDeactivation);
                                  }
                                  
                                  if (_state == ItemState.Grabbed)
                                  {
                                      FireEvent(ItemEvents.onGrabRelease);
                                  }
                              }
                          }
                  
                          return stateChanged;
                      }
                  
                  
                      #endregion
                      
    #region Focus


        
        
  
    public void TryAddFocus(Focus _focus)
    {//add from colliders
        if(state.Has(ItemState.Grabbed)){return;}
        if (!currentColliders.Contains(_focus))
        {//first time focus activation
            currentColliders.Add(_focus);
            _focus.AddToOrderingList(this);
            SetHoveredLast(); //everything new is last
        }
    }
    public void SetHoveredLast()
    {
        SetLayer("hoveredLast");
        UpdateState(ItemState.Focused, true);
    }

    public void SetHovered()
    {
        if (UpdateState(ItemState.Focused, true))
        {
            Debug.Log("trying to SetHovered focus on unfocused new");
        }
        else
        {
            SetLayer("hovered");
        }
    }
    public void SetUnHovered()
    {
        if (UpdateState(ItemState.Focused, false))
        {
            SetLayer("interactive");
        }
        else
        {
            Debug.Log("trying to SetUnHovered focus on unfocused");
        }
    }
 
    public void TryRemoveFocus(Focus _focus)
    {
        if (currentColliders.Contains(_focus))
        {
            currentColliders.Remove(_focus);
            
            if(_focus.RemoveFromOrderingList(this))
            {
               SetUnHovered();
            }
        }
    }
    
    
    public void SetLayer(string layer)
    {
        foreach (Transform go in GetComponentsInChildren<Transform>(true))
        {
            go.gameObject.layer = LayerMask.NameToLayer(layer);
        }
    }
    #endregion
}
