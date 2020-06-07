using LScript;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour {

    [SerializeField]
    private Text textInfo = null;
    [SerializeField]
    private Dots dots = null;
    [SerializeField]
    private WebCamImage camImg = null;
    [SerializeField]
    private OutputConsole console = null;

    private string lastScannedScript = "scripts/script1.ls"; 

    private void Update() {
        WebCamImage.CamState state = this.camImg.state;
        switch(state) {
            case WebCamImage.CamState.NO_AUTHORIZATION:
                this.textInfo.text = "Not Authorization To Use Camera...";
                break;
            case WebCamImage.CamState.NO_CAMERAS:
                this.textInfo.text = "No Cameras Found On Device";
                break;
            case WebCamImage.CamState.OK:
                this.textInfo.text = string.Empty;
                break;
        }

        if(this.camImg.state == WebCamImage.CamState.OK && this.camImg.playing) {
            this.dots.updateDots(this.camImg.webcamTexture, this.camImg.rawImage);
        }
    }

    public void callback_capture() {
        Directory.CreateDirectory("scripts/");

        string path = "scripts/script1.ls";
        StreamWriter writer = new StreamWriter(path, false);
        foreach(PreviewDot dot in this.dots.getDots()) {
            writer.Write((int)dot.getToken());
        }
        writer.Close();

        this.lastScannedScript = path;

        this.console.log("Script scanned successfully");
    }

    public void callback_nextCamera() {
        int i = this.camImg.cameraIndex + 1;
        if(i >= WebCamTexture.devices.Length) {
            i = 0;
        }

        this.camImg.cameraIndex = i;
    }

    public void callback_runScript() {
        if(this.lastScannedScript == null || !File.Exists(this.lastScannedScript)) {
            this.console.log("Scan a script first");
        } else {
            // Run script
            Interpreter interpreter = new Interpreter();
            interpreter.exec(this.lastScannedScript);
        }
    }

    public void callback_clearConsole() {
        this.console.clear();
    }
}