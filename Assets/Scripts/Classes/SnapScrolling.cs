using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Data;
using TMPro;

public class SnapScrolling : MonoBehaviour
{

    [Header("Controllers")]

    [Range(0, 500)]
    [SerializeField] private int _panelOffset;

    [Range(0, 100)]
    [SerializeField] private int _snapSpeed;

    [Range(0, 10)]
    [SerializeField] private int _scaleOffset;

    [Range(0, 20)]
    [SerializeField] private int _scaleSpeed;

    [Header("Prefabs")]
    [SerializeField] private GameObject _panelPrefab;

    [SerializeField] private CurrencyCounter _currencyCounter;

    public CurrencyCounter CurrencyCounter 
    { 
        get => _currencyCounter; 
        private set => _currencyCounter = value; 
    }

    [SerializeField] private Tower[] _towers;

    public Tower this[int index]
    {
        get => _towers[index];
        set => _towers[index] = value;
    }

    private GameObject[] _panels;

    private Vector2[] _panelPositions;
    private Vector2 _contentVector;
    private Vector2[] _panelsScale;

    private RectTransform[] _panelRectTransforms;
    private RectTransform _contentRectTransform;

    [SerializeField] private ScrollRect _scrollRect;

    public int SelectedPanelID { get; private set; }

    public bool IsScrolling { get; private set; }

    public void Scroll(bool isScroll)
    {
        IsScrolling = isScroll;
        if (isScroll) _scrollRect.inertia = true;
    }

    private void Awake() => _contentRectTransform = GetComponent<RectTransform>();

    private void Start()
    {
        GeneratePanels();
        _panelsScale = new Vector2[_towers.Length];
    }

    public void GeneratePanels()
    {
        CreatePanels();
        CreatePanelsRectTransforms();
        CreatePanelPositions();
    }

    private void CreatePanels()
    {
        _panels = new GameObject[_towers.Length];
        for (int index = 0; index < _towers.Length; ++index)
        {

            _panels[index] = Instantiate(_panelPrefab, transform, false);
            _panels[index].GetComponentsInChildren<TextMeshProUGUI>()[1].text = _towers[index].Name;
            _panels[index].transform.GetChild(0).GetComponentInChildren<Image>().sprite = _towers[index].ShopImage;

            var scroll = _panels[index].transform.GetChild(0).GetComponent<ScrollPanelTouchReceiever>();

            scroll.CurrentIndex = index;
            scroll.SnapScroll = this;


            if (!_towers[index].IsBought)
            {
                _panels[index].GetComponentsInChildren<TextMeshProUGUI>()[0].text = $"<sprite=0>{_towers[index].Cost}";
                _panels[index].transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1f, 1f, 1f, 0.4f);
                continue;
            }

            _panels[index].transform.GetChild(0).transform.GetChild(0).GetComponentsInChildren<Image>()[0].gameObject.SetActive(false);
            _panels[index].transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = "йсокемн";

            if (_towers[index].IsSelected)
            {
                _panels[index].transform.GetChild(1).GetComponentInChildren<Image>().color = new Color(0.1f, 0.5f, 0.2f, 1);
                _panels[index].transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = "бшапюмн";
                _panels[index].transform.GetChild(2).gameObject.SetActive(true);
                _panels[index].GetComponentInChildren<Outline>().effectColor = new Color(0.1f, 0.5f, 0.2f, 1);
            }
        }

    }

    private void CreatePanelsRectTransforms()
    {
        _panelRectTransforms = new RectTransform[_panels.Length];
        for (int index = 0; index < _panels.Length; ++index)
            _panelRectTransforms[index] = _panels[index].GetComponent<RectTransform>();
    }

    private void CreatePanelPositions()
    {
        float currentPositionX = _panels[0].transform.position.x;
        _panelPositions = new Vector2[_panels.Length];

        for (int index = 0; index < _panels.Length; ++index)
        {
            _panels[index].transform.localPosition = new Vector2(currentPositionX, _panels[index].transform.localPosition.y);
            currentPositionX += _panelRectTransforms[index].sizeDelta.x + _panelOffset;
            _panelPositions[index] = -_panels[index].transform.localPosition;
        }
    }

    private void FixedUpdate()
    {
        if ((_contentRectTransform.anchoredPosition.x >= _panelPositions[0].x || _contentRectTransform.anchoredPosition.x <= _panelPositions[_towers.Length - 1].x) && !IsScrolling)
            _scrollRect.inertia = false;


        float nearestPosition = float.MaxValue;
        for (int index = 0; index < _towers.Length; ++index)
        {
            float distance = Mathf.Abs(_contentRectTransform.anchoredPosition.x - _panelPositions[index].x);
            if (distance <= nearestPosition)
            {
                nearestPosition = distance;
                SelectedPanelID = index;
            }
            float scale = Mathf.Clamp(1 / (distance / _panelOffset) * _scaleOffset, 0.5f, 1f);
            _panelsScale[index].x = Mathf.SmoothStep(_panels[index].transform.localScale.x, scale, _scaleSpeed * Time.fixedDeltaTime);
            _panelsScale[index].y = Mathf.SmoothStep(_panels[index].transform.localScale.y, scale, _scaleSpeed * Time.fixedDeltaTime);

            _panels[index].transform.localScale = _panelsScale[index];
        }
        float scrollVelocity = Mathf.Abs(_scrollRect.velocity.x);
        if (scrollVelocity < 400 && !IsScrolling) _scrollRect.inertia = false;


        if (IsScrolling || scrollVelocity >= 400) return;
        _contentVector.x = Mathf.SmoothStep(_contentRectTransform.anchoredPosition.x, _panelPositions[SelectedPanelID].x, _snapSpeed * Time.fixedDeltaTime);
        _contentRectTransform.anchoredPosition = _contentVector;
    }

}
