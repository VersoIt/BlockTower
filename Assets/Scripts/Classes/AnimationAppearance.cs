using UnityEngine;

public class AnimationAppearance : MonoBehaviour
{
    [SerializeField] private string _targetAnimation;

    private Animation _animation;

    private void Awake()
    {
        _animation = GetComponent<Animation>();
    }

    private void OnEnable()
    {
        _animation.Play();
    }
}
