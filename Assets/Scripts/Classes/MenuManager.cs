using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Classes
{
    class MenuManager : MonoBehaviour, ILeave
    {
        [SerializeField] private GameObject _menu;

        public void Back()
        {
            print("back");
            _menu.SetActive(false);
        }

        public void Open()
        {
            _menu.SetActive(true);
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}
