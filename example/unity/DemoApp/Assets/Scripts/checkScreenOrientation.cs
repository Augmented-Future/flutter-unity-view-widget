using UnityEngine;

public class checkScreenOrientation : MonoBehaviour
{
    public GameObject flash;
    public GameObject capture;
    public GameObject record;
    public GameObject recording;
    public GameObject recognition;
    public GameObject gallery;
    public GameObject info;
    public GameObject vidShare;
    public GameObject vidSave;
    public GameObject vidDiscard;
    public GameObject imgShare;
    public GameObject imgSave;
    public GameObject imgDiscard;
    public GameObject progressBar;
    private bool portraitrun = true;
    private bool landscaperun = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.deviceOrientation == DeviceOrientation.Portrait && portraitrun == false)
        {
            flash.transform.localEulerAngles = new Vector3(0, 0, 0);
            capture.transform.localEulerAngles = new Vector3(0, 0, 0);
            record.transform.localEulerAngles = new Vector3(0, 0, 0);
            recording.transform.localEulerAngles = new Vector3(0, 0, 0);
            recognition.transform.localEulerAngles = new Vector3(0, 0, 0);
            gallery.transform.localEulerAngles = new Vector3(0, 0, 0);
            info.transform.localEulerAngles = new Vector3(0, 0, 0);
            vidShare.transform.localEulerAngles = new Vector3(0, 0, 0);
            vidSave.transform.localEulerAngles = new Vector3(0, 0, 0);
            vidDiscard.transform.localEulerAngles = new Vector3(0, 0, 0);
            imgShare.transform.localEulerAngles = new Vector3(0, 0, 0);
            imgSave.transform.localEulerAngles = new Vector3(0, 0, 0);
            imgDiscard.transform.localEulerAngles = new Vector3(0, 0, 0);
            progressBar.transform.localEulerAngles = new Vector3(0, 0, 0);
            portraitrun = true;
            landscaperun = false;
        }
        if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft && landscaperun == false)
        {
            flash.transform.localEulerAngles = new Vector3(0, 0, -90f);
            capture.transform.localEulerAngles = new Vector3(0, 0, -90f);
            record.transform.localEulerAngles = new Vector3(0, 0, -90f);
            recording.transform.localEulerAngles = new Vector3(0, 0, -90f);
            recognition.transform.localEulerAngles = new Vector3(0, 0, -90f);
            gallery.transform.localEulerAngles = new Vector3(0, 0, -90f);
            info.transform.localEulerAngles = new Vector3(0, 0, -90f);
            vidShare.transform.localEulerAngles = new Vector3(0, 0, -90f);
            vidSave.transform.localEulerAngles = new Vector3(0, 0, -90f);
            vidDiscard.transform.localEulerAngles = new Vector3(0, 0, -90f);
            imgShare.transform.localEulerAngles = new Vector3(0, 0, -90f);
            imgSave.transform.localEulerAngles = new Vector3(0, 0, -90f);
            imgDiscard.transform.localEulerAngles = new Vector3(0, 0, -90f);
            progressBar.transform.localEulerAngles = new Vector3(0, 0, -90f);
            portraitrun = false;
            landscaperun = true;
        }
    }
}
