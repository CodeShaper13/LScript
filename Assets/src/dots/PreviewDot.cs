using UnityEngine;
using UnityEngine.UI;
using LScript;

[RequireComponent(typeof(RawImage))]
public class PreviewDot : MonoBehaviour {

    private RawImage img;
    private BrickColorList colorList;

    private BrickColor tokenColor = BrickColor.RED;

    private void Awake() {
        this.img = this.GetComponent<RawImage>();
        this.colorList = GameObject.FindObjectOfType<BrickColorList>();
    }

    public void updateDot(WebCamTexture webcamTexture, RawImage img) {
        Vector2 v;
        Util.getPositionOnImage01(img, this.transform.position, out v);
        int xpos = (int)(webcamTexture.width * v.x);
        int ypos = (int)(webcamTexture.height * v.y);

        Color averageColor = Util.getAverageColor(webcamTexture, xpos, ypos, 6);

        this.tokenColor = this.colorList.getTokenFromColor(averageColor);

        // Update ui color.
        this.img.color = this.colorList.colorFromToken(this.tokenColor);
    }

    public BrickColor getToken() {
        return this.tokenColor;
    }
}
