using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UiScreen : MonoBehaviour {

    private static List<UiScreen> allUis = new List<UiScreen>();

    /// <summary>
    /// The currently open UI.  May be null.
    /// </summary>
    public static List<UiScreen> openUis { get; private set; } = new List<UiScreen>();
    /// <summary>
    /// The number of ui screens that are open.
    /// </summary>
    public static int openUiCount => openUis.Count;

    /// <summary>
    /// Returns an Enumerator to loop through all of the UIs.
    /// </summary>
    public IEnumerator<UiScreen> enumerator => allUis.GetEnumerator();

    /// <summary>
    /// Looks up and returns the UI with the matching type.
    /// </summary>
    public static T getUi<T>(Predicate<T> predicate = null) where T : UiScreen {
        foreach(UiScreen screen in UiScreen.allUis) {
            if(screen is T) {
                if(predicate == null || predicate((T)screen)) {
                    return (T)screen;
                }
            }
        }

        Debug.LogWarningFormat(
            "UiScreen of type {0} could not be found!{1}",
            typeof(T),
            predicate == null ? string.Empty : " There was a predicate, this may be the problem.");

        return null;
    }



    [SerializeField, Tooltip("The key that will close the UI.")]
    private KeyCode _closeKey = KeyCode.None;

    private bool cursorStatePreOpen;
    private bool isOpen;

    protected virtual void Awake() {
        UiScreen.allUis.Add(this);

        // Disable the imediate children in case the user forgot.
        this.setChildState(false);
    }

    protected virtual void OnDestroy() {
        UiScreen.allUis.Remove(this);
    }

    private void Update() {
        if(this.isOpen) {
            this.onUpdate();
        }
    }

    private void LateUpdate() {
        if(this.isOpen) {
            this.onLateUpdate();

            if(Input.GetKeyDown(this._closeKey)) {
                this.close();
            }
        }
    }

    /// <summary>
    /// Called when the UI is shown.
    /// </summary>
    protected virtual void onOpen() {
    }

    /// <summary>
    /// Called when the UI is closed/hidden.
    /// </summary>
    protected virtual void onClose() {
    }

    /// <summary>
    /// Called every frame that the UI is open.
    /// </summary>
    protected virtual void onUpdate() {
    }

    /// <summary>
    /// Called at the end of every frame that the UI is open.
    /// </summary>
    protected virtual void onLateUpdate() {
    }

    /// <summary>
    /// Opens this UiScreen and closes the open ones (if there is one).
    /// </summary>
    public void open() {
        if(this.isOpen) {
            return;
        }

        // Close the currently open UIs.
        this.closeAll();

        this.func();
    }

    /// <summary>
    /// Opens this UiScreen additively (any open UI are not closed).
    /// </summary>
    public void openAdditively() {
        if(this.isOpen) {
            return;
        }

        this.func();
    }

    /// <summary>
    /// Closes this UiScreen.  If it is not the open UiScreen, nothing happens.
    /// </summary>
    public void close() {
        if(!this.isOpen) {
            return; // Can't close UI, it's not open.
        }

        this.setChildState(false);
        this.onClose();
        UiScreen.openUis.Remove(this);

        this.isOpen = false;
    }

    public void closeAll() {
        UiScreen.closeAllUis();
    }

    private void func() {
        // Open this UI
        this.setChildState(true);
        this.onOpen();
        UiScreen.openUis.Add(this);

        this.isOpen = true;
    }

    private void setChildState(bool active) {
        foreach(Transform child in this.transform) {
            child.gameObject.SetActive(active);
        }
    }

    public static void closeAllUis() {
        // Close the currently open UIs.
        for(int i = UiScreen.openUis.Count - 1; i >= 0; i--) {
            UiScreen.openUis[i].close();
        }
    }

    [Button("Open")]
    protected void editor_buttonOpen() {
        if(Application.isPlaying) {
            this.open();
        }
        else {
            this.setChildState(true);
        }
    }

    [Button("Close")]
    protected void editor_buttonClose() {
        if(Application.isPlaying) {
            this.close();
        }
        else {
            this.setChildState(false);
        }
    }
}
