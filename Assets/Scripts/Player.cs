using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    private int _positionX;
    private GameController _gameController;
    private EnemiesController _enemiesController;

    private void Start()
    {
        _positionX = 3;
        _gameController = FindFirstObjectByType<GameController>();
        _enemiesController = FindFirstObjectByType<EnemiesController>();
    }

    public void MovePlayer(InputAction.CallbackContext context) {
        if (context.performed)
        {
            if (_gameController.IsGameOver) return;
            
            _enemiesController.RefreshEnemyList();
            GameObject oldestEnemy = _enemiesController.Enemies[0];

                if (oldestEnemy.transform.position.x > transform.position.x) {
                    _positionX++;
                }
                else {
                    _positionX--;
                }
            
            Vector3 newPosition = transform.position;
            newPosition.x = PlacementsVariable.Placements[_positionX].transform.position.x;
            

            if (newPosition.x == oldestEnemy.transform.position.x) {
                _gameController.GameOver();
                return;
            }

            transform.position = newPosition;
        }
    }

    public void SlashEnemy(InputAction.CallbackContext context) {
        if (context.performed)
        {
            if (_gameController.IsGameOver) return;
            
            _enemiesController.RefreshEnemyList();
            var oldestEnemy = _enemiesController.Enemies[0];
            var enemyIndexPos = PlacementsVariable.GetIndexOfEnemyPostion(oldestEnemy);
            if (_positionX - 1 == enemyIndexPos || enemyIndexPos == _positionX + 1)
            {
                Destroy(oldestEnemy);
                _gameController.UpdateScore();
                _enemiesController.SpawnEnemy(_positionX);
            }
            else
            {
                _gameController.GameOver();
            }
        }
    }
}
