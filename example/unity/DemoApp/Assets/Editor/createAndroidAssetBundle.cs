using UnityEditor;
using UnityEngine;

public class createAndroidAssetBundle : MonoBehaviour
{
    [MenuItem("Assets/Build Android AssetBundle")]
    static void BuildAllAssetBundles()
    {
        BuildPipeline.BuildAssetBundles("Assets/AssetBundlesAndroid", BuildAssetBundleOptions.None, BuildTarget.Android);
    }
}
