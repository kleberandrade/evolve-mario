using System.IO;
using UnityEngine;

public class Screenshot : MonoBehaviour
{
    public string m_Filename = "figure";

    public bool m_UseTimelapsed;
    public float m_RepeatTime = 1.0f;
    public KeyCode m_TakePhotoKeyCode = KeyCode.P;

    private void Start()
    {
        Directory.CreateDirectory($"{Application.dataPath}/../Save");

        if (m_UseTimelapsed)
        {
            InvokeRepeating("TakePhoto", 0.0f, m_RepeatTime);
        }
    }

    private void TakePhoto()
    {
        ScreenCapture.CaptureScreenshot($"{Application.dataPath}/../Save/{m_Filename}_{Time.time:000000}.png");
    }

    private void Update()
    {
        if (!m_UseTimelapsed && Input.GetKeyDown(m_TakePhotoKeyCode))
        {
            TakePhoto();
        }
    }
}