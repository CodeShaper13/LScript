using NaughtyAttributes;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class UiScreenScriptList : UiScreen {

    [SerializeField, Required]
    private GameObject _prefabListEntry = null;
    [SerializeField, Required]
    private RectTransform _entryParent = null;

    protected override void onOpen() {
        base.onOpen();

        foreach(string path in Directory.GetFiles(Main.SCRIPT_DIRECTORY, "*" + Main.SCRIPT_EXTENSION)) {
            GameObject go = GameObject.Instantiate(this._prefabListEntry, this._entryParent);
            go.GetComponentInChildren<Button>().onClick.AddListener(() => {
                UiScreenScriptInfo ui = UiScreen.getUi<UiScreenScriptInfo>();
                ui.setScriptPath(path);
                ui.open();
            });
            go.GetComponentInChildren<Text>().text = Path.GetFileNameWithoutExtension(path);
        }
    }

    protected override void onClose() {
        base.onClose();

        foreach(Transform t in this._entryParent) {
            GameObject.Destroy(t.gameObject);
        }
    }
}