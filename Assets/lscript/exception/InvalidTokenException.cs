using System;

namespace LScript {

    /// <summary>
    /// Thrown when a Token is encountered that is unexpected.
    /// </summary>
    public class InvalidTokenException : Exception {

        /// <summary>
        /// The token that caused the exception to be thrown.
        /// </summary>
        public Token token;

        public InvalidTokenException(string message, Token unexpectedToken) : base(message) {
            this.token = unexpectedToken;
        }
    }
}