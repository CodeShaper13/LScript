using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Shows a warning when their is a problem with the device's camera (none present or no authorization)
/// </summary>
[RequireComponent(typeof(Text))]
public class TextCameraWarning : MonoBehaviour {

    [SerializeField, Required]
    private WebCamImage _camImg = null;

    private Text text = null;

    private void Awake() {
        this.text = this.GetComponent<Text>();
    }

    private void Update() {
        switch(this._camImg.state) {
            case WebCamImage.EnumCameraState.NO_AUTHORIZATION:
                this.text.text = "No Authorization To Use Camera...";
                break;
            case WebCamImage.EnumCameraState.NO_CAMERAS:
                this.text.text = "No Camera(s) Found On Device";
                break;
            case WebCamImage.EnumCameraState.OK:
                this.text.text = string.Empty;
                break;
        }
    }
}