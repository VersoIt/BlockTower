using Assets.Scripts.Interfaces;
using UnityEngine;

class ApplicationLeaver : MonoBehaviour, IQuitable
{
    public void Quit() => Application.Quit();
}
