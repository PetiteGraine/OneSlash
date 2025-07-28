using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    [Header("Top UI elements")]
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _highscoreText;
    [SerializeField] private TextMeshProUGUI _deathCountText;

    [Header("Score details UI elements")]
    [SerializeField] private GameObject _scoreDetails;
    [SerializeField] private TextMeshProUGUI _scoreInDetailsText;
    [SerializeField] private TextMeshProUGUI _stepsText;
    [SerializeField] private TextMeshProUGUI _enemiesKilledText;
    [SerializeField] private TextMeshProUGUI _timeSurvivedText;

    [Header("Other UI elements")]
    [SerializeField] private TextMeshProUGUI _scoreNearPlayerText;
    [SerializeField] private TextMeshProUGUI _pressToStartText;

    [Header("Game stats")]
    private int _score;
    private int _steps;
    private int _enemiesKilled;
    private int _highscore;
    private int _deathCount = 0;
    public bool IsGameOver;
    private bool _isGameStarted;
    public static bool ProModeEnabled = false;

    [Header("References")]
    private GameObject _player;

    [Header("Game over settings")]
    private float _gameOverTime = -1f;
    private float _restartDelay = 0.01f;

    private void Start()
    {
        _score = 0;
        _highscore = 0;
        _player = GameObject.FindGameObjectWithTag("Player");
        _player.GetComponent<Player>().ResetPlayerPosition();
        IsGameOver = true;
        _isGameStarted = false;
    }

    public void GameOver()
    {
        IsGameOver = true;
        _isGameStarted = false;
        _gameOverTime = Time.unscaledTime;
        GetScripts.GameControllerScript.GetComponent<Countdown>().StopTimer();
        _deathCount++;
        _deathCountText.text = _deathCount.ToString("D4");
        _scoreInDetailsText.text = "Score: " + _score.ToString("D4");
        _stepsText.text = "Steps: " + _steps.ToString("D4");
        _enemiesKilledText.text = "Slashs: " + _enemiesKilled.ToString("D4");
        _timeSurvivedText.text = "Time survived: " + GetScripts.GameControllerScript.GetComponent<Countdown>().GetRemainingTime().ToString("F2");
        _scoreDetails.gameObject.SetActive(true);
    }

    private void DestroyAllEnemies()
    {
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(enemy);
        }
    }

    public void ProcessGameInput(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (IsGameOver)
        {
            if (Time.unscaledTime - _gameOverTime < _restartDelay)
                return;

            ResetStats();
            UpdateScore();
            _player.GetComponent<Player>().ResetPlayerPosition();
            _pressToStartText.gameObject.SetActive(false);
            _scoreDetails.gameObject.SetActive(false);
            DestroyAllEnemies();
            GameObject newEnemy = GetScripts.GameControllerScript.GetComponent<EnemiesController>().FirstSpawnEnemy();
            GetScripts.GameControllerScript.GetComponent<Countdown>().ResetTimer();
            GetScripts.PlayerScript.InitArrowDirection(newEnemy);
            IsGameOver = false;
            _isGameStarted = false;
            return;
        }

        if (!IsGameOver && !_isGameStarted)
        {
            _isGameStarted = true;
            GetScripts.GameControllerScript.GetComponent<Countdown>().BeginTimer();
            return;
        }
    }


    public void IncreaseScore(int score)
    {
        _score += score;
        UpdateScore();
    }

    public void IncreaseSteps(int steps)
    {
        _steps += steps;
    }

    public void IncreaseEnemiesKilled(int count)
    {
        _enemiesKilled += count;
    }

    public void ResetStats()
    {
        _score = 0;
        _steps = 0;
        _enemiesKilled = 0;
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
