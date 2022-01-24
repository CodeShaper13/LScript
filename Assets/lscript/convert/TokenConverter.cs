namespace LScript {

    /// <summary>
    /// A collection of static methods to help converting Token to data.
    /// </summary>
    public static class TokenConverter {

        public static readonly char[] characters = new char[] {
            // Numbers:
            '0',
            '1',
            '2',
            '3',
            '4',
            '5',
            '6',
            '7',
            '8',
            '9',
            // Letter Lower Case
            'a',
            'b',
            'c',
            'd',
            'e',
            'f',
            'g',
            'h',
            'i',
            'j',
            'k',
            'l',
            'm',
            'n',
            'o',
            'p',
            'q',
            'r',
            's',
            't',
            'u',
            'v',
            'w',
            'x',
            'y',
            'z',
            // Upper Case
            'A',
            'B',
            'C',
            'D',
            'E',
            'F',
            'G',
            'H',
            'I',
            'J',
            'K',
            'L',
            'M',
            'N',
            'O',
            'P',
            'Q',
            'R',
            'S',
            'T',
            'U',
            'V',
            'W',
            'X',
            'Y',
            'Z',
            // Space
            ' ',
            // Punctuation
            '.',
            ',',
            '!',
            '?',
            ':',
            ';',
            '\'',
            '"',
            // Math
            '-',
            '+',
            '*',
            '/',
            '\\',
            '<',
            '>',
            // Braces
            '(',
            ')',
            '[',
            ']',
            '{',
            '}',
            // Symbols
            '@',
            '#',
            '$',
            '%',
            '&',
            // Other
            '`',
            '~',
            '^',
            // Escape Characters
            '\t',
            '\n',
        };

        public static int brickToNumber(BrickColor color) {
            return (int)color;
        }

        /// <summary>
        /// Converts the passed Token to an integer.
        /// </summary>
        public static int tokenToNumber(Token token) {
            int n1 = brickToNumber(token.first);
            int n2 = brickToNumber(token.second);

            return n1 * 10 + n2;
        }

        /// <summary>
        /// Converts the passed Token to a character and returns it.
        /// </summary>
        public static char tokenToChar(Token token) {
            int index = TokenConverter.tokenToNumber(token);

            if(index < 0 || index >= characters.Length) {
                throw new InvalidTokenException(string.Format("Could not convert {0} to a character.", token), token);
            }

            return characters[index];
        }
    }
}
