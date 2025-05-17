using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Player Position")]
    private int _positionX;
    private int _initialPosX;

    [Header("Game Controllers")]
    private GameController _gameControllerScript;
    private EnemiesController _enemiesController;

    private void Start()
    {
        _initialPosX = PlacementsVariable.Placements.Length / 2;
        _positionX = _initialPosX;
        Debug.Log("Player Position: " + _positionX);
        _gameControllerScript = FindFirstObjectByType<GameController>();
        _enemiesController = FindFirstObjectByType<EnemiesController>();
    }

    public void ResetPlayerPosition()
    {
        _positionX = _initialPosX;
        Vector3 newPos = PlacementsVariable.Placements[_positionX].transform.position;
        newPos.y += 0.625f;
        transform.position = newPos;
    }

    public void MovePlayer(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (_gameControllerScript.IsGameOver) return;

            _enemiesController.RefreshEnemyList();
            GameObject oldestEnemy = _enemiesController.Enemies[0];

            if (oldestEnemy.transform.position.x > transform.position.x) _positionX++;
            else _positionX--;

            Vector3 newPosition = transform.position;
            newPosition.x = PlacementsVariable.Placements[_positionX].transform.position.x;

            if (newPosition.x == oldestEnemy.transform.position.x)
            {
                _gameControllerScript.GameOver();
                return;
            }

            transform.position = newPosition;
        }
    }

    public void Slash(InputAction.CallbackContext context, string validEnemyName)
    {
        if (!context.performed || _gameControllerScript.IsGameOver) return;

        _enemiesController.RefreshEnemyList();

        var oldestEnemy = _enemiesController.Enemies[0];
        var enemyIndexPos = PlacementsVariable.GetIndexOfEnemyPostion(oldestEnemy);

        if (!oldestEnemy.name.StartsWith(validEnemyName))
        {
            _gameControllerScript.GameOver();
            return;
        }

        if (Mathf.Abs(enemyIndexPos - _positionX) == 1)
        {
            Destroy(oldestEnemy);
            _gameControllerScript.IncrementScore();
            _enemiesController.SpawnEnemy(_positionX);
        }
        else
        {
            _gameControllerScript.GameOver();
        }
    }

    public void SlashA(InputAction.CallbackContext context)
    {
        Slash(context, "EnemyA");
    }

    public void SlashB(InputAction.CallbackContext context)
    {
        Slash(context, "EnemyB");
    }
}
