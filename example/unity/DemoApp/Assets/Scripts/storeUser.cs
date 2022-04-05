using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class storeUser : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (File.Exists(Application.persistentDataPath + "/userSetting.txt"))
        {
            using (TextReader reader = File.OpenText(Application.persistentDataPath + "/userSetting.txt"))
            {
                string str = reader.ReadToEnd();
                string[] splitString = str.Split(new string[] { ",", "\n" }, StringSplitOptions.None);
                SaveLink.username = splitString[0];
            }
        }
    }
}
