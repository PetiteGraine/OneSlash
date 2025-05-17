using UnityEngine;

public class PlacementsVariable : MonoBehaviour
{
    public static GameObject[] Placements;
    [SerializeField] private GameObject[] _placements;

    private void Awake()
    {
        Placements = _placements;
    }

    public static int GetIndexOfEnemyPostion(GameObject enemy)
    {
        for (int i = 0; i < Placements.Length; i++)
        {
            if (Placements[i].transform.position.x == enemy.transform.position.x)
            {
                return i;
            }
        }
        return -1;
    }
}
