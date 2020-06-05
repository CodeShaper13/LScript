using System;

namespace LScript {

    public class InvalidFileException : Exception {

        public InvalidFileException(string reason) : base(reason) {

        }
    }
}
