using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    

    [Header("UI Elements")]
    private int _score;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _scoreNearPlayerText;
    private int _highscore;
    [SerializeField] private TextMeshProUGUI _highscoreText;
    [SerializeField] private TextMeshProUGUI _pressToStartText;
    private int _deathCount = 0;
    [SerializeField] private TextMeshProUGUI _deathCountText;

    [Header("Game State")]
    public bool IsGameOver;
    private GameObject _gameplayController;
    private GameObject _player;
    private float _gameOverTime = -1f;
    private float _restartDelay = 0.2f;


    private void Start()
    {
        _score = 0;
        _highscore = 0;
        _gameplayController = GameObject.FindGameObjectWithTag("GameController");
        _player = GameObject.FindGameObjectWithTag("Player");
        _player.GetComponent<Player>().ResetPlayerPosition();
        IsGameOver = true;
    }

    public void GameOver()
    {
        IsGameOver = true;
        _pressToStartText.gameObject.SetActive(true);
        _gameOverTime = Time.unscaledTime;
        _gameplayController.GetComponent<Countdown>().StopTimer();
        _deathCount++;
        _deathCountText.text = _deathCount.ToString("D4");
    }

    private void DestroyAllEnemies()
    {
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(enemy);
        }
    }

    public void StartGame(InputAction.CallbackContext context)
    {
        if (context.performed && IsGameOver)
        {
            if (Time.unscaledTime - _gameOverTime < _restartDelay)
                return;

            _score = 0;
            UpdateScore();
            _player.GetComponent<Player>().ResetPlayerPosition();
            _pressToStartText.gameObject.SetActive(false);
            DestroyAllEnemies();
            _gameplayController.GetComponent<Countdown>().BeginTimer();
            _gameplayController.GetComponent<EnemiesController>().FirstSpawnEnemy();
            IsGameOver = false;
        }
    }

    public void IncreaseScore(int score)
    {
        _score += score;
        UpdateScore();
    }

    public void UpdateScore()
    {
        _scoreText.text = "Score : " + _score.ToString();
        if (_score > _highscore)
        {
            _highscore = _score;
            _highscoreText.text = "Highscore : " + _highscore.ToString();
        }
    }
    
    public void UpdateScoreNearPlayer()
    {
        _scoreNearPlayerText.text = _score.ToString();
    }
}
