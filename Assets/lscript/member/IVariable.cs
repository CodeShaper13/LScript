namespace LScript {

    public interface IVariable {

        /// <summary>
        /// Evaluates the variable to an integer.
        /// </summary>
        int evalInt(Interpreter interpreter);

        /// <summary>
        /// Evaluates the variable to a string.
        /// </summary>
        string evalString(Interpreter interpreter);
    }
}
