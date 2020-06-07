using System.Collections.Generic;
using System.Text;

namespace LScript {

    public abstract class Variable : IMember {

        public abstract int evalInt();

        public abstract string evalString();

        public class Integer : Variable {

            private int value;

            public Integer(int i) {
                this.value = i;
            }

            public Integer(List<Token> tokens) {
                string s = "";
                foreach(Token t in tokens) {
                    s += BlockConverter.blockToNumber(t.first).ToString();
                    s += BlockConverter.blockToNumber(t.second).ToString();
                }

                this.value = int.Parse(s);
            }

            public override int evalInt() {
                return this.value;
            }

            public override string evalString() {
                return this.value.ToString();
            }
        }

        public class String : Variable {

            private string value;

            public String(string s) {
                this.value = s;
            }

            public String(List<Token> tokens) {
                StringBuilder sb = new StringBuilder();
                foreach(Token t in tokens) {
                    sb.Append(BlockConverter.tokenToChar(t));
                }
                this.value = sb.ToString();
            }

            public override int evalInt() {
                throw new System.NotImplementedException();
            }

            public override string evalString() {
                return value;
            }
        }
    }
}
