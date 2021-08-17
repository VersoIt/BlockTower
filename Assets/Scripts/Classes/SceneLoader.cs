using Assets.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour, IReloadable
{
    public void Reload()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Load(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void OpenDevSite()
    {
        Application.OpenURL("https://vk.com/ruslan.itpro");
    }
}
