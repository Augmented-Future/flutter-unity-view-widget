using UnityEditor;
using UnityEngine;

public class createIosAssetBundle : MonoBehaviour
{
    [MenuItem("Assets/Build iOS AssetBundle")]
    static void BuildAllAssetBundles()
    {
        BuildPipeline.BuildAssetBundles("Assets/AssetBundlesiOS", BuildAssetBundleOptions.None, BuildTarget.iOS);
    }
}
