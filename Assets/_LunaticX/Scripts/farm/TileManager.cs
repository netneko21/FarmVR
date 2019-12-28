
using Boo.Lang;
using UnityEngine;


public class TileManager : Singleton<TileManager>
{
    private Tile _currentTile;
    private Tile _prevTile;
    public Tile currentTile
    {
        get { return _currentTile; }
        set
        {
           
            if(_currentTile != value)
            {
                _currentTile = value;

            }
            else
            {
           
            }
       
        }
    }
}
