using UnityEngine;

public class PlacementsVariable : MonoBehaviour
{
    public static GameObject[] Placements;
    [SerializeField] private GameObject[] _placements;
    private static bool _isPathActive = true;
    [SerializeField] private Color _setColorADark;
    [SerializeField] private Color _setColorBDark;
    [SerializeField] private Color _setColorAParticles;
    [SerializeField] private Color _setColorBParticles;
    private static Color _colorADark;
    private static Color _colorBDark;
    private static Color _colorAParticles;
    private static Color _colorBParticles;

    private void Awake()
    {
        Placements = _placements;
        _colorADark = _setColorADark;
        _colorBDark = _setColorBDark;
        _colorAParticles = _setColorAParticles;
        _colorBParticles = _setColorBParticles;
    }

    public static int GetIndexOfEnemyPostion(GameObject enemy)
    {
        for (int i = 0; i < Placements.Length; i++)
        {
            if (Placements[i].transform.position.x == enemy.transform.position.x)
            {
                return i;
            }
        }
        return -1;
    }

    public static void ActivePlacement(int playerPos, int enemyPos)
    {
        if (!_isPathActive) return;
        int startVFXIndex = playerPos + 1 * (enemyPos > playerPos ? 1 : -1);
        for (int i = 0; i < Placements.Length; i++)
        {
            bool shouldBeActive = (i >= Mathf.Min(startVFXIndex, enemyPos) && i <= Mathf.Max(startVFXIndex, enemyPos));
            Placements[i].SetActive(shouldBeActive);
            if (Placements[i].transform.childCount > 0)
            {
                Placements[i].transform.GetChild(0).gameObject.SetActive(shouldBeActive);
            }
        }
    }

    public static void changeColor(bool isEnemyA)
    {
        if (!_isPathActive) return;
        Color newColorDark = isEnemyA ? _colorADark : _colorBDark;
        Color newColorParticles = isEnemyA ? _colorAParticles : _colorBParticles;
        foreach (GameObject placement in Placements)
        {

            if (placement.transform.childCount > 0)
            {
                Transform child = placement.transform.GetChild(0);
                if (child.TryGetComponent(out SpriteRenderer childSpriteRenderer))
                {
                    Color originalColor = childSpriteRenderer.color;
                    childSpriteRenderer.color = new Color(newColorDark.r, newColorDark.g, newColorDark.b, originalColor.a);
                }

                if (child.TryGetComponent(out ParticleSystem particleSystem))
                {
                    var main = particleSystem.main;
                    Color originalColor = main.startColor.color;
                    main.startColor = new Color(newColorParticles.r, newColorParticles.g, newColorParticles.b, originalColor.a);
                }
            }
        }
    }

    public static void TogglePathActive()
    {
        _isPathActive = !_isPathActive;
        if (_isPathActive) return;
        foreach (GameObject placement in Placements)
        {
            placement.SetActive(false);
            if (placement.transform.childCount > 0)
            {
                placement.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }
}
