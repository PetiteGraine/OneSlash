using UnityEngine;

public class ChangeArrowsDirection : MonoBehaviour
{
    public bool IsRight;
    public GameObject Arrow1;
    public GameObject Arrow2;

    public void UpdateArrowDirection(bool IsRight)
    {
        if (IsRight == this.IsRight)
            return;
        float yRotation = IsRight ? 0f : 180f;
        if (Arrow1 != null)
            Arrow1.transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
        if (Arrow2 != null)
            Arrow2.transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
        this.IsRight = IsRight;
    }
}
