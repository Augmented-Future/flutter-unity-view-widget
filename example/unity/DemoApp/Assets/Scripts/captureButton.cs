using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.IO;
using System.Collections;
using VoxelBusters.ReplayKit;


public class captureButton : MonoBehaviour
{
    public GameObject stopRecordBtn;
    public GameObject startRecordBtn;
    private bool displayed = true;
    public string videoPath;
    public GameObject vidPreview;
    public GameObject rawImage;
    public GameObject mainUI;
    private Component[] UItoHide;
    private Texture2D ss;
    public GameObject imgPreview;
    public RawImage irawImage;
    public GameObject recoText;

    public void IsAvailable()
    {
        bool isRecordingAPIAvailable = ReplayKitManager.IsRecordingAPIAvailable();
        string message = isRecordingAPIAvailable ? "Replay Kit recording API is available!" : "Replay Kit recording API is not available.";
        Debug.Log(message);
    }
    void Start()
    {
        ReplayKitManager.Initialise();
    }

    public void videoRecord()
    {
        StartRecording();
    }
    public void captureScreenShot()
    {
        StartCoroutine(TakeScreenshotAndSave());
    }
    private IEnumerator TakeScreenshotAndSave()
    {
        GetComponent<AudioSource>().Play();
        UItoHide = mainUI.GetComponentsInChildren<Image>();
        foreach (Image hide in UItoHide)
            hide.enabled = false;
        recoText.SetActive(false);
        yield return new WaitForEndOfFrame();

        ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        string filePath = Path.Combine(Application.temporaryCachePath, "Visual Photo.jpeg");
        File.WriteAllBytes(filePath, ss.EncodeToPNG());
        // To avoid memory leaks
        imgPreview.SetActive(true);
        irawImage.texture = ss;

    }
    public void ShareImg()
    {
        new NativeShare().AddFile(ss)
        .SetSubject("Visual AR").SetText("Augmented Reality Experience with Visual AR")
        .SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
        .Share();
        discardImg();
    }
    public void saveImg()
    {
       // Debug.Log("Permission result: " + NativeGallery.SaveImageToGallery(ss, "Visual AR", "Visual Photo {0}.jpeg"));
        discardImg();
    }
    public void discardImg()
    {
        imgPreview.SetActive(false);
        foreach (Image hide in UItoHide)
            hide.enabled = true;
        recoText.SetActive(true);
    }
    void OnEnable()
    {
        // Register to the events
        ReplayKitManager.DidInitialise += DidInitialise;
        ReplayKitManager.DidRecordingStateChange += DidRecordingStateChange;
    }
    void OnDisable()
    {
        // Deregister the events
        ReplayKitManager.DidInitialise -= DidInitialise;
        ReplayKitManager.DidRecordingStateChange -= DidRecordingStateChange;
    }
    private void DidRecordingStateChange(ReplayKitRecordingState state, string
    message)
    {
        Debug.Log("Received Event Callback : DidRecordingStateChange [State:" +
       state.ToString() + " " + "Message:" + message);
        switch (state)
        {
            case ReplayKitRecordingState.Started:
                Debug.Log("ReplayKitManager.DidRecordingStateChange : Video Recording Started");
                stopRecordBtn.SetActive(true);
                startRecordBtn.SetActive(false);

                break;
            case ReplayKitRecordingState.Stopped:
                Debug.Log("ReplayKitManager.DidRecordingStateChange : Video Recording Stopped");
                stopRecordBtn.SetActive(false);
                startRecordBtn.SetActive(true);
                break;
            case ReplayKitRecordingState.Failed:
                Debug.Log("ReplayKitManager.DidRecordingStateChange : Video Recording Failed with message[" + message + "]");
                stopRecordBtn.SetActive(false);
                startRecordBtn.SetActive(true);
                break;
            case ReplayKitRecordingState.Available:
                Debug.Log("ReplayKitManager.DidRecordingStateChange : Video Recording available for preview");
                stopRecordBtn.SetActive(false);
                startRecordBtn.SetActive(true);
                OnReplay();
                break;
            default:
                Debug.Log("Unknown State");
                stopRecordBtn.SetActive(false);
                startRecordBtn.SetActive(true);
                break;
        }
    }
    private void DidInitialise(ReplayKitInitialisationState state, string message)
    {
        Debug.Log("Received Event Callback : DidInitialise [State:" +
       state.ToString() + " " + "Message:" + message);
        switch (state)
        {
            case ReplayKitInitialisationState.Success:
                Debug.Log("ReplayKitManager.DidInitialise : Initialisation Success");
                break;
            case ReplayKitInitialisationState.Failed:
                Debug.Log("ReplayKitManager.DidInitialise : Initialisation Failed with message[" + message + "]");
                break;
            default:
                Debug.Log("Unknown State");
                break;
        }
    }
    public void StartRecording()
    {
        ReplayKitManager.SetMicrophoneStatus(true);
        ReplayKitManager.StartRecording();
    }
    public void StopRecording()
    {
        ReplayKitManager.StopRecording();
        stopRecordBtn.SetActive(false);
        startRecordBtn.SetActive(true);
    }
    bool Preview()
    {
        if (ReplayKitManager.IsPreviewAvailable())
        {
            return ReplayKitManager.Preview();
        }
        // Still preview is not available. Make sure you call preview after you receive ReplayKitRecordingState.Available status from DidRecordingStateChange
        return false;
    }
    bool Discard()
    {
        if (ReplayKitManager.IsPreviewAvailable())
        {
            return ReplayKitManager.Preview();
        }
        // Still preview is not available. Make sure you call preview after you receive eReplayKitRecordingState.Available status from DidRecordingStateChange
        return false;
    }
    private void OnReplay()
    {
        string path = ReplayKitManager.GetPreviewFilePath();
        Debug.Log("Saved recording to: " + path);
        videoPath = path;
        vidPreview.SetActive(true);
        rawImage.GetComponent<VideoPlayer>().enabled = true;
        rawImage.GetComponent<VideoPlayer>().url = videoPath;
    }
    public void Share()
    {
        ReplayKitManager.SharePreview();
        discard();
    }
    public void save()
    {
       // Debug.Log("Permission result: " + NativeGallery.SaveVideoToGallery(videoPath, "Visual AR", "Visual Video {0}.mp4"));
        discard();
    }
    public void discard()
    {
        rawImage.GetComponent<VideoPlayer>().enabled = false;
        vidPreview.SetActive(false);
        foreach (Image hide in UItoHide)
            hide.enabled = true;
        displayed = true;
        recoText.SetActive(true);
    }
}
