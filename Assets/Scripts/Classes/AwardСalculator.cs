using UnityEngine;
using UnityEngine.UI;

public class AwardСalculator : MonoBehaviour
{
    [SerializeField, Range(0, 100)] private int _share;

    [SerializeField, Tooltip("The text to be changed"), Space(5)] private Text _text;

    [SerializeField] private  Counter _counter;

    private void Awake()
    {
        float result = _counter.Count / (100 / _share);
       _text.text = $"+{Mathf.Round(result)}";
    }
}
