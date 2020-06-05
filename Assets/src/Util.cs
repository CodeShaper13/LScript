using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public static class Util {

    /// <summary>
    /// Samples the average color at the passed position on the passed WebCamTexture.
    /// </summary>
    public static Color getAverageColor(WebCamTexture texture, int xPos, int yPos, int sampleRadius) {
        Color c = new Color(0, 0, 0);

        int pixelsSampled = 0;
        for(int x = -sampleRadius; x <= sampleRadius; x++) {
            for(int y = -sampleRadius; y <= sampleRadius; y++) {
                c += texture.GetPixel(xPos + x, yPos + y);
                pixelsSampled++;
            }
        }

        return c / pixelsSampled;
    }

    /// <summary>
    /// Take an image and a screenPosition. Return the ptLocationRelativeToImage01
    /// relative to the image (where x,y between 0.0 and 1.0 are on the image, values below 0.0 or above 1.0 are outside the image).
    /// </summary>
    public static bool getPositionOnImage01(RawImage rawImage, Vector2 screenPosition, out Vector2 ptLocationRelativeToImage01) {
        ptLocationRelativeToImage01 = new Vector2();
        RectTransform uiImageObjectRect = rawImage.GetComponent<RectTransform>();
        Vector2 localCursor = new Vector2();
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(
            uiImageObjectRect,
            screenPosition,
            null,
            out localCursor)) {
            Vector2 ptPivotCancelledLocation = new Vector2(localCursor.x - uiImageObjectRect.rect.x, localCursor.y - uiImageObjectRect.rect.y);
            Vector2 ptLocationRelativeToImageInScreenCoordinates = new Vector2();
            // How do we get the location of the image? Calculate the size of the image, then use the pivot information.

            ptLocationRelativeToImageInScreenCoordinates.Set(ptPivotCancelledLocation.x, ptPivotCancelledLocation.y);
            ptLocationRelativeToImage01.Set(ptLocationRelativeToImageInScreenCoordinates.x / uiImageObjectRect.rect.width, ptLocationRelativeToImageInScreenCoordinates.y / uiImageObjectRect.rect.height);

            return true;
        }
        return false;
    }
}
