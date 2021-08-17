using UnityEngine;

public class MenuStatement : MonoBehaviour
{
    public static MenuStatement Instance { get; private set; }

    public bool IsEnabled { get; set; }

    private void Awake() => Instance = this;
}
