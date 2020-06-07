using UnityEngine;
using UnityEngine.UI;

public class WebCamImage : MonoBehaviour {

    [SerializeField]
    public RawImage rawImage = null;
    [SerializeField]
    private AspectRatioFitter aspectRatioFilter = null;
    [SerializeField]
    private bool correctRotation = true;
    [SerializeField]
    [Tooltip("The index of hte Camera to use.  If this number is greater than the camera count, the highest one is used.")]
    private int startCameraIndex = 0;

    /// <summary> The selected device index. </summary>
    private int indexDevice = -1;
    private Vector3 rotationVector = new Vector3(0f, 0f, 0f);


    public bool playing {
        get {
            return this.webcamTexture == null ? false : this.webcamTexture.isPlaying;
        }
        set {
            if(this.webcamTexture != null) {
                if(value) {
                    this.webcamTexture.Play();
                }
                else {
                    this.webcamTexture.Stop();
                }
            }
        }
    }

    public int cameraIndex {
        get {
            return this.indexDevice;
        }
        set {
            this.setCamera(value);
        }
    }

    public WebCamTexture webcamTexture { get; private set; } = null;

    public CamState state {
        get {
            if(!Application.HasUserAuthorization(UserAuthorization.WebCam)) {
                return CamState.NO_AUTHORIZATION;
            }
            else if(WebCamTexture.devices == null || WebCamTexture.devices.Length == 0) {
                return CamState.NO_CAMERAS;
            }
            else {
                return CamState.OK;
            }
        }
    }

    private void Awake() {
        Application.RequestUserAuthorization(UserAuthorization.WebCam);
    }

    private void Update() {
        if(this.state == CamState.OK) {

            // Once the camera's have been located, start using the first one.
            if(this.indexDevice == -1 && WebCamTexture.devices.Length > 0) {
                int i = this.startCameraIndex;
                if(i > WebCamTexture.devices.Length) {
                    i = 0;
                }
                this.setCamera(i);
            }

            if(null != this.webcamTexture && this.webcamTexture.didUpdateThisFrame) {
                this.rawImage.texture = webcamTexture;

                // Rotate image to show correct orientation.
                if(this.correctRotation) {
                    rotationVector.z = -this.webcamTexture.videoRotationAngle;
                    this.rawImage.rectTransform.localEulerAngles = rotationVector;
                }

                // Set AspectRatioFitter's ratio.
                if(this.aspectRatioFilter != null) {
                    float videoRatio = ((float)this.webcamTexture.width) / this.webcamTexture.height;
                    this.aspectRatioFilter.aspectRatio = videoRatio;
                }

                Vector3 defaultScale = new Vector3(1f, 1f, 1f);
                Vector3 fixedScale = new Vector3(-1f, 1f, 1f);

                // Unflip if vertically flipped
                this.rawImage.uvRect =
                    this.webcamTexture.videoVerticallyMirrored ? new Rect(0f, 1f, 1f, -1f) : new Rect(0f, 0f, 1f, 1f);

                // Mirror front-facing camera's image horizontally to look more natural
                //if(this.mirrorFrontCam) {
                //    imageParent.localScale = activeCameraDevice.isFrontFacing ? new Vector3(-1f, 1f, 1f) : new Vector3(1f, 1f, 1f);
                //}
            }
        }
    }

    /// <summary>
    /// Sets the camera to use.  This will always set the RawIamge to update.
    /// </summary>
    private void setCamera(int index) {
        if(index == this.indexDevice) {
            return; // Same index as before.
        }

        this.indexDevice = index;

        // Cleanup the old camera.
        if(this.webcamTexture != null) {
            this.playing = false;

            // Destroy the old render texture.
            GameObject.DestroyImmediate(this.webcamTexture, true);
        }

        this.webcamTexture = new WebCamTexture(WebCamTexture.devices[index].name);
        //this.webcamTexture.filterMode = FilterMode.Trilinear; // Make smoother

        // start playing
        this.playing = true;

        this.rawImage.texture = webcamTexture;
    }

    public enum CamState {
        NO_AUTHORIZATION = 0,
        NO_CAMERAS = 1,
        OK = 2,
    }
}
