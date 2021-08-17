using Assets.Scripts.Interfaces;
using UnityEngine;

public class Shaker : MonoBehaviour, IShakeable
{
    private Transform _mainCamera;

    private float _shakeAmount = 0.1f;
    private float _shakeDuration = 1f;
    private float _decreaseFactor = 1.5f;

    private Vector3 _originPosition;

    private void Start()
    {
        _mainCamera = GetComponent<Transform>();
        _originPosition = _mainCamera.position;
    }

    private void Update()
    {
        Shake();
    }

    public void Shake()
    {
        if (_shakeDuration > 0)
        {
            _mainCamera.localPosition += Random.insideUnitSphere * _shakeAmount;
            _shakeDuration -= _decreaseFactor;
        }
        else { _shakeDuration = 0f; }
    }


}
