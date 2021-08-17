using UnityEngine;

class CameraController : MonoBehaviour
{
    public const float CameraMoveSpeed = 3f;
    public Vector3 StartPosition { get; private set; }

    private Transform _transform;

    private void Start()
    {
        StartPosition = transform.position;
        _transform = GetComponent<Transform>();
    }
}