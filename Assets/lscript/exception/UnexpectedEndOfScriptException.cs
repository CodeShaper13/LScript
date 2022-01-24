using System;

namespace LScript {
    
    /// <summary>
    /// Thrown when a script ends unexpectedly.  this happens when a
    /// variable is defined, but there is no end to the definition.
    /// </summary>
    public class UnexpectedEndOfScriptException : Exception {

        public UnexpectedEndOfScriptException() : base("Script ended unexpectedly") { }
    }
}