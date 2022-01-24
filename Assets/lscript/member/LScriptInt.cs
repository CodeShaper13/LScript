namespace LScript {

    public class LScriptInt : IVariable {

        private int value;

        public LScriptInt(int i) {
            this.value = i;
        }

        public LScriptInt(Token[] tokens) {
            string s = "";
            foreach(Token t in tokens) {
                s += TokenConverter.brickToNumber(t.first).ToString();
                s += TokenConverter.brickToNumber(t.second).ToString();
            }

            this.value = int.Parse(s);
        }

        public int evalInt(Interpreter interpreter) {
            return this.value;
        }

        public string evalString(Interpreter interpreter) {
            return this.value.ToString();
        }
    }
}