/*==============================================================================
Copyright (c) 2015-2018 PTC Inc. All Rights Reserved.

Copyright (c) 2012-2015 Qualcomm Connected Experiences, Inc. All Rights Reserved.

Vuforia is a trademark of PTC Inc., registered in the United States and other 
countries.
==============================================================================*/
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.Networking;
using Vuforia;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
//using TriLib;
//using TriLib.Samples;
/// <summary>
/// This MonoBehaviour implements the Cloud Reco Event handling for this sample.
/// It registers itself at the CloudRecoBehaviour and is notified of new search results as well as error messages
/// The current state is visualized and new results are enabled using the TargetFinder API.
/// </summary>
public class recoEventHandler : MonoBehaviour, IObjectRecoEventHandler
{
    #region PRIVATE_MEMBERS
    CloudRecoBehaviour m_CloudRecoBehaviour;
    ObjectTracker m_ObjectTracker;
    TargetFinder m_TargetFinder;
    string contentType;
    string itemName;
    string scale;
    string rotatex;
    string rotatey;
    string rotatez;
    private VideoPlayer videoPlayer;
    public GameObject menuContainer;
    public GameObject menuItem;
    public GameObject showAnimCloud;
    private GameObject model;
    //private BlendShapeControl _blendShapeControlPrefab;
    private Transform _blendShapesContainerTransform;
    public GameObject sliderGO;
    public Slider sliderComp;
    public GameObject loading;
    private GameObject _rootGameObject;
    #endregion // PRIVATE_MEMBERS


    #region PUBLIC_MEMBERS
    /// <summary>
    /// Can be set in the Unity inspector to reference a ImageTargetBehaviour 
    /// that is used for augmentations of new cloud reco results.
    /// </summary>
    [Tooltip("Here you can set the ImageTargetBehaviour from the scene that will be used to " +
             "augment new cloud reco search results.")]
    public ImageTargetBehaviour m_ImageTargetBehaviour;
    #endregion // PUBLIC_MEMBERS


    #region MONOBEHAVIOUR_METHODS
    /// <summary>
    /// Register for events at the CloudRecoBehaviour
    /// </summary>
    void Start()
    {
        // Register this event handler at the CloudRecoBehaviour
        m_CloudRecoBehaviour = GetComponent<CloudRecoBehaviour>();
        if (m_CloudRecoBehaviour)
        {
            m_CloudRecoBehaviour.RegisterEventHandler(this);
        }
    }
    #endregion // MONOBEHAVIOUR_METHODS


    #region INTERFACE_IMPLEMENTATION_ICloudRecoEventHandler
    /// <summary>
    /// called when TargetFinder has been initialized successfully
    /// </summary>
    public void OnInitialized()
    {
        Debug.Log("Cloud Reco initialized successfully.");

        m_ObjectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
        m_TargetFinder = m_ObjectTracker.GetTargetFinder<ImageTargetFinder>();
    }

    public void OnInitialized(TargetFinder targetFinder)
    {
        Debug.Log("Cloud Reco initialized successfully.");

        m_ObjectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
        m_TargetFinder = targetFinder;
    }

    // Error callback methods implemented in CloudErrorHandler
    public void OnInitError(TargetFinder.InitState initError) { }
    public void OnUpdateError(TargetFinder.UpdateState updateError) { }


    /// <summary>
    /// when we start scanning, unregister Trackable from the ImageTargetBehaviour, 
    /// then delete all trackables
    /// </summary>
    public void OnStateChanged(bool scanning)
    {
        Debug.Log("<color=blue>OnStateChanged(): </color>" + scanning);

        // Changing CloudRecoBehaviour.CloudRecoEnabled to false will call:
        // 1. TargetFinder.Stop()
        // 2. All registered ICloudRecoEventHandler.OnStateChanged() with false.

        // Changing CloudRecoBehaviour.CloudRecoEnabled to true will call:
        // 1. TargetFinder.StartRecognition()
        // 2. All registered ICloudRecoEventHandler.OnStateChanged() with true.
    }

