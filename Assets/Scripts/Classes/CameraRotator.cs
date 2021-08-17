using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 5f;
    public Transform CameraTransform { get; set; }

    // Start is called before the first frame update
    private void Start()
    {
        CameraTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    private void Update()
    {
        CameraTransform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
    }
}
