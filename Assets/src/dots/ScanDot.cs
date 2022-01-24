using UnityEngine;
using UnityEngine.UI;
using LScript;

[RequireComponent(typeof(RawImage))]
public class ScanDot : MonoBehaviour {

    private RawImage rawImg;
    private BrickColorList colorList;

    public BrickColor brickColor { get; private set; } = BrickColor.RED;

    private void Awake() {
        this.rawImg = this.GetComponent<RawImage>();
    }

    private void Start() {
        this.colorList = GameObject.FindObjectOfType<BrickColorList>();
    }

    public void updateDot(WebCamTexture webcamTexture, RawImage img) {
        Vector2 v;
        Util.getPositionOnImage01(img, this.transform.position, out v);
        int xpos = (int)(webcamTexture.width * v.x);
        int ypos = (int)(webcamTexture.height * v.y);

        Color averageColor = Util.getAverageColor(webcamTexture, xpos, ypos, 6);

        this.brickColor = this.colorList.RGBToBrickColor(averageColor);

        // Update ui color.
        this.rawImg.color = this.colorList.brickToRgb(this.brickColor);
    }
}
