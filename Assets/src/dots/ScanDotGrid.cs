using UnityEngine;
using System.Collections.Generic;
using NaughtyAttributes;

public class ScanDotGrid : MonoBehaviour {

    [SerializeField, Required]
    private WebCamImage _camImg = null;
    [SerializeField]
    private int size = 8;
    [SerializeField]
    private GameObject previewDotPrefab = null;
    [SerializeField]
    private RectTransform targetRect = null;

    public List<ScanDot> dots { get; private set; }

    private void Awake() {
        this.dots = new List<ScanDot>();
    }

    private void Start() {
        // Create all of the Dots.
        for(int i = 0; i < this.size * this.size; i++) {
            GameObject obj = GameObject.Instantiate(this.previewDotPrefab, this.targetRect);
            this.dots.Add(obj.GetComponent<ScanDot>());
        }


        // Arrange all of the dots in a grid.
        float xSpacing = this.targetRect.sizeDelta.x / this.size;
        float ySpacing = this.targetRect.sizeDelta.y / this.size;

        for(int x = 0; x < this.size; x++) {
            for(int y = 0; y < this.size; y++) {
                ScanDot dot = this.dots[(y * this.size) + x];
                dot.transform.localPosition = new Vector2(
                    (xSpacing / 2) + x * xSpacing,
                    ((ySpacing / 2) + y * ySpacing) * -1);
            }
        }
    }

    private void Update() {
        if(this._camImg.state == WebCamImage.EnumCameraState.OK && this._camImg.isPlaying) {
            // Update all of the dots by having them resample the pixels under them.
            foreach(ScanDot dot in this.dots) {
                dot.updateDot(this._camImg.webcamTexture, this._camImg._rawImage);
            }
        }
    }
}
