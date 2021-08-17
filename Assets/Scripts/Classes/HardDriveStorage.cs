using Assets.Scripts.Interfaces;
using UnityEngine;

class HardDriveStorage : MonoBehaviour, IHardDriveAccessable, IController
{
    [SerializeField] public string TagFormat;
    [SerializeField] public string DisableFormat;
    [SerializeField] public string EnableFormat;

    public bool IsEnable()
    {
        return PlayerPrefs.GetString(TagFormat) == EnableFormat ? true : PlayerPrefs.GetString(EnableFormat) == DisableFormat ? false : false;
    }

    public void Disable()
    {
        PlayerPrefs.SetString(TagFormat, DisableFormat);
    }

    public void Enable()
    {
        PlayerPrefs.SetString(TagFormat, EnableFormat);
    }

    public void Switch()
    {
        if (IsEnable()) { Disable(); }
        else { Enable(); }
    }

    public void SetString(string value)
    {
        PlayerPrefs.SetString(TagFormat, value);
    }

    public void SetInt(int value)
    {
        PlayerPrefs.SetInt(TagFormat, value);
    }

    public void SetFloat(float value)
    {
        PlayerPrefs.SetFloat(TagFormat, value);
    }

    public string GetString()
    {
        return PlayerPrefs.GetString(TagFormat);
    }

    public int GetInt()
    {
        return PlayerPrefs.GetInt(TagFormat);
    }

    public float GetFloat()
    {
        return PlayerPrefs.GetFloat(TagFormat);
    }

    public void SetState(bool targetState)
    {
        if (targetState) Enable();
        else Disable();
    }

}