    /// <summary>
    /// Handles new search results
    /// </summary>
    /// <param name="targetSearchResult"></param>
    public void OnNewSearchResult(TargetFinder.TargetSearchResult targetSearchResult)
    {
        Debug.Log("<color=blue>OnNewSearchResult(): </color>" + targetSearchResult.TargetName);

        TargetFinder.CloudRecoSearchResult cloudRecoResult = (TargetFinder.CloudRecoSearchResult)targetSearchResult;

        // This code demonstrates how to reuse an ImageTargetBehaviour for new search results
        // and modifying it according to the metadata. Depending on your application, it can
        // make more sense to duplicate the ImageTargetBehaviour using Instantiate() or to
        // create a new ImageTargetBehaviour for each new result. Vuforia will return a new
        // object with the right script automatically if you use:
        // TargetFinder.EnableTracking(TargetSearchResult result, string gameObjectName)

        // Check if the metadata isn't null
        if (cloudRecoResult.MetaData == null)
        {
            Debug.Log("Target metadata not available.");
        }
        else
        {
            string[] splitString = cloudRecoResult.MetaData.Split(new string[] { ",", "\n" }, StringSplitOptions.None);
            contentType = splitString[0];
            itemName = splitString[1];
            SaveLink.item = itemName;
            scale = splitString[2];
            rotatex = splitString[3];
            rotatey = splitString[4];
            rotatez = splitString[5];
            if (contentType == "video")
            {
                showAnimCloud.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("video").gameObject.SetActive(true);
                m_ImageTargetBehaviour.gameObject.transform.Find("greenscreen").gameObject.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("image").gameObject.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("3dmodel").gameObject.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("youtube").gameObject.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("audio").gameObject.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("AssetBundle").gameObject.SetActive(false);
                video();
            }
            else if (contentType == "greenscreen")
            {
                showAnimCloud.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("video").gameObject.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("greenscreen").gameObject.SetActive(true);
                m_ImageTargetBehaviour.gameObject.transform.Find("image").gameObject.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("3dmodel").gameObject.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("youtube").gameObject.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("audio").gameObject.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("AssetBundle").gameObject.SetActive(false);
                video();
            }
            else if (contentType == "image")
            {
                showAnimCloud.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("video").gameObject.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("greenscreen").gameObject.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("image").gameObject.SetActive(true);
                m_ImageTargetBehaviour.gameObject.transform.Find("3dmodel").gameObject.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("youtube").gameObject.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("audio").gameObject.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("AssetBundle").gameObject.SetActive(false);
                StartCoroutine(image());
            }
            else if (contentType == "3dmodel")
            {
                showAnimCloud.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("video").gameObject.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("greenscreen").gameObject.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("image").gameObject.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("3dmodel").gameObject.SetActive(true);
                m_ImageTargetBehaviour.gameObject.transform.Find("youtube").gameObject.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("audio").gameObject.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("AssetBundle").gameObject.SetActive(false);
                Show3DModel();
            }
            else if (contentType == "youtube")
            {
                showAnimCloud.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("video").gameObject.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("greenscreen").gameObject.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("image").gameObject.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("3dmodel").gameObject.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("youtube").gameObject.SetActive(true);
                m_ImageTargetBehaviour.gameObject.transform.Find("audio").gameObject.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("AssetBundle").gameObject.SetActive(false);
                YTvideo();
            }
            else if (contentType == "audio")
            {
                showAnimCloud.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("video").gameObject.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("greenscreen").gameObject.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("image").gameObject.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("3dmodel").gameObject.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("youtube").gameObject.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("audio").gameObject.SetActive(true);
                m_ImageTargetBehaviour.gameObject.transform.Find("AssetBundle").gameObject.SetActive(false);
                StartCoroutine(audioplay());
            }
            else if (contentType == "website")
            {
                showAnimCloud.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("video").gameObject.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("greenscreen").gameObject.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("image").gameObject.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("3dmodel").gameObject.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("youtube").gameObject.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("audio").gameObject.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("AssetBundle").gameObject.SetActive(false);
                website();
            }
            else
            {
                showAnimCloud.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("video").gameObject.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("greenscreen").gameObject.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("image").gameObject.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("3dmodel").gameObject.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("youtube").gameObject.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("audio").gameObject.SetActive(false);
                m_ImageTargetBehaviour.gameObject.transform.Find("AssetBundle").gameObject.SetActive(true);
                StartCoroutine(ShowAssetBundle());
            }
            Debug.Log("MetaData: " + cloudRecoResult.MetaData);
            Debug.Log("TargetName: " + cloudRecoResult.TargetName);
            Debug.Log("Pointer: " + cloudRecoResult.TargetSearchResultPtr);
            Debug.Log("TrackingRating: " + cloudRecoResult.TrackingRating);
            Debug.Log("UniqueTargetId: " + cloudRecoResult.UniqueTargetId);
        }

        // Changing CloudRecoBehaviour.CloudRecoEnabled to false will call TargetFinder.Stop()
        // and also call all registered ICloudRecoEventHandler.OnStateChanged() with false.
        m_CloudRecoBehaviour.CloudRecoEnabled = false;

        // Clear any existing trackables
        m_TargetFinder.ClearTrackables(false);

        // Enable the new result with the same ImageTargetBehaviour:
        m_TargetFinder.EnableTracking(cloudRecoResult, m_ImageTargetBehaviour.gameObject);

        // Pass the TargetSearchResult to the Trackable Event Handler for processing
        m_ImageTargetBehaviour.gameObject.SendMessage("TargetCreated", cloudRecoResult, SendMessageOptions.DontRequireReceiver);
    }
    #endregion // INTERFACE_IMPLEMENTATION_ICloudRecoEventHandler

