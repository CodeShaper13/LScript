using LScript;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;

public class UiScreenConsole : UiScreen {

    [SerializeField, Required]
    private OutputConsole _outputConsole = null;

    private Token[] script;

    protected override void onOpen() {
        base.onOpen();

        this._outputConsole.clear();
    }

    public void callback_runScript() {
        this.runScript();
    }

    public void setScript(List<BrickColor> bricks) {
        // Compile script.
        int length = bricks.Count / 2;

        this.script = new Token[length];

        for(int i = 0; i < length; i++) {
            this.script[i] = new Token(
                bricks[i * 2],
                bricks[(i * 2) + 1]);
        }
    }

    public void runScript() {
        if(this.script != null) {
            Interpreter interpreter = new Interpreter(new OutputStreamApplication(this._outputConsole));
            try {
                interpreter.execute(this.script);
            } catch (ScriptTerminationException) {
                this._outputConsole.log("Script Ran Successfully");
            } catch (Exception e) {
                this._outputConsole.log(e.Message);
                print(e.StackTrace);
            }
        } else {
            Debug.LogWarning("No script set!");
        }
    }
}