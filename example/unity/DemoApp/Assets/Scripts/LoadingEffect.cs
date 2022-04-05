using UnityEngine;
using System.Collections;

public class LoadingEffect : MonoBehaviour
{
    void Start()
    {
        iTween.RotateBy(gameObject, iTween.Hash("z", 1, "time", 1, "looptype", iTween.LoopType.loop, "easetype", iTween.EaseType.linear));
    }

}