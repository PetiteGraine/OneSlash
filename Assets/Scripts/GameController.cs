using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private int _score;
    [SerializeField] private TextMeshProUGUI _scoreText;
    public bool IsGameOver;
    private Vector3 _initialPosition;
    private EnemiesController _enemiesController;

    private void Start()
    {
        _score = 0;
        _enemiesController = FindFirstObjectByType<EnemiesController>();
        _initialPosition = PlacementsVariable.Placements[PlacementsVariable.Placements.Length / 2].transform.position;
        _initialPosition.y = GameObject.FindGameObjectWithTag("Player").transform.position.y;
        IsGameOver = false;
    }

    public void GameOver()
    {
        if (IsGameOver)
        {
            Time.timeScale = 0;
        }
        IsGameOver = true;
        Debug.Log("Game Over");
    }

    public void UpdateScore()
    {
        _score++;
        _scoreText.text = "Score : " + _score.ToString();
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        IsGameOver = false;
        _scoreText.text = "Score : 0";
        _enemiesController.Enemies = null;
        Player player = FindFirstObjectByType<Player>();
        player.transform.position = _initialPosition;
    }
}
