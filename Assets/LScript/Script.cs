using UnityEngine;
using System.Collections;

public class Script {

    private LSToken[] tokens;

    public Script(LSToken[] tokens) {
        this.tokens = tokens;
    }

    public int getTokenCount() {
        return this.tokens.Length;
    }
}
