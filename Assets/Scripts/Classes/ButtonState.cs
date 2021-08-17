using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Interfaces;

class ButtonState : MonoBehaviour, IController
{
    [Header("Sprites")]
    [SerializeField] private Sprite _enableState;
    [SerializeField] private Sprite _disableState;

    [Header("IController objects")]
    [SerializeField] private MonoBehaviour _controllerObject;
    [SerializeField] private MonoBehaviour[] _relativeObjects;

    [Header("Game")]
    [SerializeField] private GameLogistics _game;

    private IController[] _relatives;
    private IController _controller;

    private bool _isEnable;

    public bool IsEnable() => _isEnable;

    private Image _image;

    private void Start()
    {
        _image = GetComponent<Image>();
        _controller = _controllerObject as IController;
        _relatives = new IController[_relativeObjects.Length];

        for (int index = 0; index < _relativeObjects.Length; ++index)
            _relatives[index] = (IController) _relativeObjects[index];

        SetState(_controller.IsEnable());
    }

    public void Enable()
    {
        _image.sprite = _enableState;
        _isEnable = true;

        SetRelativesState(_isEnable);
    }

    public void Disable()
    {
        _image.sprite = _disableState;
        _isEnable = false;

        SetRelativesState(_isEnable);
    }

    public void Switch()
    {
        if (_isEnable) { Disable(); }
        else { Enable(); }
    }

    public void SetState(bool targetState)
    {
        if (targetState) Enable();
        else Disable();

        SetRelativesState(targetState);
    }

    private void SetRelativesState(bool targetState)
    {
        foreach (var element in _relatives)
            element.SetState(targetState);
    }

}