using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditScript : MonoBehaviour
{
    [SerializeField] RectTransform creditPanel;
    [SerializeField] float speed;

    void Update()
    {
        var pos = creditPanel.anchoredPosition;
        pos.y += speed * Time.deltaTime;
        creditPanel.anchoredPosition = pos;
    }

    public void ReturnToHomeScreen()
    {
        SceneManager.LoadScene("HomeScreen", LoadSceneMode.Single);
    }
}
