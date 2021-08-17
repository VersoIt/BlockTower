using UnityEngine;


class GameMenu : MonoBehaviour
{
    public static GameMenu Instance { get; set; }

    public bool IsEnabled { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}