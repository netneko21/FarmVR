
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Vegetable", menuName = "Vegetable Data", order = 51)]
public class VegetableData : ScriptableObject
{
    [SerializeField]
    public VegType vegType;
    [SerializeField]
    public GameObject early,mid,late,objectCurrent;
    [SerializeField]
    public Tile tileRef;
    
    [SerializeField]
    public Sprite icon;
    [SerializeField]public string name;
    [SerializeField]   private bool canGrow;
    [SerializeField] private bool canCollect;
    [SerializeField] public Sprite iconSprite;
    [SerializeField] public Image smile, needWater;
    [SerializeField] public float growthTimer, growthTime;
    
    public Sprite IconSprite
    {
        get
        {
            return iconSprite;
        }
    }

}

