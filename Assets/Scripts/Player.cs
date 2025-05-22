using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Player Position")]
    private int _positionX;
    private int _initialPosX;


    [Header("Game Controllers")]
    private GameController _gameControllerScript;
    private EnemiesController _enemiesController;

    [Header("UI Elements")]
    [SerializeField] private GameObject _canvasScoreNearPlayerText;
    private float __canvasScoreNearPlayerTextPosOffestX = 1.5f;
    private Coroutine _scoreTextCoroutine;
    [SerializeField] private Button _buttonD;
    [SerializeField] private Button _buttonF;
    [SerializeField] private Button _buttonJ;
    [SerializeField] private Button _buttonK;


    [Header("Animation")]
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private Animator _animator;
    [SerializeField] private AnimationClip _idle;
    [SerializeField] private AnimationClip _dash;
    [SerializeField] private AnimationClip _slashA;
    [SerializeField] private AnimationClip _slashB;
    [SerializeField] private AnimationClip _death;

    private void Start()
    {
        _spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _initialPosX = PlacementsVariable.Placements.Length / 2;
        _positionX = _initialPosX;
        _gameControllerScript = FindFirstObjectByType<GameController>();
        _enemiesController = FindFirstObjectByType<EnemiesController>();
    }

    public void ResetPlayerPosition()
    {
        _positionX = _initialPosX;
        Vector3 newPos = PlacementsVariable.Placements[_positionX].transform.position;
        newPos.y += 0.625f;
        transform.position = newPos;
        _animator.Play(_idle.name);
    }

    public void MovePlayer(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (context.action.name == "Move1")
                PressButton(_buttonD);
            else
            {
                PressButton(_buttonF);
            }

            if (_gameControllerScript.IsGameOver) return;

            _enemiesController.RefreshEnemyList();
            GameObject oldestEnemy = _enemiesController.Enemies[0];

            if (oldestEnemy.transform.position.x > transform.position.x) _positionX++;
            else _positionX--;

            Vector3 newPosition = transform.position;
            newPosition.x = PlacementsVariable.Placements[_positionX].transform.position.x;

            if (Mathf.Approximately(newPosition.x, oldestEnemy.transform.position.x))
            {
                _gameControllerScript.GameOver();
                _animator.Play(_death.name);
                return;
            }

            _gameControllerScript.IncreaseScore(1);
            transform.position = newPosition;
            _animator.Play(_dash.name);
        }

        else if (context.canceled)
        {
            if (context.action.name == "Move1")
                ReleaseButton(_buttonD);
            else
                ReleaseButton(_buttonF);
        }
    }

    public void UpdateFlipPlayer(GameObject currentEnemy)
    {
        if (currentEnemy.transform.position.x > transform.position.x) _spriteRenderer.flipX = false;
        else _spriteRenderer.flipX = true;

        var child = transform.GetChild(0);
        Vector3 localPos = child.localPosition;
        localPos.x = _spriteRenderer.flipX ? -Mathf.Abs(_spriteRenderer.transform.localPosition.x) : Mathf.Abs(_spriteRenderer.transform.localPosition.x);
    }

    private void Slash(InputAction.CallbackContext context, string validEnemyName, string slashAnimation)
    {
        if (!context.performed || _gameControllerScript.IsGameOver) return;

        _enemiesController.RefreshEnemyList();

        GameObject oldestEnemy = _enemiesController.Enemies[0];
        int enemyIndexPos = PlacementsVariable.GetIndexOfEnemyPostion(oldestEnemy);

        if (!oldestEnemy.name.StartsWith(validEnemyName))
        {
            _gameControllerScript.GameOver();
            _animator.Play(_death.name);
            return;
        }

        if (Mathf.Abs(enemyIndexPos - _positionX) == 1)
        {
            _gameControllerScript.IncreaseScore(5);
            StartCoroutineShowScoreText();
            Destroy(oldestEnemy);
            _enemiesController.SpawnEnemy(_positionX);
            _animator.Play(slashAnimation);
        }
        else
        {
            _gameControllerScript.GameOver();
            _animator.Play(_death.name);
        }
    }

    public void SlashA(InputAction.CallbackContext context)
    {
        Slash(context, "EnemyA", _slashA.name);
        if (context.performed)
        {
            PressButton(_buttonJ);
        }
        else if (context.canceled)
        {
            ReleaseButton(_buttonJ);
        }
    }

    public void SlashB(InputAction.CallbackContext context)
    {
        Slash(context, "EnemyB", _slashB.name);
        if (context.performed)
        {
            PressButton(_buttonK);
        }
        else if (context.canceled)
        {
            ReleaseButton(_buttonK);
        }
    }

    private void UpdateScoreNearPlayerPos()
    {
        Vector3 scoreTextLocalPos = _canvasScoreNearPlayerText.transform.localPosition;
        scoreTextLocalPos.x = _spriteRenderer.flipX ? -__canvasScoreNearPlayerTextPosOffestX : __canvasScoreNearPlayerTextPosOffestX;
        scoreTextLocalPos.x += transform.localPosition.x;
        _canvasScoreNearPlayerText.transform.localPosition = scoreTextLocalPos;
    }

    private System.Collections.IEnumerator ShowScoreTextCoroutine()
    {
        _gameControllerScript.UpdateScoreNearPlayer();
        UpdateScoreNearPlayerPos();
        _canvasScoreNearPlayerText.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        _canvasScoreNearPlayerText.SetActive(false);
        _scoreTextCoroutine = null;
    }

    private void StartCoroutineShowScoreText()
    {
        if (_scoreTextCoroutine != null)
        {
            StopCoroutine(_scoreTextCoroutine);
        }
        _scoreTextCoroutine = StartCoroutine(ShowScoreTextCoroutine());
    }

    private void PressButton(Button button)
    {
        ExecuteEvents.Execute(button.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerDownHandler);
    }

    private void ReleaseButton(Button button)
    {
        ExecuteEvents.Execute(button.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerUpHandler);
    }
}
