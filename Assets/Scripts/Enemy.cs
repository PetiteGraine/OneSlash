using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private Animator _animator;
    [SerializeField] private AnimationClip _idle;
    [SerializeField] private AnimationClip _attack;
    [SerializeField] private AnimationClip _death;

    public void AttackAnimation()
    {
        _animator.Play(_attack.name);
    }

    public void DeathAnimation()
    {
        gameObject.tag = "Untagged";
        _animator.Play(_death.name);
        Destroy(gameObject, _death.length / 2f);
    }
}
