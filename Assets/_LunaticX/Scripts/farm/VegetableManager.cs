
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum VegType
{
    empty = 0,
    pepper = 1,
    broccoli = 2,
    cabbage = 3,
    carrot = 4,
    radish = 5
}

public class VegetableManager : Singleton<VegetableManager>
{
    public Dictionary<VegType, VegetableData> vegetableList = new Dictionary<VegType, VegetableData>();
    public List<VegetableData> vegetables;
    private void Awake()
    {
        foreach (var vegetable in vegetables)
        {
            vegetableList.Add(vegetable.type, vegetable);
        }
    }
    
    public VegetableData Get(VegType type)
    {
        return vegetableList[type];
    }
}
