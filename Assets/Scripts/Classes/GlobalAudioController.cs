using UnityEngine;
using UnityEngine.Audio;
using Assets.Scripts.Interfaces;

public class GlobalAudioController : MonoBehaviour, IController
{
    [SerializeField] private AudioMixerGroup _mixer;

    [SerializeField] private string _groupName;

    private HardDriveStorage _storage;

    private bool _isEnable;

    public bool IsEnable() => _isEnable;

    private void Start()
    {
        _storage = GetComponent<HardDriveStorage>();

        if (_storage.IsEnable()) 
            Enable();
        else
            Disable(); 
    }

    public void Disable()
    {
        _mixer.audioMixer.SetFloat(_groupName, -80f);

        _isEnable = false;
        _storage.Disable();
    }

    public void Switch()
    {
        if (_isEnable) { Disable(); }
        else { Enable(); }
    }

    public void Enable()
    {
        _mixer.audioMixer.SetFloat(_groupName, 0f);

        _isEnable = true;
        _storage.Enable();
    }

    public void SetState(bool targetState)
    {
        if (targetState) Enable();
        else Disable();
    }
}