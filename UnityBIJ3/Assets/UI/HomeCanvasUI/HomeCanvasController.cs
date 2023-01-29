using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeCanvasController : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("PlaySceen", LoadSceneMode.Single);
    }

    public void Quit()
    {
        Application.Quit();
        EditorApplication.isPlaying = false;
    }
}
