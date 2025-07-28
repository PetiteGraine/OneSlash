using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Player position")]
    private int _positionX;
    private int _initialPosX;

    [Header("UI elements")]
    [SerializeField] private GameObject _canvasScoreNearPlayerText;
    private float __canvasScoreNearPlayerTextPosOffestX = 2.5f;
    private Coroutine _scoreTextCoroutine;
    [SerializeField] private Button[] _buttonsD;
    [SerializeField] private Button[] _buttonsF;
    [SerializeField] private Button[] _buttonsJ;
    [SerializeField] private Button _buttonK;

    [Header("Animation")]
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _deathAnimationDelay = 0.165f;
    [SerializeField] private Animator _animator;
    [SerializeField] private AnimationClip _idle;
    [SerializeField] private AnimationClip _dash;
    [SerializeField] private AnimationClip _slashA;
    [SerializeField] private AnimationClip _slashB;
    [SerializeField] private AnimationClip _death;

    [Header("Audio")]
    private AudioManager _audioManager;

    private void Start()
    {
        _spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _initialPosX = PlacementsVariable.Placements.Length / 2;
        _positionX = _initialPosX;
        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
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
            if (context.action.name == "Move1" || !GameController.ProModeEnabled)
                foreach (var _buttonD in _buttonsD)
                {
                    PressButton(_buttonD);
                }
            else
            {
                foreach (var _buttonF in _buttonsF)
                {
                    PressButton(_buttonF);
                }
            }

            if (GetScripts.GameControllerScript.IsGameOver) return;

            GetScripts.EnemiesControllerScript.RefreshEnemyList();
            GameObject oldestEnemy = GetScripts.EnemiesControllerScript.Enemies[0];

            if (oldestEnemy.transform.position.x > transform.position.x)
            {
                _positionX++;
            }
            else
            {
                _positionX--;
            }

            Vector3 newPosition = transform.position;
            newPosition.x = PlacementsVariable.Placements[_positionX].transform.position.x;
            _audioManager.PlaySFX(_audioManager.Dashs[Random.Range(0, _audioManager.Dashs.Count)]);

            if (Mathf.Approximately(newPosition.x, oldestEnemy.transform.position.x))
            {
                Die();
                return;
            }

            GetScripts.GameControllerScript.IncreaseScore(1);
            GetScripts.GameControllerScript.IncreaseSteps(1);
            transform.position = newPosition;
            _animator.Play(_dash.name);
            PlacementsVariable.ActivePlacement(_positionX, PlacementsVariable.GetIndexOfEnemyPostion(oldestEnemy));
        }

        else if (context.canceled)
        {
            if (context.action.name == "Move1" || !GameController.ProModeEnabled)
                foreach (var _buttonD in _buttonsD)
                {
                    ReleaseButton(_buttonD);
                }
            else
                foreach (var _buttonF in _buttonsF)
                {
                    ReleaseButton(_buttonF);
                }
        }
    }

    public void UpdateFlipPlayer(GameObject currentEnemy)
    {
        if (currentEnemy.transform.position.x <= transform.position.x == _spriteRenderer.flipX) return;
        StartCoroutine(WaitAndFlip(_slashA.length / 4f, currentEnemy));
    }

    private System.Collections.IEnumerator WaitAndFlip(float waitTime, GameObject currentEnemy)
    {
        yield return new WaitForSeconds(waitTime);

        bool shouldFlip = currentEnemy.transform.position.x <= transform.position.x;
        _spriteRenderer.flipX = shouldFlip;

        var child = transform.GetChild(0);
        Vector3 localPos = child.localPosition;
        localPos.x = shouldFlip ? -Mathf.Abs(_spriteRenderer.transform.localPosition.x) : Mathf.Abs(_spriteRenderer.transform.localPosition.x);
        _spriteRenderer.transform.localPosition = localPos;
    }


    private void Slash(InputAction.CallbackContext context, string validEnemyName, string slashAnimation)
    {
        if (!context.performed || GetScripts.GameControllerScript.IsGameOver) return;

        GetScripts.EnemiesControllerScript.RefreshEnemyList();

        GameObject oldestEnemy = GetScripts.EnemiesControllerScript.Enemies[0];
        EnemyAnimation enemyScript = oldestEnemy.GetComponent<EnemyAnimation>();
        int enemyIndexPos = PlacementsVariable.GetIndexOfEnemyPostion(oldestEnemy);

        if (!oldestEnemy.name.StartsWith(validEnemyName))
        {
            Die();
            return;
        }

        if (Mathf.Abs(enemyIndexPos - _positionX) == 1)
        {
            GetScripts.GameControllerScript.IncreaseScore(5);
            GetScripts.GameControllerScript.IncreaseEnemiesKilled(1);
            StartCoroutineShowScoreText();
            enemyScript.DeathAnimation();
            _animator.Play(slashAnimation);

            GameObject newEnemy = GetScripts.EnemiesControllerScript.SpawnEnemy(_positionX);
            GetScripts.ChangeArrowsDirectionScript.UpdateArrowDirection(transform.position.x < newEnemy.transform.position.x);
            PlacementsVariable.ActivePlacement(_positionX, PlacementsVariable.GetIndexOfEnemyPostion(newEnemy));
        }
        else
        {
            Die();
        }
    }

    public void InitArrowDirection(GameObject newEnemy)
    {
        GetScripts.ChangeArrowsDirectionScript.UpdateArrowDirection(transform.position.x < newEnemy.transform.position.x);
    }

    public void SlashA(InputAction.CallbackContext context)
    {
        Slash(context, "EnemyA", _slashA.name);
        if (context.performed)
        {
            foreach (var _buttonJ in _buttonsJ)
            {
                PressButton(_buttonJ);
            }
            _audioManager.PlaySFX(_audioManager.SlashA);
        }
        else if (context.canceled)
        {
            foreach (var _buttonJ in _buttonsJ)
            {
                ReleaseButton(_buttonJ);
            }
        }
    }

    public void SlashB(InputAction.CallbackContext context)
    {
        Slash(context, "EnemyB", _slashB.name);
        if (context.performed)
        {
            PressButton(_buttonK);
            _audioManager.PlaySFX(_audioManager.SlashB);
        }
        else if (context.canceled)
        {
            ReleaseButton(_buttonK);
        }
    }

    private void Die()
    {
        GameObject oldestEnemy = GetScripts.EnemiesControllerScript.Enemies[0];
        EnemyAnimation enemyScript = oldestEnemy.GetComponent<EnemyAnimation>();
        GetScripts.GameControllerScript.GameOver();

        enemyScript.AttackAnimation();
        _audioManager.PlaySFX(_audioManager.EnemyAttacks[Random.Range(0, _audioManager.EnemyAttacks.Count)]);
        StartCoroutine(PlayDeathAnimationWithDelay());

        System.Collections.IEnumerator PlayDeathAnimationWithDelay()
        {
            yield return new WaitForSeconds(_deathAnimationDelay);
            _animator.Play(_death.name);
            //_audioManager.PlaySFX(_audioManager.Death);
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
        GetScripts.GameControllerScript.UpdateScoreNearPlayer();
        UpdateScoreNearPlayerPos();
        var anim = _canvasScoreNearPlayerText.transform.GetChild(0).GetComponent<Animation>();
        anim.Stop();
        anim.Play();
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
