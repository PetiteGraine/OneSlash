using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Player Position")]
    private int _positionX;
    private int _initialPosX;
    private float _spriteOffsetX;

    [Header("Game Controllers")]
    private GameController _gameControllerScript;
    private EnemiesController _enemiesController;
    [SerializeField] private Button _buttonD;
    [SerializeField] private Button _buttonF;


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
        _spriteOffsetX = transform.GetChild(0).localPosition.x;
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
        if (_gameControllerScript.IsGameOver) return;
        if (context.performed)
        {
            Debug.Log("MovePlayer: " + context.action.name);
            _enemiesController.RefreshEnemyList();
            GameObject oldestEnemy = _enemiesController.Enemies[0];

            if (oldestEnemy.transform.position.x > transform.position.x) _positionX++;
            else _positionX--;

            Vector3 newPosition = transform.position;
            newPosition.x = PlacementsVariable.Placements[_positionX].transform.position.x;

            if (context.action.name == "Move1")
                PressButton(_buttonD);
            else
            {
                PressButton(_buttonF);
            }

            if (Mathf.Approximately(newPosition.x, oldestEnemy.transform.position.x))
            {
                _gameControllerScript.GameOver();
                _animator.Play(_death.name);
                return;
            }

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
        localPos.x = _spriteRenderer.flipX ? -_spriteOffsetX : _spriteOffsetX;
        child.localPosition = localPos;
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
            Destroy(oldestEnemy);
            _gameControllerScript.IncrementScore();
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
    }

    public void SlashB(InputAction.CallbackContext context)
    {
        Slash(context, "EnemyB", _slashB.name);
    }

    private void PressButton(Button button)
    {
        ExecuteEvents.Execute(button.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerDownHandler);
    }

    private void ReleaseButton(Button button)
    {
        ExecuteEvents.Execute(button.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerUpHandler);
        button.onClick.Invoke();
    }
}
