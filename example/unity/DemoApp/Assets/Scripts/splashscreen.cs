using UnityEngine;
using System.IO;

public class splashscreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("check", 2);
    }
    void check()
    {
        if(File.Exists(Application.persistentDataPath + "/userSetting.txt")){
            UnityEngine.SceneManagement.SceneManager.LoadScene("main");
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("login");
        }
    }
}
