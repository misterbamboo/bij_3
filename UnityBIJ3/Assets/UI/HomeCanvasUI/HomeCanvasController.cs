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
        Application.Quit();
        EditorApplication.isPlaying = false;
    }

    public void CreditScreen()
    {
        SceneManager.LoadScene("CreditScreen", LoadSceneMode.Single);
    }
}
