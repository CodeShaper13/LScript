using UnityEngine;
using LScript;
using System.IO;
using System.Collections.Generic;
using NaughtyAttributes;

public class DebugRunner : MonoBehaviour {

    [SerializeField, Dropdown("__getFileNames")]
    public string _fileName = string.Empty;

    [Button]
    public void runScipt() {
        if(string.IsNullOrWhiteSpace(this._fileName)) {
            Debug.LogError("Select a script in the inspector!");
        } else {
            Interpreter interpreter = new Interpreter(new OutputStreamEditor());
            Token[] tokens = interpreter.compileScript(Main.SCRIPT_DIRECTORY + this._fileName + Main.SCRIPT_EXTENSION);
            interpreter.execute(tokens);
        }
    }

    /// <summary>
    /// Used to auto generate some of the text for the github page.
    /// </summary>
    [Button]
    public void generateMappings() {
        StreamWriter w = File.CreateText("mappings.txt");

        for(int i = 0; i < TokenConverter.characters.Length; i++) {
            w.WriteLine(string.Format(
                "{0} = {1} {2}",
                TokenConverter.characters[i],
                (BrickColor)this.getIndividualDigits(i, 0),
                (BrickColor)this.getIndividualDigits(i, 1)));
        }

        w.Close();
    }

    private int getIndividualDigits(int num, int digit) {
        List<int> listOfInts = new List<int>();
        while(num > 0) {
            listOfInts.Add(num % 10);
            num = num / 10;
        }
        listOfInts.Reverse();

        if(digit >= listOfInts.Count) {
            return 0;
        }
        else {
            return listOfInts[digit];
        }
    }

    // Used by the Naughty Attributes custom inspector.
    private DropdownList<string> __getFileNames() {

        string[] files = Directory.GetFiles(Main.SCRIPT_DIRECTORY, "*" + Main.SCRIPT_EXTENSION);
        if(files.Length == 0) {
            return new DropdownList<string>() { { "No scripts found!  Create a script in /scripts first.", string.Empty } };
        } else {
            DropdownList<string> strings = new DropdownList<string>();
            foreach(string path in files) {
                strings.Add(Path.GetFileNameWithoutExtension(path), Path.GetFileNameWithoutExtension(path));
            }
            return strings;
        }
    }
}
