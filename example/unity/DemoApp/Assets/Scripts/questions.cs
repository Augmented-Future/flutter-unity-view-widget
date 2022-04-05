using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class questions : MonoBehaviour
{
    public ToggleGroup myToggleGroup;
    string toogleGroupValue;
    public GameObject a;
    public GameObject b;
    public GameObject c;
    public GameObject d;
    public GameObject question;
    int counter = 1;
    string[] splitString;
    int length;
    string str;
    public GameObject labelPanel;
    public GameObject submitBtn;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(get());
    }

    IEnumerator get()
    {
        WWWForm form = new WWWForm();
        form.AddField("item", SaveLink.item);
        form.AddField("username", SaveLink.username);
        WWW www = new WWW(SaveLink.link + "userquestions.php", form);
        yield return www;
        if (www.text != "failed")
        {
            if(www.text != "matched")
            {
                if (www.text != "0")
                {
                    Debug.Log(www.text);
                    splitString = www.text.Split(new string[] { ",", "\n" }, StringSplitOptions.None);
                    length = splitString.Length;
                    loadQuestion();
                }
                else
                {
                    question.GetComponent<Text>().text = "No Questions Found for this Image Target. Thank You!";
                    labelPanel.SetActive(false);
                    submitBtn.SetActive(false);
                }
            }
            else
            {
                question.GetComponent<Text>().text = "You have already given answers to these questions. Thank You!";
                labelPanel.SetActive(false);
                submitBtn.SetActive(false);
            }                
        }
        else
        {
            Debug.Log("quesry failed.");
        }
    }
    void loadQuestion()
    {
        Debug.Log(counter);
        Debug.Log(length);
        if (counter < length-1)
        {
            question.GetComponent<Text>().text = splitString[counter];
            a.GetComponent<Text>().text = splitString[counter + 1];
            b.GetComponent<Text>().text = splitString[counter + 2];
            c.GetComponent<Text>().text = splitString[counter + 3];
            d.GetComponent<Text>().text = splitString[counter + 4];
            counter += 5;
        }
        else
        {
            question.GetComponent<Text>().text = "Answers Submitted. Thank You!";
            labelPanel.SetActive(false);
            submitBtn.SetActive(false);
        }
    }
    public void submit()
    {
        StartCoroutine(submitAnswer());
    }
    IEnumerator submitAnswer()
    {
        foreach (var toggle in myToggleGroup.ActiveToggles())
        {
            toogleGroupValue = toggle.GetComponentInChildren<Text>().text;
            break;
        }
        WWWForm form = new WWWForm();
        form.AddField("username", SaveLink.username);
        form.AddField("object", SaveLink.item);
        form.AddField("question", question.GetComponent<Text>().text);
        form.AddField("answer", toogleGroupValue);
        WWW www = new WWW(SaveLink.link + "useranswers.php", form);
        yield return www;
        if (www.text != "failed")
        {
            loadQuestion();
            Debug.Log(www.text);
        }
    }
}
