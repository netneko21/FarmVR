
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Vegetable", menuName = "Vegetable Data", order = 51)]
public class VegetableData : ScriptableObject
{
    [SerializeField]
    public VegType type;
    [SerializeField]
    public GameObject early,mid,late,objectCurrent;
    [SerializeField]public string name;
    [SerializeField]public Sprite iconSprite;
    [SerializeField]public float  growthTime=500,dryTime=100,dryDieTime=300,overGrowTime = 300;
    [SerializeField]public bool unlocked;

    public Vector3[] bushOffset; 
    public Vector3[] bushStartScale;
    public Sprite IconSprite => iconSprite;

    public void Unlock()
    {
        unlocked = true;
    }

}

