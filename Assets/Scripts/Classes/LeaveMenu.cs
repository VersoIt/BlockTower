using UnityEngine;
using Assets.Scripts.Interfaces;

public class LeaveMenu : MonoBehaviour, ILeave
{
    public void Back() => gameObject.SetActive(false);

    public void Exit() => gameObject.SetActive(false);
}
