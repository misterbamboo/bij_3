using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeCanvasController : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("PlayScreen", LoadSceneMode.Single);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void CreditScreen()
    {
        SceneManager.LoadScene("CreditScreen", LoadSceneMode.Single);
    }
}
