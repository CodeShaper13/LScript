using UnityEngine;
using LScript;
using System.IO;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DebugRunner : MonoBehaviour {

    #if UNITY_EDITOR
    [CustomEditor(typeof(DebugRunner))]
    class DecalMeshHelperEditor : Editor {

        public override void OnInspectorGUI() {
            if(GUILayout.Button("Run scripts/script1.ls")) {
                Interpreter interpreter = new Interpreter();
                interpreter.exec("scripts/script1.ls");
            }

            if(GUILayout.Button("Generate Mappings")) {
                StreamWriter w = File.CreateText("mappings.txt");

                for(int i = 0; i < BlockConverter.characters.Length; i++) {

                    int j1 = getIndividualDigits(i, 0);
                    int j2 = getIndividualDigits(i, 1);
                    w.WriteLine(
                        BlockConverter.characters[i] +
                        "        =    " +
                        (BrickColor)j1 +
                        " " +
                        (BrickColor)j2);
                }

                w.Close();
            }
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
            } else {
                return listOfInts[digit];
            }
        }
    }
    #endif
}
