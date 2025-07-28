using UnityEngine;

public class ChangeArrowsDirection : MonoBehaviour
{
    [Header("Arrow buttons")]
    public GameObject[] Arrows1;
    public GameObject[] Arrows2;

    public void UpdateArrowDirection(bool isRight)
    {
        GameObject[][] arrowGroups = { Arrows1, Arrows2 };
        foreach (var group in arrowGroups)
        {
            if (group.Length >= 2)
            {
                group[0].SetActive(!isRight);
                group[1].SetActive(isRight);
            }
        }
    }
}
