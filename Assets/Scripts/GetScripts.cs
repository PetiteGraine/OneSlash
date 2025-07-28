using UnityEngine;

public class GetScripts : MonoBehaviour
{
    public static GameController GameControllerScript;
    public static Player PlayerScript;
    public static EnemiesController EnemiesControllerScript;
    public static ChangeArrowsDirection ChangeArrowsDirectionScript;

    private void Awake()
    {
        GameControllerScript = FindFirstObjectByType<GameController>();
        PlayerScript = FindFirstObjectByType<Player>();
        EnemiesControllerScript = FindFirstObjectByType<EnemiesController>();
        ChangeArrowsDirectionScript = FindFirstObjectByType<ChangeArrowsDirection>();
    }
}
