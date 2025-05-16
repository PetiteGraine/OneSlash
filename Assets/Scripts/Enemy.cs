using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int _spawnOrder;

    public int SpawnOrder
    {
        get { return _spawnOrder; }
        set { _spawnOrder = value; }
    }
}
