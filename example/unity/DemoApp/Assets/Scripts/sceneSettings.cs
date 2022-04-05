using UnityEngine;
using Vuforia;

public class sceneSettings : MonoBehaviour
{
    private bool mFlashEnabled = false;
    public GameObject noConnection;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            noConnection.SetActive(true);
        }
        else
        {
            noConnection.SetActive(false);
        }
    }
    public void flashlight()
    {
        if (mFlashEnabled)
        {
            CameraDevice.Instance.SetFlashTorchMode(false);
            mFlashEnabled = false;
        }
        else
        {
            CameraDevice.Instance.SetFlashTorchMode(true);
            mFlashEnabled = true;
        }

    }
}
