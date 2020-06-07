using System;

namespace LScript {

        public static class BlockConverter {

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

        public static int blockToNumber(BrickColor color) {
            return (int)color;
        }

        public static int tokenToNumber(Token token) {
            int n1 = blockToNumber(token.first);
            int n2 = blockToNumber(token.second);

            return n1 * 10 + n2;
        }

        public static char tokenToChar(Token token) {
            int index = BlockConverter.tokenToNumber(token);

            if(index < 0 || index >= characters.Length) {
                throw new Exception("Index out of range");
            }

            return characters[index];
        }
    }
}
