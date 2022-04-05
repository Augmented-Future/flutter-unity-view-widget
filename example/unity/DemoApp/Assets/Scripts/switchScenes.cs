using UnityEngine;

public class switchScenes : MonoBehaviour
{
    public void changeScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
