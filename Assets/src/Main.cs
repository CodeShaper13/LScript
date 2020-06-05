using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour {

    [SerializeField]
    private RawImage rawImage = null;
    [SerializeField]
    private AspectRatioFitter imageFitter = null;
    [SerializeField]
    private Text textInfo = null;
    [SerializeField]
    private Dots dots = null;

    /// <summary> The selected device index </summary>
    private int indexDevice = -1;
    /// <summary> The web cam texture </summary>
    private WebCamTexture webcamTexture = null;
    private Vector3 rotationVector = new Vector3(0f, 0f, 0f);

    private void Awake() {
        // Request to have access to the camera(s).
        Application.RequestUserAuthorization(UserAuthorization.WebCam);
    }

    private void Update() {
        if(!Application.HasUserAuthorization(UserAuthorization.WebCam)) {
           this.textInfo.text = "Not Authorization To Use Camera...";
            return;
        }
        else if(WebCamTexture.devices == null) {
            this.textInfo.text =  "No Cameras Found On Device";
            return;
        }

        // Once the camera's have been located, start using the first one.
        if(this.indexDevice == -1 && WebCamTexture.devices.Length > 0) {
            this.setCamera(0);
        }

        if(null != webcamTexture && webcamTexture.didUpdateThisFrame) {
            this.rawImage.texture = webcamTexture;

            // Rotate image to show correct orientation 
            rotationVector.z = -this.webcamTexture.videoRotationAngle;
            this.rawImage.rectTransform.localEulerAngles = rotationVector;

            // Set AspectRatioFitter's ratio
            float videoRatio = ((float)this.webcamTexture.width) / this.webcamTexture.height;
            this.imageFitter.aspectRatio = videoRatio;

            Vector3 defaultScale = new Vector3(1f, 1f, 1f);
            Vector3 fixedScale = new Vector3(-1f, 1f, 1f);

            // Unflip if vertically flipped
            this.rawImage.uvRect =
                this.webcamTexture.videoVerticallyMirrored ? new Rect(0f, 1f, 1f, -1f) : new Rect(0f, 0f, 1f, 1f);

            // Mirror front-facing camera's image horizontally to look more natural
            //if(this.mirrorFrontCam) {
            //    imageParent.localScale = activeCameraDevice.isFrontFacing ? new Vector3(-1f, 1f, 1f) : new Vector3(1f, 1f, 1f);
            //}

            this.dots.updateDots(this.webcamTexture, this.rawImage);
        }
    }

    public void callback_capture() {
        print("Creating Script file...");

        Directory.CreateDirectory("scripts/");

        StreamWriter writer = new StreamWriter("scripts/script1.ls", false);
        foreach(PreviewDot dot in this.dots.getDots()) {
            writer.Write((int)dot.getToken());
        }
        writer.Close();

        print("Script Saved!");
    }

    public void callback_nextCamera() {
        this.indexDevice += 1;
        if(this.indexDevice >= WebCamTexture.devices.Length) {
            this.indexDevice = 0;
        }

        this.setCamera(this.indexDevice);
    }

    /// <summary>
    /// Sets the camera to use.
    /// </summary>
    private void setCamera(int index) {
        this.indexDevice = index;
        WebCamDevice device = WebCamTexture.devices[index];

        // Starts clear so background msg can be see in the event of an error.
        this.rawImage.color = Color.white;

        // Cleanup the old camera.
        if(this.webcamTexture != null) {
            if(this.webcamTexture.isPlaying) {
                this.webcamTexture.Stop();
            }

            // Destroy the old render texture.
            if(null != this.webcamTexture) {
                GameObject.DestroyImmediate(this.webcamTexture, true);
            }
        }

        // use the device name
        this.webcamTexture = new WebCamTexture(device.name);
        this.webcamTexture.filterMode = FilterMode.Trilinear; // Make smoother

        // start playing
        this.webcamTexture.Play();

        this.rawImage.texture = webcamTexture;
    }
}