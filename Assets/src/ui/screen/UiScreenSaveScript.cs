using LScript;
using NaughtyAttributes;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class UiScreenSaveScript : UiScreen {

    [SerializeField, Required]
    private ScriptPreview _scriptPreview = null;

    private List<BrickColor> bricks;

    public void callback_run() {
        UiScreenConsole ui = UiScreen.getUi<UiScreenConsole>();
        ui.setScript(this.bricks);
        ui.openAdditively();
        ui.runScript();
    }

    public void callback_save() {
        if(this.bricks != null) {
            // Make directory if it doesn't exist.
            Directory.CreateDirectory(Main.SCRIPT_DIRECTORY);

            string path = Main.SCRIPT_DIRECTORY + this.getAvailableScriptName();

            StreamWriter writer = new StreamWriter(path, false);
            foreach(BrickColor brick in this.bricks) {
                writer.Write((int)brick);
            }
            writer.Close();

            Debug.LogFormat("Saved Script as {0}", path);

            UiScreen.getUi<UiScreenScriptList>().open();
        }
    }

    public void setScript(List<BrickColor> bricks) {
        this.bricks = bricks;

        this._scriptPreview.setBricks(bricks);
    }

    private string getAvailableScriptName() {
        for(int i = 1; i < int.MaxValue; i++) {
            string filePath = "Script" + i + Main.SCRIPT_EXTENSION;
            if(!File.Exists(Main.SCRIPT_DIRECTORY + filePath)) {
                return filePath;
            }
        }

        return "Script0" + Main.SCRIPT_EXTENSION;
    }
}