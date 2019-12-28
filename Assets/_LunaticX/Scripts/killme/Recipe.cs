using System;
using UnityEngine;

public enum IngredientUnit { Spoon, Cup, Bowl, Piece }



public class Recipe : MonoBehaviour
{
    public ItemEventAction2 potionResult;
    public ItemEventAction2[] potionIngredients;
}

[Serializable]
public class ItemEventAction2
{
    public ItemEvents onEvent;
    public ItemActions type;
    public ActionMode mode;
    public bool requireFocus;
    public InteractiveObject target;
    public int id;
}