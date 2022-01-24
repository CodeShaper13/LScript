using System.Collections;
using UnityEngine;

public static class ColorExtensions {

    public static float GetHue(this Color color) {
        Color.RGBToHSV(
            color,
            out float hue,
            out float saturation,
            out float v);
        return hue;
    }

    public static float GetSaturation(this Color color) {
        Color.RGBToHSV(
            color,
            out float hue,
            out float saturation,
            out float v);
        return saturation;
    }
}