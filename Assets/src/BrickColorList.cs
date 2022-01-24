using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using LScript;
using NaughtyAttributes;

public class BrickColorList : MonoBehaviour {

    [SerializeField]
    private EnumCompareMode _compareMode = EnumCompareMode.RGB;
    [SerializeField, ReorderableList]
    private List<Color> allColors = new List<Color>();

    /// <summary>
    /// Returns the RGB color of the passed BrickColor.
    /// </summary>
    public Color brickToRgb(BrickColor brick) {
        return this.allColors[(int)brick];
    }

    public BrickColor RGBToBrickColor(Color target) {
        switch(this._compareMode) {
            case EnumCompareMode.RGB:
                float colorDiffs = this.allColors.Select(n => this.getColorDiffInRGB(n, target)).Min(n => n);
                return (BrickColor)this.allColors.FindIndex(n => this.getColorDiffInRGB(n, target) == colorDiffs);
            case EnumCompareMode.HSV:
                float distance = float.PositiveInfinity;
                int match = 0;
                for(int i= 0; i < this.allColors.Count; i++) {
                    Color color = this.allColors[i];

                    Color.RGBToHSV(target, out float h1, out float s1, out float v1);
                    Color.RGBToHSV(color, out float h2, out float s2, out float v2);

                    h1 *= (Mathf.PI * 2);
                    h2 *= (Mathf.PI * 2);

                    float f =
                        Mathf.Pow((Mathf.Sin(h1) * s1 * v1 - Mathf.Sin(h2) * s2 * v2), 2) +
                        Mathf.Pow((Mathf.Cos(h1) * s1 * v1 - Mathf.Cos(h2) * s2 * v2), 2) +
                        Mathf.Pow((v1 - v2), 2);

                    if(f < distance) {
                        match = i;
                        distance = f;
                    }
                }
                return (BrickColor)match;

            case EnumCompareMode.H_ONLY:
                return (BrickColor)this.closestColor1(this.allColors, target);
        }

        return BrickColor.RED;
    }

    // closed match for hues only:
    private int closestColor1(List<Color> colors, Color target) {
        float hue1 = target.GetHue();
        var diffs = colors.Select(n => getHueDistance(n.GetHue(), hue1));
        var diffMin = diffs.Min(n => n);
        return diffs.ToList().FindIndex(n => n == diffMin);
    }

    // distance between two hues:
    private float getHueDistance(float hue1, float hue2) {
        float d = Mathf.Abs(hue1 - hue2);
        return d > 180 ? 360 - d : d;
    }

    /// <summary>
    /// Returns the difference between the two colors in RGB space.
    /// </summary>
    private float getColorDiffInRGB(Color c1, Color c2) {
        return Mathf.Sqrt(
            (c1.r - c2.r) * (c1.r - c2.r) +
            (c1.g - c2.g) * (c1.g - c2.g) +
            (c1.b - c2.b) * (c1.b - c2.b));
    }

    private enum EnumCompareMode {
        RGB = 0,
        HSV = 1,
        H_ONLY = 2,
    }
}
