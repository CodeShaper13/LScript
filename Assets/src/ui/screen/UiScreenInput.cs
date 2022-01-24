using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

// Not implemented, yet...
public class UiScreenInput : UiScreen {

    [SerializeField, Required]
    private InputField _input = null;

    public string result { get; private set; }

    protected override void onOpen() {
        base.onOpen();

        this.result = null;
    }

    public void callback_submit() {
        this.result = this._input.text;
    }
}