    void video()
    {
        Debug.Log(itemName);
        if (itemName != "")
        {
            if (File.Exists(Application.persistentDataPath + "/" + itemName))
            {
                sliderGO.SetActive(false);
                StartCoroutine(playVideo());
            }
            else
            {
                StartCoroutine(DownloadVideo());
            }
        }
    }
    IEnumerator playVideo()
    {
        GameObject plane;
        if (contentType == "video")
        {
            plane = m_ImageTargetBehaviour.gameObject.transform.Find("video").gameObject;
        }
        else
        {
            plane = m_ImageTargetBehaviour.gameObject.transform.Find("greenscreen").gameObject;
        }

        // Will attach a VideoPlayer to the main camera.
        videoPlayer = plane.gameObject.GetComponent<VideoPlayer>();
        videoPlayer.url = Application.persistentDataPath + "/" + itemName;
        videoPlayer.Prepare();
        //Wait until video is prepared
        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }
        Debug.Log("Done Preparing Video");
        Debug.Log(videoPlayer.texture.width);
        Debug.Log(videoPlayer.texture.height);
        float height = videoPlayer.texture.height;
        float width = videoPlayer.texture.width;
        float aspect = height / width;
        Debug.Log(width);
        float vscale = float.Parse(scale);
        //int aspect = videoPlayer.texture.height / videoPlayer.texture.width;
        plane.transform.localScale = new Vector3(vscale, 1f, vscale * aspect);
        float rotateplus = float.Parse(rotatey) + 180;
        plane.transform.localEulerAngles = new Vector3(float.Parse(rotatex), rotateplus, float.Parse(rotatez));
        plane.transform.localPosition = new Vector3(0, 0, 0);
        videoPlayer.Play();
        Debug.Log("Playing Video");
        while (videoPlayer.isPlaying)
        {
            yield return null;
        }

