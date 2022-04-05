using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class login : MonoBehaviour {

    public InputField nickname;
    public InputField password;
    private string str;

    public void callLogin()
    {
        StartCoroutine(userLogin());
    }
    IEnumerator userLogin()
    {    
        WWWForm form = new WWWForm();
        form.AddField("nickname", nickname.text);
        form.AddField("password", password.text);
        WWW www = new WWW(SaveLink.link + "userlogin.php", form);
        yield return www;
        if (www.text == "0")
        {
            Debug.Log("user logged in successfully");
            str = nickname.text+","+password.text;
            remember();
            UnityEngine.SceneManagement.SceneManager.LoadScene("main");
        }
        else
        {
            Debug.Log("user login failed.");
        }
    }    
    public void remember()
    {
        using (TextWriter writer = File.CreateText(Application.persistentDataPath + "/userSetting.txt"))
        {
            writer.WriteLine(str);
        }
    }
}
