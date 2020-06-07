using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class OutputConsole : MonoBehaviour {

    [SerializeField]
    private Text consoleText = null;

    private StringBuilder sb;

    private void Awake() {
        this.sb = new StringBuilder();

        this.updateTextComponent();
    }

    public void log(string s) {
        this.sb.AppendLine(s);

        this.updateTextComponent();
    }

    public void log(char c) {
        this.sb.AppendLine(c.ToString());

        this.updateTextComponent();
    }

    /// <summary>
    /// Clears the console.
    /// </summary>
    public void clear() {
        this.sb.Clear();

        this.updateTextComponent();
    }

    private void updateTextComponent() {
        this.consoleText.text = this.sb.ToString();
    }
}
