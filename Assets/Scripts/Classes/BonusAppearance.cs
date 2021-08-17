using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusAppearance : MonoBehaviour
{
    [SerializeField] private Counter _currencyCount;

    [SerializeField, Range(0, 10)] private int _limit;
    public void Show()
    {
        if (_currencyCount.Count >= _limit) gameObject.SetActive(true);
        else gameObject.SetActive(false);
    }

}
