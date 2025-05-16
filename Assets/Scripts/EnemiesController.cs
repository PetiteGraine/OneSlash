using UnityEngine;

public class EnemiesController : MonoBehaviour
{
    public GameObject[] Enemies;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _nextPosIcon;

    private void Start()
    {
        int excludedIndex = PlacementsVariable.Placements.Length / 2;
        bool isEnemySpawnRight = Random.Range(0, 2) == 1;
        Vector3 firstSpawnPos = PlacementsVariable.Placements[isEnemySpawnRight ? excludedIndex + 2 : excludedIndex - 2].transform.position;
        firstSpawnPos.y += 0.625f;
        Instantiate(_enemyPrefab, firstSpawnPos, Quaternion.identity);
        _nextPosIcon.transform.position = new Vector3(PlacementsVariable.Placements[isEnemySpawnRight ? excludedIndex - 2 : excludedIndex + 2].transform.position.x, _nextPosIcon.transform.position.y, 0);
    }

    private void UpdateNextPosIcon(int excludedIndex, int currentEnemyIndex)
    {
        int[] availableIndices = new int[PlacementsVariable.Placements.Length - 1];
        for (int i = 0, j = 0; i < PlacementsVariable.Placements.Length - 1; i++)
        {
            if (i == excludedIndex) continue;
            if (currentEnemyIndex < excludedIndex && i == currentEnemyIndex + 1) continue;
            if (excludedIndex < currentEnemyIndex && i == currentEnemyIndex - 1) continue;
            availableIndices[j++] = i;
        }

        int randomIndex = availableIndices[Random.Range(0, availableIndices.Length)];
        Vector3 nextSpawnPosition = PlacementsVariable.Placements[randomIndex].transform.position;
        _nextPosIcon.transform.position = new Vector3(nextSpawnPosition.x, _nextPosIcon.transform.position.y, 0);
    }

    public void SpawnEnemy(int playerPos)
    {
        GameObject currentEnemy = Instantiate(_enemyPrefab, _nextPosIcon.transform.position, Quaternion.identity);
        int currentEnemyIndex = PlacementsVariable.GetIndexOfEnemyPostion(currentEnemy);
        UpdateNextPosIcon(playerPos, currentEnemyIndex);
    }

    public void RefreshEnemyList() {
        Enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }
}