        Debug.Log("Done Playing Video");
    }
    private IEnumerator DownloadVideo()
    {
        var www = new WWW(SaveLink.link + "uploads/" + itemName);
        Debug.Log(SaveLink.link + "uploads/" + itemName);
        while (!www.isDone)
        {
            yield return null;
            sliderGO.SetActive(true);
            sliderComp.value = www.progress;
        }
        sliderGO.SetActive(false);
        var VideoLocalPath = "/" + itemName;
        string filePath = Application.persistentDataPath + VideoLocalPath;
        File.WriteAllBytes(filePath, www.bytes);
        StartCoroutine(playVideo());
    }
    IEnumerator image()
    {
        var _www = new WWW(SaveLink.link + "uploads/" + itemName);
        //_www = WWW.LoadFromCacheOrDownload(ModelURI, 5);
        while (!_www.isDone)
        {
            yield return null;
        }
        GameObject DisplayImage = m_ImageTargetBehaviour.transform.Find("image").gameObject;
        float iscale = float.Parse(scale);
        float heighti = _www.texture.height;
        float widthi = _www.texture.width;
        float aspect = heighti / widthi;
        //int aspect = videoPlayer.texture.height / videoPlayer.texture.width;
        DisplayImage.transform.localScale = new Vector3(iscale, 1f, iscale * aspect);
        DisplayImage.transform.localPosition = new Vector3(0, 0, 0);
        DisplayImage.transform.localEulerAngles = new Vector3(float.Parse(rotatex), float.Parse(rotatey), float.Parse(rotatez));
        DisplayImage.GetComponent<Renderer>().material.mainTexture = _www.texture;
    }
    void Show3DModel()
    {
        if (itemName != "")
        {
            if (File.Exists(Application.persistentDataPath + "/" + itemName))
            {
                sliderGO.SetActive(false);
               // LoadModel();
            }
            else
            {
                StartCoroutine(DownloadModel());
            }
        }
    }
    /*private void AssetLoader_OnMetadataProcessed(AssimpMetadataType metadataType, uint metadataIndex, string metadataKey, object metadataValue)
    {
        Debug.Log("Found metadata of type [" + metadataType + "] at index [" + metadataIndex + "] and key [" + metadataKey + "] with value [" + metadataValue + "]");
    } 
    void LoadModel()
    {
        loading.SetActive(true);
        model = m_ImageTargetBehaviour.gameObject.transform.Find("3dmodel").gameObject;
        var assetLoaderOptions = GetAssetLoaderOptions();
        using (var assetLoader = new AssetLoaderAsync())
        {
            assetLoader.OnMetadataProcessed += AssetLoader_OnMetadataProcessed;
            assetLoader.LoadFromFileWithTextures(Application.persistentDataPath + "/" + itemName, assetLoaderOptions, null, delegate (GameObject loadedGameObject)
            {
                CheckForValidModel(assetLoader);
                _rootGameObject = loadedGameObject;
                _rootGameObject.transform.parent = model.transform;
                _rootGameObject.transform.localPosition = new Vector3(0, 0, 0);
                _rootGameObject.transform.localEulerAngles = new Vector3(float.Parse(rotatex), float.Parse(rotatey), float.Parse(rotatez));
                _rootGameObject.transform.localScale = new Vector3(float.Parse(scale), float.Parse(scale), float.Parse(scale));
                loading.SetActive(false);
                FullPostLoadSetup();
            });
        }
    } */
    private void FullPostLoadSetup()
    {
        if (_rootGameObject != null)
        {
            PostLoadSetup();
        }
    }
    private void PostLoadSetup()
    {        
        var rootAnimation = _rootGameObject.GetComponent<Animation>();
        var skinnedMeshRenderers = _rootGameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        if (skinnedMeshRenderers != null)
        {
            var hasBlendShapes = false;
            foreach (var skinnedMeshRenderer in skinnedMeshRenderers)
            {
                if (!hasBlendShapes && skinnedMeshRenderer.sharedMesh.blendShapeCount > 0)
                {
                    hasBlendShapes = true;
                }
                for (var i = 0; i < skinnedMeshRenderer.sharedMesh.blendShapeCount; i++)
                {
                  //  CreateBlendShapeItem(skinnedMeshRenderer, skinnedMeshRenderer.sharedMesh.GetBlendShapeName(i), i);
                }
            }
        }
        if (rootAnimation != null)
        {
            foreach (Transform child in menuContainer.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            showAnimCloud.SetActive(true);
            foreach (AnimationState animationState in rootAnimation)
            {
                Debug.Log(animationState.name);
                GameObject newItem = Instantiate(menuItem);
                newItem.transform.SetParent(menuContainer.transform);
                newItem.transform.localScale = new Vector3(1f, 1f, 1f);
                newItem.GetComponentInChildren<Text>().text = animationState.name;
                Button button = newItem.GetComponent<Button>();
                button.onClick.AddListener(() => { _rootGameObject.GetComponent<Animation>().Play(animationState.name); });
            }
        }
    }
 /*   private void CreateBlendShapeItem(SkinnedMeshRenderer skinnedMeshRenderer, string name, int index)
    {
        var instantiated = Instantiate(_blendShapeControlPrefab, _blendShapesContainerTransform);
        instantiated.SkinnedMeshRenderer = skinnedMeshRenderer;
        instantiated.Text = name;
        instantiated.BlendShapeIndex = index;
    }
   private void CheckForValidModel(AssetLoaderBase assetLoader)
    {
        if (assetLoader.MeshData == null || assetLoader.MeshData.Length == 0)
        {
            throw new Exception("File contains no meshes");
        }
    }
    private AssetLoaderOptions GetAssetLoaderOptions()
    {
        var assetLoaderOptions = AssetLoaderOptions.CreateInstance();
        assetLoaderOptions.DontLoadCameras = false;
        assetLoaderOptions.DontLoadLights = false;
        assetLoaderOptions.UseOriginalPositionRotationAndScale = true;
        assetLoaderOptions.AddAssetUnloader = true;
        assetLoaderOptions.AutoPlayAnimations = true;
        assetLoaderOptions.AdvancedConfigs.Add(AssetAdvancedConfig.CreateConfig(AssetAdvancedPropertyClassNames.FBXImportDisableDiffuseFactor, true));
        return assetLoaderOptions;
    } */
    private IEnumerator DownloadModel()
    {
        Debug.Log(SaveLink.link + "uploads/" + itemName);
        var _www = new WWW(SaveLink.link + "uploads/" + itemName);
        //_www = WWW.LoadFromCacheOrDownload(ModelURI, 5);
        while (!_www.isDone)
        {
            yield return null;
            sliderGO.SetActive(true); 
            sliderComp.value = _www.progress;
        }
        sliderGO.SetActive(false);

        var ModelLocalPath = "/" + itemName;

        var fullPath = Application.persistentDataPath + ModelLocalPath;
        File.WriteAllBytes(fullPath, _www.bytes);
        StartCoroutine(LoadAfterDownload());
    }

    IEnumerator LoadAfterDownload()
    {
        yield return new WaitForSeconds(3f);
       // LoadModel();
    }
    void YTvideo()
    {
        GameObject plane = m_ImageTargetBehaviour.gameObject.transform.Find("youtube").gameObject;
        // Vide clip from Url
        YoutubePlayer yPlayer = plane.GetComponent<YoutubePlayer>();
        if (yPlayer)
        {
            yPlayer.enabled = true;
            yPlayer.youtubeUrl = itemName;
            yPlayer.Play();
        }
    }
    IEnumerator audioplay()
    {
        GameObject asource = m_ImageTargetBehaviour.gameObject.transform.Find("audio").gameObject;
        AudioSource audioSource = asource.GetComponent<AudioSource>();
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(SaveLink.link + "uploads/" + itemName, AudioType.UNKNOWN))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                AudioClip myClip = DownloadHandlerAudioClip.GetContent(www);
                audioSource.clip = myClip;
                audioSource.Play();
            }
        }
    }
    void website()
    {
        Application.OpenURL(itemName);
    }
    IEnumerator ShowAssetBundle()
    {
        if (itemName != "")
        {
            GameObject model = m_ImageTargetBehaviour.gameObject.transform.Find("AssetBundle").gameObject;
            foreach (Transform child in model.transform)
                child.gameObject.SetActive(false);

            loading.SetActive(true);
            UnityWebRequest request;
            string mname;
            string[] nameString = itemName.Split(new string[] { "-", "\n" }, StringSplitOptions.None);
            if (Application.platform != RuntimePlatform.Android)
            {
                mname = nameString[1];
                request = UnityWebRequestAssetBundle.GetAssetBundle(SaveLink.link + "iPhoneAssets/" + mname, 1, 0);
            }
            else
            {
                mname = nameString[0];
                request = UnityWebRequestAssetBundle.GetAssetBundle(SaveLink.link + "AndroidAssets/" + mname, 1, 0);
            }

            yield return request.SendWebRequest();
            if (request.isDone)
            {
                Debug.Log(request.downloadProgress);
            }
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);
            string[] splitString = mname.Split(new string[] { ".", "\n" }, StringSplitOptions.None);
            GameObject cube = bundle.LoadAsset<GameObject>(splitString[0] + ".prefab");
            //cube.AddComponent<MeshRenderer>();
            //cube.AddComponent<ProductPlacement>();
            //cube.AddComponent<TouchHandler>();
            Instantiate(cube, model.transform);
            bundle.Unload(false);
            loading.SetActive(false);
            while (!Caching.ready)
                yield return null;
        }
    }
    public void onClose()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("main");
    }
}
