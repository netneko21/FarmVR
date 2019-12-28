
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
public class VegetableManager : MonoBehaviour
{
    [SerializeField]
    public Vegetable[] vegetables;
    
    [System.Serializable]
    public class Vegetable : MonoBehaviour
    {
        public VegType vegType;
        public GameObject early,mid,late;
        public GameObject objectCurrent;
        public Tile tileRef;
        public Sprite icon;
        public string name;
        private bool canGrow;
        private bool canCollect;
        public Sprite iconSprite;
        public Image smile, needWater;
        public float growthTimer, growthTime;
    }

}
