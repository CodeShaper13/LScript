using System;
using System.Text;

namespace LScript {

    public class LScriptString : IVariable  {

        private string value;

        public LScriptString(string s) {
            this.value = s;
        }

        public LScriptString(Token[] tokens) {
            StringBuilder sb = new StringBuilder();
            foreach(Token t in tokens) {
                sb.Append(TokenConverter.tokenToChar(t));
            }
            this.value = sb.ToString();
        }

        public int evalInt(Interpreter interpreter) {
            throw new InvalidCastException("Can't treat a string as an integer");
        }

        public string evalString(Interpreter interpreter) {
            return this.value;
        }
    }
}