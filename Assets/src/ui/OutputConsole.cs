using NaughtyAttributes;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class OutputConsole : MonoBehaviour {

    [SerializeField, Required]
    private Text consoleText = null;

    private StringBuilder sb = new StringBuilder();

    private void Awake() {
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
