using UnityEditor;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float scroolSpeed = 50;
    [SerializeField] private float padding = 50;

    private int horizontal;
    private int vertical;

    void Update()
    {
        horizontal = 0;
        vertical = 0;

        float screenWidth = GetScreenWidth();
        float screenHeight = GetScreenHeight();

        // Note: Input.mousePosition (0,0) start at bottom left and topLeft is (width,height)
        var rightBound = screenWidth - padding;
        var topBound = screenHeight - padding;
        float leftBound = padding;
        float bottomBound = padding;
        GetKeyboardInfo();
        GetMouseInfo(rightBound, topBound, leftBound, bottomBound);

        MoveCamera();
    }

    private void GetMouseInfo(float rightBound, float topBound, float leftBound, float bottomBound)
    {
        float screenWidth = GetScreenWidth();
        float screenHeight = GetScreenHeight();
        var screen = new Rect(Vector2.zero, new Vector2(screenWidth, screenHeight));
        if (screen.Contains(Input.mousePosition))
        {
            horizontal += Input.mousePosition.x < leftBound ? -1 : 0;
            horizontal += Input.mousePosition.x > rightBound ? 1 : 0;
            vertical += Input.mousePosition.y < bottomBound ? -1 : 0;
            vertical += Input.mousePosition.y > topBound ? 1 : 0;
        }
    }

    private void GetKeyboardInfo()
    {
        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");
    }

    private static float GetScreenHeight()
    {
#if UNITY_EDITOR
        return Handles.GetMainGameViewSize().y;
#else
        return Screen.height;
#endif
    }

    private static float GetScreenWidth()
    {
#if UNITY_EDITOR
        return Handles.GetMainGameViewSize().x;
#else
        return Screen.width;
#endif
    }

    private void MoveCamera()
    {
        if (horizontal != 0 || vertical != 0)
        {
            mainCamera.transform.position += new Vector3(-horizontal, 0, -vertical) * scroolSpeed * Time.deltaTime;
        }
    }
}
