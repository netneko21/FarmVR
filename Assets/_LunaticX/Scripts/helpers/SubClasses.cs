using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;




public enum ActivationTriggers
{
    None = 0,
    Touch = 1,
    GazeFocus = 2,
    GazeTimer = 4,
    Button  = 8,
    Grab = 16,
    Pointer = 32,
    External = 64,
    
}

public enum ItemProperties
{
    None = 0,
    SnapToHand = 2,
    Kinematic = 4,
    ReturnToBase = 8,
}

public enum FocusTriggers
{
    None = 0,
    Gaze = 1,
    Touch = 2,
    Pointer = 4,
    External = 8,
}

public enum ItemState
{
    None = 0,
    Focused = 1,
    Activated = 2,
    Grabbed = 4,
    Hidden = 8,
    Busy = 16,
    Disabled = 32
}
public enum HandSide
{
    Left = 0,
    Right = 1,
    Dual = 2,
    Any = 3
}

public enum HandButtons
{
    Trigger = 0,
    Grip = 1,
}

public static class FlagsExtensions
{
    public static bool Is<T>(this T _source, T _value)
    {
        return ((int)(object)_value & (int)(object)_source) == (int)(object)_source;
    }

    public static bool Has<T>(this T _source, T _value)
    {
        return ((int)(object)_value & (int)(object)_source) == (int)(object)_value;
    }
    
    public static void Add<Enum>(this ref Enum _to, Enum _add) where Enum : struct
    {
        _to = (Enum) (object) ((int) (object) _to | (int) (object) _add);
    }
    
    public static void AddTo<Enum>(this Enum _add,ref Enum _to)
    {
        _to = (Enum) (object) ((int) (object) _to | (int) (object) _add);
    }

    public static void RemoveFrom<T>(this T _remove, ref T _from)
    {
        int mask = ~(int) (object) _remove;
        int newValue = (int) (object) _from & mask;
        _from = (T) (object) newValue;
    }
    
    public static void Remove<T>(this ref T _from, T _remove) where T : struct
    {
        int mask = ~(int) (object) _remove;
        int newValue = (int) (object) _from & mask;
        _from = (T) (object) newValue;
    }
}

[Serializable]
public class LinkedTransform
{
    public XRNode node;
    public Transform transform;
}

public class MathX
{
    public static Vector3 AngleLerp(Vector3 _startAngle, Vector3 _finishAngle, float _t)
    {
        float xLerp = Mathf.LerpAngle(_startAngle.x, _finishAngle.x, _t);
        float yLerp = Mathf.LerpAngle(_startAngle.y, _finishAngle.y, _t);
        float zLerp = Mathf.LerpAngle(_startAngle.z, _finishAngle.z, _t);
        Vector3 Lerped = new Vector3(xLerp, yLerp, zLerp);
        return Lerped;
    }
}
