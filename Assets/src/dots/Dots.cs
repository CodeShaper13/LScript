using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Dots : MonoBehaviour {

    [SerializeField]
    private int size = 8;
    [SerializeField]
    private GameObject previewDotPrefab = null;
    [SerializeField]
    private RectTransform targetRect = null;

    private List<PreviewDot> dots;

    private void Awake() {
        // Create all of the Dot objects

        this.dots = new List<PreviewDot>();
        for(int i = 0; i < this.size * this.size; i++) {
            GameObject obj = GameObject.Instantiate(this.previewDotPrefab, this.targetRect);
            this.dots.Add(obj.GetComponent<PreviewDot>());
        }

        float xSpacing = this.targetRect.sizeDelta.x / this.size;
        float ySpacing = this.targetRect.sizeDelta.y / this.size;

        for(int x = 0; x < this.size; x++) {
            for(int y = 0; y < this.size; y++) {
                PreviewDot dot = this.dots[(y * this.size) + x];

                dot.transform.localPosition = new Vector2((xSpacing / 2) + x * xSpacing, ((ySpacing / 2) + y * ySpacing) * -1);
            }
        }
    }

    /// <summary>
    /// Updates all of the dots, causing them to resample the pixels on the screen.
    /// </summary>
    public void updateDots(WebCamTexture webcamTexture, RawImage img) {
        foreach(PreviewDot dot in this.dots) {
            dot.updateDot(webcamTexture, img);
        }
    }

    public List<PreviewDot> getDots() {
        return this.dots;
    }
}
