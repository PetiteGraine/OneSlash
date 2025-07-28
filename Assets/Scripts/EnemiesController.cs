using UnityEngine;
using UnityEngine.UI;

public class EnemiesController : MonoBehaviour
{
    [Header("Enemy prefabs")]
    [SerializeField] private GameObject _enemyAPrefab;
    [SerializeField] private GameObject _enemyBPrefab;

    [Header("UI elements")]
    [SerializeField] private GameObject _nextPosIcon;
    [SerializeField] private Image _nextPosIconImage;
    [SerializeField] private Color _nextPosColorA;
    [SerializeField] private Color _nextPosColorB;

    [Header("Enemy management")]
    public GameObject[] Enemies;
    private bool _isEnemyAIsNext;
    private int _centerIndex;
    private GameObject _player;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _centerIndex = PlacementsVariable.Placements.Length / 2;
    }

    public void RefreshEnemyList()
    {
        Enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    public GameObject FirstSpawnEnemy()
    {
        bool isEnemySpawnRight = Random.Range(0, 2) == 1;
        _isEnemyAIsNext = Random.Range(0, 2) == 1;
        PlacementsVariable.changeColor(_isEnemyAIsNext);
        Vector3 firstSpawnPos = PlacementsVariable.Placements[isEnemySpawnRight ? _centerIndex + 2 : _centerIndex - 2].transform.position;
        firstSpawnPos.y += 0.875f;
        GameObject currentEnemy = Instantiate(_isEnemyAIsNext ? _enemyAPrefab : _enemyBPrefab, firstSpawnPos, Quaternion.identity);
        _nextPosIcon.transform.position = new Vector3(PlacementsVariable.Placements[isEnemySpawnRight ? _centerIndex - 2 : _centerIndex + 2].transform.position.x, _nextPosIcon.transform.position.y, 0);
        UpdateNextIconImage();
        RefreshEnemyList();
        _player.GetComponent<Player>().UpdateFlipPlayer(currentEnemy);
        UpdateFlipEnemy(currentEnemy);
        _nextPosIcon.SetActive(true);
        PlacementsVariable.ActivePlacement(PlacementsVariable.Placements.Length / 2, PlacementsVariable.GetIndexOfEnemyPostion(currentEnemy));
        return currentEnemy;
    }

    public GameObject SpawnEnemy(int playerPos)
    {
        PlacementsVariable.changeColor(_isEnemyAIsNext);
        Vector3 enemyPos = _nextPosIcon.transform.position;
        enemyPos.y -= _nextPosIcon.transform.position.y - 0.75f;
        GameObject currentEnemy = Instantiate(_isEnemyAIsNext ? _enemyAPrefab : _enemyBPrefab, enemyPos, Quaternion.identity);
        int currentEnemyIndex = PlacementsVariable.GetIndexOfEnemyPostion(currentEnemy);
        UpdateNextEnemyPos(playerPos, currentEnemyIndex);
        UpdateNextIconImage();
        UpdateFlipEnemy(currentEnemy);
        _player.GetComponent<Player>().UpdateFlipPlayer(currentEnemy);
        return currentEnemy;
    }

    private void UpdateNextEnemyPos(int excludedIndex, int currentEnemyIndex)
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

    private void UpdateNextIconImage()
    {
        float alpha = _nextPosIconImage.color.a;
        _isEnemyAIsNext = Random.Range(0, 2) == 1;
        Color nextColor = _isEnemyAIsNext
            ? _nextPosColorA
            : _nextPosColorB;
        nextColor.a = alpha;
        _nextPosIconImage.color = nextColor;
    }

    private void UpdateFlipEnemy(GameObject currentEnemy)
    {
        if (currentEnemy.transform.position.x > _player.transform.position.x)
        {
            currentEnemy.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = false;
            currentEnemy.transform.GetChild(0).localPosition = new Vector3(_enemyAPrefab.transform.GetChild(0).localPosition.x, currentEnemy.transform.GetChild(0).localPosition.y, currentEnemy.transform.GetChild(0).localPosition.z);
        }
        else
        {
            currentEnemy.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
            currentEnemy.transform.GetChild(0).localPosition = new Vector3(-_enemyAPrefab.transform.GetChild(0).localPosition.x, currentEnemy.transform.GetChild(0).localPosition.y, currentEnemy.transform.GetChild(0).localPosition.z);
        }
    }
}
