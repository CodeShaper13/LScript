using LScript;
using UnityEngine;

public class OutputStreamEditor : IStream {

    public void write(string msg) {
        Debug.Log(msg);
    }
}