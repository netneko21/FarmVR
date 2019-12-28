using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ActionsIO;
using UnityEngine;
using System.Linq;


public class ActionsManager : Singleton<ActionsManager>
{
    public static Dictionary<ItemActions, Type> Actions = new Dictionary<ItemActions, Type>();
    void GetTypes()
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (var currentType in assembly.GetTypes().Where(_ => typeof(MonoBehaviour).IsAssignableFrom(_)))
            {
                var attributes = currentType.GetCustomAttributes(typeof(ComponentIdentifierAttribute), false);
                if (attributes.Length > 0)
                {
                    var targetAttribute = attributes.First() as ComponentIdentifierAttribute;
                    Actions.Add(targetAttribute.action, currentType);
                }
            }
        }
    }
    
    
    protected override void OnAwake()
    {
        GetTypes();
    }

    [SuppressMessage("ReSharper", "Unity.PerformanceCriticalCodeInvocation")]
    
    
    public static void DoActionOutside(InteractiveObject _source,ItemActions _type)
    {
        ItemEventAction action = new ItemEventAction().Init(_type,ActionMode.Run);
        DoAction(_source, action);
    }
    
    public static void DoAction(InteractiveObject _source, ItemEventAction _action)
    {
        if (_action.type == ItemActions.None)
        {
            return;
        }

        if (!_action.target)
        {
            _action.target=_source;
        }

        ActionIO existingComponent = null;
        var typeToLoad = Actions[_action.type];
        
        if (_action.actionComponent != null)
        {//try get component 
            existingComponent = _action.actionComponent;
        }
        else
        {//try find component
            existingComponent = (ActionIO)_action.target.gameObject.GetComponent(typeToLoad);
        }
        
        _action.target.gameObject.SetActive(true);
        switch (_action.mode)
        {
            case ActionMode.Run:
                if (existingComponent)
                {
                    existingComponent.mode = ActionMode.Run;
                    existingComponent.Run();
                }
                else
                {
                    ActionIO newComponent = (ActionIO)_action.target.gameObject.AddComponent(typeToLoad);
                    newComponent.Run();
                }
                break;
            
            case ActionMode.Stop:
                if (existingComponent)
                {
                    existingComponent.mode = ActionMode.Stop;
                    existingComponent.Stop();
                }
                break;
            
            case ActionMode.Remove:
                    Destroy(existingComponent);
                break;
        }
    }
}


[Serializable]
public class ItemEventAction
{
    public ItemEvents onEvent;
    public ItemActions type;
    public ActionMode mode;
    public bool requireFocus;
    public InteractiveObject target;
    public ActionIO actionComponent;

    public ItemEventAction Init(ItemActions _type,ActionMode _mode)
    {
        type = _type;
        mode = _mode;
        return this;
    }
}

public enum ActionMode
{
    Run = 0,
    Stop = 1,
    Remove = 2,
}

public enum ItemEvents
{
    None = 0,
    onEnable = 1,
    onFocusOn = 2,
    onFocusOff = 3,
    onActivation = 4,
    onGrab = 5,
    onGrabRelease = 6,
    onDeactivation = 7,
    onDisable = 8,
    onShow = 9,
    onHide = 10,
    onTriggerPressed = 11,
    onTriggerReleased = 12,
    onGripPressed = 13,
    onGripReleased = 14,
    onCustom = 15,
}

public enum ItemActions
{
    None = 0,
    Enable = 1,
    Disable = 2,
    Show = 3,
    Hide = 4,
    Activate = 5,
    Deactivate = 6,
    Rotate = 7,
    Move = 8,
    ClearAllActions = 9,
    Grow = 10,
    Scale = 11,
    Shrink = 12,
    GoDisable = 13,
    GoEnable = 14,
    GrabMe = 15,
    StartShow = 16,
    Reset = 17,
    EndShow = 18,
    OpenUI = 19,
    LoadScene = 20,
    UIButtonOver = 21,
    UIButtonOff = 22,
    UIButtonClick = 23,
    UIInputOver = 24,
    UIInputOff = 25,
    UIInputClick = 26,
    VRKeyOver = 27,
    VRKeyOff = 28,
    VRKeyClick = 29,
    HideActionsMenu = 30,
    TileDig = 31,
    TileAction =32 ,
    TileHarvest = 33,
    SeedsSelection = 34,
    TileClear = 35,
    ShowActionsMenu = 36,
    ShowInfoMenu = 37,

    HideInfoMenu = 38,
    SeedPlant = 39,
    MenuToggle = 40
 
}
