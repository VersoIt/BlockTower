using UnityEngine;
using UnityEngine.Events;
using Assets.Scripts.Data;

class ScrollPanelTouchReceiever : MonoBehaviour
{
    [SerializeField] private GameObject _buyUI;

    public int CurrentIndex { get; set; }

    public SnapScrolling SnapScroll { get; set; }

    public UnityEvent BuyTrigger;

    public void SelectPanel()
    {
        print(CurrentIndex);
        if (SnapScroll[CurrentIndex].IsBought)
        {
            print("�������");
        }
        else
        {
            if (SnapScroll.CurrencyCounter.CurrencyCount >= SnapScroll[CurrentIndex].Cost)
            {
                print("test");
                _buyUI.SetActive(true);
            }
            else
            {
                print("������������ �����");
            }

        }
        if (SnapScroll[CurrentIndex].IsSelected)
        {
            print("�������");
        }

    }
}