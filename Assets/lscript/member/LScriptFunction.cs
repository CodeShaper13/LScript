namespace LScript {

    public class LScriptFunction : IVariable {

        /// <summary>
        /// The tokens making up the logic of the method.
        /// </summary>
        private readonly Token[] body;

        public LScriptFunction(Token[] contents) {
            this.body = contents;
        }

        public int evalInt(Interpreter interpreter) {
            return this.func(interpreter, new LScriptInt(0)).evalInt(interpreter);
        }

        public string evalString(Interpreter interpreter) {
            return this.func(interpreter, new LScriptString("0")).evalString(interpreter);
        }

        /// <summary>
        /// Executes the function.  This is for functions that don't return a value.
        /// </summary>
        public void execute(Interpreter interpreter) {
            interpreter.execute(this.body);
        }

        private IVariable func(Interpreter interpreter, IVariable defaultValue) {
            Token? returnValue = interpreter.execute(this.body);

            if(returnValue == null) {
                return defaultValue;
            } else {
                return interpreter.getMember((Token)returnValue);
            }
        }
    }
}
