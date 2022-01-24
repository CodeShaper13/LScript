using LScript;

public class OutputStreamApplication : IStream {

    private OutputConsole console;

    public OutputStreamApplication(OutputConsole outputConsole) {
        this.console = outputConsole;
    }

    public void write(string msg) {
        this.console.log(msg);
    }
}