using Assets.Scripts.Interfaces;
using UnityEngine;

class ProcessLeaveEvents : MonoBehaviour, ILeave
{

    [SerializeField] private MonoBehaviour _targetObject;

    private ILeave _target;

    private void Awake()
    {
        _target = _targetObject as ILeave;
    }

    private void Update() => ProcessBack();

    private void ProcessBack()
    {

        if (Input.GetKey(KeyCode.Menu) || Input.GetKey(KeyCode.Home) || Input.GetKeyDown(KeyCode.Escape))
        {
            Back();
        }

    }

    public void Back() => _target.Back();

    public void Exit() => _target.Exit();
}
