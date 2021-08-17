using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using Assets.Scripts.Interfaces;

public class ToggleSwitch : MonoBehaviour, IPointerDownHandler, IController
{
    [SerializeField] private MonoBehaviour _controllerObject;

    [SerializeField] private ButtonState[] _relatives;

    [SerializeField] private RectTransform _toggleIndicator;

    [SerializeField] private GameLogistics _game;

    [SerializeField] private Color _onColor;
    [SerializeField] private Color _offColor;

    [SerializeField] private Image _backgroundImage;

    private float _onX;
    private float _offX;
    private float _tweenTime = 0.25f;

    private IController _controller;

    //private AudioSource _audioSource;

    public delegate void ValueChanged(bool value);
    public event ValueChanged valueChanged;

    private void Awake()
    {
        _controller = _controllerObject as IController;
    }

    private void Start()
    {
        _offX = _toggleIndicator.anchoredPosition.x;
        if (_game.IsStarted) _offX = _toggleIndicator.anchoredPosition.x - _toggleIndicator.rect.width / 2f;
        _onX = _backgroundImage.rectTransform.rect.width - _toggleIndicator.rect.width * 1.5f;

        SetState(_controller.IsEnable());
        
        //_audioSource = GetComponent<AudioSource>();
    }

    public void Switch()
    {
        // if (playSFX) { _audioSource.Play(); }
        if (valueChanged != null) { valueChanged(!_controller.IsEnable()); }

        SetState(!_controller.IsEnable());

        if (_game.IsStarted)
        {
            foreach (var element in _relatives)
            {
                element.Switch();
            }
        }

        _controller.Switch();
    }

    public void SetState(bool state)
    {
        ToggleColor(state);
        MoveIndicator(state);
    }

    private void SetRelativesState(bool targetState)
    {
        if (_game.IsStarted)
        {
            foreach (var element in _relatives)
                element.SetState(targetState);
        }
    }

    public bool IsEnable() => _controller.IsEnable();

    public void Disable() => SetState(false);

    public void Enable() => SetState(true);

    private void ToggleColor(bool value)
    {
        if (value) { _backgroundImage.DOColor(_onColor, _tweenTime).SetUpdate(true); }
        else { _backgroundImage.DOColor(_offColor, _tweenTime).SetUpdate(true); }
    }

    private void MoveIndicator(bool value)
    {
        if (value) { _toggleIndicator.DOAnchorPosX(_onX, _tweenTime).SetUpdate(true); }
        else { _toggleIndicator.DOAnchorPosX(_offX, _tweenTime).SetUpdate(true); }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Switch();
    }
}
