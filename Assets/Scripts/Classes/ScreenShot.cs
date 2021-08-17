using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class ScreenShot : MonoBehaviour
{
    public const string _screenShotName = "Unity screenshot";

    [MenuItem("Tools/Make screenshot")]
    public static void MakeScreenShot()
    {
        ScreenCapture.CaptureScreenshot(_screenShotName);
    }
}
