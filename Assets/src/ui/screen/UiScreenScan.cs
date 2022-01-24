using LScript;
using NaughtyAttributes;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UiScreenScan : UiScreen {

    [SerializeField, Required]
    private ScanDotGrid _dots = null;

    public void callback_captureScript() {
        Directory.CreateDirectory(Main.SCRIPT_DIRECTORY);


        List<BrickColor> bricks = new List<BrickColor>();
        foreach(ScanDot dot in this._dots.dots) {
            bricks.Add(dot.brickColor);
        }

        UiScreenSaveScript ui = UiScreen.getUi<UiScreenSaveScript>();
        ui.setScript(bricks);
        ui.openAdditively();
    }
}