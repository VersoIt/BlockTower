using UnityEngine;
using System;
using UnityEngine.UI;

public class RecordsProvider : MonoBehaviour
{
    [SerializeField] private Text _maxWidthText;
    [SerializeField] private Text _maxHeightText;
    [SerializeField] private Text _blocksInstalledText;

    private int _maxWidth;
    private int _maxHeight;
    private int _blocksInstalled;

    private void Awake()
    {
        _maxWidth = PlayerPrefs.GetInt(nameof(_maxWidth));
        _maxHeight = PlayerPrefs.GetInt(nameof(_maxHeight));
        _blocksInstalled = PlayerPrefs.GetInt(nameof(_blocksInstalled));
    }

    void Start() => UpdateText();

    public void UpdateData(int width, int height, int blocksCount)
    {
        _maxWidth = Math.Max(_maxWidth, width);
        _maxHeight = Math.Max(_maxHeight, height);
        _blocksInstalled += blocksCount;

        UpdateText();
    }

    private void WriteData()
    {
        PlayerPrefs.SetInt(nameof(_maxWidth), _maxWidth);
        PlayerPrefs.SetInt(nameof(_maxHeight), _maxHeight);
        PlayerPrefs.SetInt(nameof(_blocksInstalled), _blocksInstalled);
    }

    private void OnDisable() => WriteData();

    private void UpdateText()
    {
        _maxWidthText.text = $"Максимальная\nширина: {_maxWidth}";
        _maxHeightText.text = $"Максимальная\nвысота: {_maxHeight}";
        _blocksInstalledText.text = $"Блоков\nустановлено: {_blocksInstalled}";
    }
}
