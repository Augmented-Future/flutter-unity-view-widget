/*============================================================================== 
Copyright (c) 2015-2018 PTC Inc. All Rights Reserved.

Copyright (c) 2012-2015 Qualcomm Connected Experiences, Inc. All Rights Reserved.

Vuforia is a trademark of PTC Inc., registered in the United States and other 
countries.
==============================================================================*/
using UnityEngine;
using UnityEngine.Video;
using System.Collections;
using Vuforia;

public class CloudTrackableEventHandler : DefaultTrackableEventHandler
{
    #region PRIVATE_MEMBERS
    CloudRecoBehaviour m_CloudRecoBehaviour;
    CloudContentManager m_CloudContentManager;
    AnimationsManager m_AnimationsManager;
    bool m_isAugmentationVisible;
    #endregion // PRIVATE_MEMBERS
    public GameObject scanImage;
    public GameObject cam;
    public GameObject close;
    bool founded = false;

    #region MONOBEHAVIOUR_METHODS
    protected override void Start()
    {
        base.Start();

        m_CloudRecoBehaviour = FindObjectOfType<CloudRecoBehaviour>();
        m_CloudContentManager = FindObjectOfType<CloudContentManager>();
        m_AnimationsManager = FindObjectOfType<AnimationsManager>();

        // Hide the Canvas Augmentation
        base.OnTrackingLost();
    }
    #endregion // MONOBEHAVIOUR_METHODS


    #region BUTTON_METHODS
    public void OnReset()
    {
        Debug.Log("<color=blue>OnReset()</color>");

        // Changing CloudRecoBehaviour.CloudRecoEnabled to true will call TargetFinder.StartRecognition()
        // and also call all registered ICloudRecoEventHandler.OnStateChanged() with true.
        m_CloudRecoBehaviour.CloudRecoEnabled = true;
        m_isAugmentationVisible = false;

        // Hide the Canvas Augmentation
        base.OnTrackingLost();

        TrackerManager.Instance.GetTracker<ObjectTracker>().GetTargetFinder<ImageTargetFinder>().ClearTrackables(false);
    }
    #endregion BUTTON_METHODS


    #region PUBLIC_METHODS
    /// <summary>
    /// Method called from the CloudRecoEventHandler
    /// when a new target is created
    /// </summary>
    public void TargetCreated(TargetFinder.CloudRecoSearchResult targetSearchResult)
    {
        //m_AnimationsManager.SetInitialAnimationFlags();

        //m_CloudContentManager.HandleMetadata(targetSearchResult.MetaData);
        Debug.Log("targetCreated");
    }

    #endregion // PUBLIC_METHODS


    #region PROTECTED_METHODS
    protected override void OnTrackingFound()
    {
        founded = true;
        scanImage.SetActive(false);
        Debug.Log("<color=blue>OnTrackingFound()</color>");

        base.OnTrackingFound();

        Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
        Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

        // Enable rendering:
        foreach (Renderer component in rendererComponents)
        {
            component.enabled = true;
        }

        // Enable colliders:
        foreach (Collider component in colliderComponents)
        {
            component.enabled = true;
        }
        if (transform.Find("video").gameObject.activeSelf || transform.Find("greenscreen").gameObject.activeSelf)
        {
            if (GetComponentInChildren<VideoPlayer>().enabled == false)
            {
                GetComponentInChildren<VideoPlayer>().enabled = true;
            }            
        }
        else if (transform.Find("youtube").gameObject.activeSelf)
        {
            if (GetComponentInChildren<YoutubePlayer>().enabled == false)
            {
                transform.Find("youtube").gameObject.transform.Find("Canvas").gameObject.SetActive(true);
                GetComponentInChildren<YoutubePlayer>().enabled = true;
                GetComponentInChildren<YoutubePlayer>().Play();
            }
        }
        else if(transform.Find("audio").gameObject.activeSelf)
        {
            if (GetComponentInChildren<AudioSource>().enabled == false)
            {
                GetComponentInChildren<AudioSource>().enabled = true;
            }
        }
        //m_isAugmentationVisible = true;

        // Starts playing the animation to 3D
        //m_AnimationsManager.PlayAnimationTo3D(transform.GetChild(0).gameObject);
        StartCoroutine(incVisit());
    }
    IEnumerator incVisit()
    {
        WWWForm form = new WWWForm();
        form.AddField("imageTarget", mTrackableBehaviour.TrackableName);
        WWW www = new WWW(SaveLink.link+"increment_visit.php", form);
        yield return www;
        Debug.Log(www.text);
        if (www.text != "none")
        {
            Debug.Log("Incremented");
        }
        else
        {
            Debug.Log("user creation failed. Error#" + www.text);
        }
    }

    protected override void OnTrackingLost()
    {
        if (founded)
        {
            close.SetActive(true);
        }
        scanImage.SetActive(true);
        Debug.Log("<color=blue>OnTrackingLost()</color>");

        // Checks that the book info is displayed
        /*if (m_isAugmentationVisible)
        {
            // Starts playing the animation to 2D
            m_AnimationsManager.PlayAnimationTo2D(transform.GetChild(0).gameObject);
        }*/
        Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
        Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

        // Disable rendering:
        foreach (Renderer component in rendererComponents)
        {
            //component.enabled = false;
        }

        // Disable colliders:
        foreach (Collider component in colliderComponents)
        {
            //component.enabled = false;
        }
        gameObject.transform.localPosition = new Vector3(0, transform.position.y, 0);
        gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);     
        cam.transform.localPosition = new Vector3(0, cam.transform.position.y, 0);
        cam.transform.localEulerAngles = new Vector3(90f, 0, 0);
    }
    #endregion // PROTECTED_METHODS
}