using UnityEngine;
using UnityEngine.UI;

public class WebCamImage : MonoBehaviour {

    [SerializeField]
    public RawImage _rawImage = null;
    [SerializeField]
    private AspectRatioFitter _aspectRatioFilter = null;
    [SerializeField, Tooltip("The index of the Camera to use."), Min(0)]
    private int _startCameraIndex = 0;
    [SerializeField]
    private FilterMode _cameraFilterMode = FilterMode.Trilinear;

    /// <summary> The selected device index. </summary>
    private int indexDevice = -1;

    public bool isPlaying {
        get => this.webcamTexture == null ? false : this.webcamTexture.isPlaying;
        set {
            if(this.webcamTexture != null) {
                if(value) {
                    this.webcamTexture.Play();
                } else {
                    this.webcamTexture.Stop();
                }
            }
        }
    }

    public int cameraIndex {
        get => this.indexDevice;
        set { this.setCamera(value); }
    }

    public WebCamTexture webcamTexture { get; private set; } = null;

    public EnumCameraState state {
        get {
            if(!Application.HasUserAuthorization(UserAuthorization.WebCam)) {
                return EnumCameraState.NO_AUTHORIZATION;
            }
            else if(WebCamTexture.devices == null || WebCamTexture.devices.Length == 0) {
                return EnumCameraState.NO_CAMERAS;
            }
            else {
                return EnumCameraState.OK;
            }
        }
    }

    private void Start() {
        Application.RequestUserAuthorization(UserAuthorization.WebCam);
    }

    private void Update() {
        if(this.state == EnumCameraState.OK) {

            // Once the camera's have been located, start using the first one.
            if(this.indexDevice == -1 && WebCamTexture.devices.Length > 0) {
                this.setCamera(Mathf.Clamp(this._startCameraIndex, 0, WebCamTexture.devices.Length));
            }

            if(this.isPlaying && this.webcamTexture.didUpdateThisFrame) {
                // Rotate the raw image to the correct orientation.
                this._rawImage.transform.localEulerAngles = new Vector3(0, 0, -this.webcamTexture.videoRotationAngle);

                // adjust the size of the camera.
                RectTransform rt = this.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(this.GetComponentInParent<CanvasScaler>().referenceResolution.y, 0);

                // Update the AspectRatioFitter's ratio.
                if(this._aspectRatioFilter != null) {
                    float videoRatio = ((float)this.webcamTexture.width) / this.webcamTexture.height;
                    this._aspectRatioFilter.aspectRatio = videoRatio;
                }

                // Unflip if vertically flipped
                //this._rawImage.uvRect = this.webcamTexture.videoVerticallyMirrored ? new Rect(0f, 1f, 1f, -1f) : new Rect(0f, 0f, 1f, 1f);
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
            this.isPlaying = false;

            // Destroy the old render texture.
            GameObject.DestroyImmediate(this.webcamTexture, true);
        }

        this.webcamTexture = new WebCamTexture(WebCamTexture.devices[index].name);
        this.webcamTexture.filterMode = this._cameraFilterMode;

        // start playing
        this.isPlaying = true;

        this._rawImage.texture = this.webcamTexture;
    }

    /// <summary>
    /// Cycles to the next camera on the device.
    /// </summary>
    public void nextCamera() {
        int index = this.cameraIndex + 1;
        if(index >= WebCamTexture.devices.Length) {
            index = 0;
        }

        this.cameraIndex = index;
    }

    public enum EnumCameraState {
        NO_AUTHORIZATION = 0,
        NO_CAMERAS = 1,
        OK = 2,
    }
}
