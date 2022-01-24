using UnityEngine;

public class Main : MonoBehaviour {

    public const string SCRIPT_EXTENSION = ".ls";
    public const string SCRIPT_DIRECTORY = "scripts/";

    private void Start() {
        UiScreen.getUi<UiScreenScan>().open();
    }
}