using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;
using System.IO;
using System;
using LScript;
using System.Collections.Generic;

public class UiScreenScriptInfo : UiScreen {

    [SerializeField, Required]
    private Text _text = null;
    [SerializeField, Required]
    private ScriptPreview _scriptPreview = null;

    private string path;
    private List<BrickColor> bricks;

    public void setScriptPath(string path) {
        this.path = path;


        this._text.text = "Script: " + Path.GetFileNameWithoutExtension(path);


        this.bricks = new List<BrickColor>();

        foreach(char c in File.ReadAllText(path)) {
            int j;
            if(Int32.TryParse(c.ToString(), out j)) {
                this.bricks.Add((BrickColor)j);
            } else {
                Debug.LogWarning("A non numerical character was found in file, this is not allowed!");
                return;
            }
        }


        this._scriptPreview.setBricks(this.bricks);
    }

    public void callback_run() {
        UiScreenConsole ui = UiScreen.getUi<UiScreenConsole>();
        ui.setScript(this.bricks);
        ui.openAdditively();
        ui.runScript();
    }

    public void callback_delete() {
        File.Delete(this.path); // TODO maybe add a confirm message.

        UiScreen.getUi<UiScreenScriptList>().open();
    }
}