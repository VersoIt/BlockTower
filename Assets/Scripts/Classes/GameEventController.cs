using Assets.Scripts.Interfaces;
using UnityEngine;

public class GameEventController : MonoBehaviour, IGameEventable
{

    [SerializeField] private MonoBehaviour _targetObject;

    private IGameEventable _target;

    private void Start() => _target = _targetObject as IGameEventable;

    public void Pause() => _target.Pause();

    public void Resume() => _target.Resume();

    public void Reload() => _target.Reload();
}
