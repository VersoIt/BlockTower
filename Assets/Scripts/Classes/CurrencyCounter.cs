using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CurrencyCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] _textsOfCurrencyCount;

    public const string SaveFormat = "CurrencyCount";

    private int _currencyCount;

    public int CurrencyCount
    {
        get => _currencyCount;
        set
        {
            ChangeTextContent($"<sprite=0>{value}");
            _currencyCount = value;
        }
    }

    private void ChangeTextContent(string content)
    {
        foreach (var text in _textsOfCurrencyCount)
            text.text = content;
    }

    private void Awake() => CurrencyCount = PlayerPrefs.GetInt(SaveFormat);

    public void Save() => PlayerPrefs.SetInt(SaveFormat, _currencyCount);
}
