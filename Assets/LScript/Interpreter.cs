using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LScript {

    public class Interpreter {

        public readonly Token TOKEN_DEFINE_END_V = new Token(BrickColor.RED, BrickColor.RED);
        public readonly Token TOKEN_DEFINE_STRING = new Token(BrickColor.RED, BrickColor.GREEN);
        public readonly Token TOKEN_DEFINE_INTEGER = new Token(BrickColor.RED, BrickColor.BLUE);
        public readonly Token TOKEN_DEFINE_FUNCTION = new Token(BrickColor.RED, BrickColor.YELLOW);
        public readonly Token TOKEN_DEFINE_END_F = new Token(BrickColor.RED, BrickColor.WHITE);

        public readonly Token TOKEN_MATH_ADD = new Token(BrickColor.GREEN, BrickColor.RED);
        public readonly Token TOKEN_MATH_SUB = new Token(BrickColor.GREEN, BrickColor.GREEN);
        public readonly Token TOKEN_MATH_MUL = new Token(BrickColor.GREEN, BrickColor.BLUE);
        public readonly Token TOKEN_MATH_DIV = new Token(BrickColor.GREEN, BrickColor.YELLOW);
        public readonly Token TOKEN_MATH_REM = new Token(BrickColor.GREEN, BrickColor.TAN);
        public readonly Token TOKEN_MATH_INC = new Token(BrickColor.GREEN, BrickColor.WHITE);
        public readonly Token TOKEN_MATH_DEC = new Token(BrickColor.GREEN, BrickColor.BLUE);

        public readonly Token TOKEN_KW_PRINT = new Token(BrickColor.BLUE, BrickColor.RED);
        public readonly Token TOKEN_KW_INPUT = new Token(BrickColor.BLUE, BrickColor.GREEN);
        public readonly Token TOKEN_KW_TERMINATE = new Token(BrickColor.BLUE, BrickColor.BLUE);
        public readonly Token TOKEN_KW_STARTCALL = new Token(BrickColor.BLUE, BrickColor.WHITE);
        public readonly Token TOKEN_KW_ENDCALL = new Token(BrickColor.BLUE, BrickColor.BLACK);
        public readonly Token TOKEN_KW_RETURN = new Token(BrickColor.BLUE, BrickColor.YELLOW);

        public readonly Token TOKEN_BLANK = new Token(BrickColor.WHITE, BrickColor.WHITE);



        private IVariable[] memory;
        private IStream outputStream;

        public Interpreter(IStream outputStream) {
            this.memory = new IVariable[100];
            this.outputStream = outputStream;

            // Fill the "ram" with 0s.
            for(int i = 0; i < this.memory.Length; i++) {
                this.memory[i] = new LScriptInt(0);
            }
        }

        /// <summary>
        /// Reads a file and "compiles" it into a script (array of Tokens).  The Token array is then returned.
        /// </summary>
        public Token[] compileScript(string filePath) {
            // Read the file.
            string text = File.ReadAllText(filePath);

            // Validate that the file has an even number of numbers in it.
            if(text.Length % 2 != 0) {
                throw new Exception("File does not have an even number of integers in it.");
            }

            // Convert file contents to an array of numbers.
            int[] numbers = new int[text.Length];
            for(int i = 0; i < text.Length; i++) {
                string s = text[i].ToString();
                if(Int32.TryParse(s, out int j)) {
                    numbers[i] = j;
                } else {
                    throw new Exception(string.Format("A non numerical character was found in file \"{0}\", this is not allowed.", s));
                }
            }

            // Convert the array of numbers to an array of Tokens.
            Token[] tokens = new Token[numbers.Length / 2];
            for(int i = 0; i < tokens.Length; i++) {
                int index = i * 2;
                tokens[i] = new Token((BrickColor)numbers[index], (BrickColor)numbers[index + 1]);
            }

            return tokens;
        }

        /// <summary>
        /// Executes the passed array of Tokens as a script.  If the
        /// script returns a number (which is treated as a pointer),
        /// it is returned.  Otherwise null is returned.
        /// </summary>
        public Token? execute(Token[] tokens) {
            // masterPointer points to the current token being evaluated.
            int masterPointer = 0;

            while(masterPointer < tokens.Length) {
                Token current = tokens[masterPointer];

                // Red (Define)
                if(current == TOKEN_DEFINE_END_V) {
                    throw new InvalidTokenException(string.Format("end_definition \"{0}\" at {1} without any start_definition token", current, masterPointer), current);
                }
                else if(current == TOKEN_DEFINE_STRING) {
                    Token[] args = this.fetchBlock(ref masterPointer, tokens, TOKEN_DEFINE_END_V);
                    this.setMember(
                        args[0],
                        new LScriptString(args.Skip(1).ToArray()));
                }
                else if(current == TOKEN_DEFINE_INTEGER) {
                    Token[] args = this.fetchBlock(ref masterPointer, tokens, TOKEN_DEFINE_END_V);
                    this.setMember(
                        args[0],
                        new LScriptInt(args.Skip(1).ToArray()));
                }
                else if(current == TOKEN_DEFINE_FUNCTION) {
                    Token[] args = this.fetchBlock(ref masterPointer, tokens, TOKEN_DEFINE_END_F);
                    Token pointer = args[0];
                    args = args.Skip(1).ToArray();

                    if(args.Contains(TOKEN_DEFINE_FUNCTION)) {
                        throw new InvalidTokenException(string.Format("define_function \"{0}\" at {1} is not allowed within another function.", current, masterPointer), current);
                    }

                    this.setMember(
                        pointer,
                        new LScriptFunction(args));
                }

                // Green (Math)
                else if(current == TOKEN_MATH_ADD) {
                    Token[] args = this.fetchBlock(ref masterPointer, tokens, 3);
                    this.func_mathHelp(args, (i, j) => { return i + j; });
                }
                else if(current == TOKEN_MATH_SUB) {
                    Token[] args = this.fetchBlock(ref masterPointer, tokens, 3);
                    this.func_mathHelp(args, (i, j) => { return i - j; } );
                }
                else if(current == TOKEN_MATH_MUL) {
                    Token[] args = this.fetchBlock(ref masterPointer, tokens, 3);
                    this.func_mathHelp(args, (i, j) => { return i * j; });
                }
                else if(current == TOKEN_MATH_DIV) {
                    Token[] args = this.fetchBlock(ref masterPointer, tokens, 3);
                    this.func_mathHelp(args, (i, j) => { return i / j; });
                }
                else if(current == TOKEN_MATH_REM) {
                    Token[] args = this.fetchBlock(ref masterPointer, tokens, 3);
                    this.func_mathHelp(args, (i, j) => { return i % j; });
                }
                else if(current == TOKEN_MATH_INC) {
                    Token[] args = this.fetchBlock(ref masterPointer, tokens, 1);
                    this.setMember(args[0], new LScriptInt(
                        this.getMember(args[0]).evalInt(this) + 1));
                }
                else if(current == TOKEN_MATH_DEC) {
                    Token[] args = this.fetchBlock(ref masterPointer, tokens, 1);
                    this.setMember(args[0], new LScriptInt(
                        this.getMember(args[0]).evalInt(this) - 1));
                }

                // Blue (Keywords)
                else if(current == TOKEN_KW_PRINT) {
                    Token[] args = this.fetchBlock(ref masterPointer, tokens, 1);

                    int address = TokenConverter.tokenToNumber(args[0]);
                    string output = this.memory[address].evalString(this);

                    if(this.outputStream != null) {
                        this.outputStream.write(output);
                    }
                }
                else if(current == TOKEN_KW_INPUT) {
                    // TODO
                    UiScreenInput ui = UiScreen.getUi<UiScreenInput>();
                }
                else if(current == TOKEN_KW_TERMINATE) {
                    throw new ScriptTerminationException();
                }
                else if(current == TOKEN_KW_STARTCALL) {
                    Token[] args = this.fetchBlock(ref masterPointer, tokens, TOKEN_KW_ENDCALL);

                    Token pointer = args[0];
                    IVariable member = this.getMember(pointer);
                    if(member is LScriptFunction function) {
                        function.execute(this);
                    } else {
                        throw new Exception(string.Format(
                            "start_function_call \"{0}\" at {1} is trying to invoke a non-function variable at address {3} ({2})",
                            current,
                            masterPointer,
                            TokenConverter.tokenToNumber(pointer),
                            pointer));
                    }
                }
                else if(current == TOKEN_KW_ENDCALL) {
                    throw new InvalidTokenException(string.Format(
                        "end_function_call token \"{0}\" at {1} found without a start_function_call \"{2}\"",
                        TOKEN_KW_ENDCALL,
                        masterPointer,
                        TOKEN_KW_STARTCALL), TOKEN_KW_ENDCALL);
                }
                else if(current == TOKEN_KW_RETURN) {
                    Token[] args = this.fetchBlock(ref masterPointer, tokens, 1);
                    return args[0];
                }

                else if(current == TOKEN_BLANK) {
                    // don't do anything, black space.
                }

                // Error
                else {
                    throw new InvalidTokenException(string.Format("Unexpected Token at {0} \"{1}\"", masterPointer, current), current);
                }

                masterPointer++;                
            }

            return null;
        }

        /// <summary>
        /// Returns the value of a location in memory.
        /// </summary>
        public IVariable getMember(int address) {
            return this.memory[address];
        }

        /// <summary>
        /// Returns the value of a location in memory.
        /// </summary>
        public IVariable getMember(Token tokenPointer) {
            return this.getMember(TokenConverter.tokenToNumber(tokenPointer));
        }

        /// <summary>
        /// Sets the value of a location in memory.  Throws ArgumentNullException if null is passed.
        /// </summary>
        public void setMember(int address, IVariable member) {
            if(member == null) {
                throw new ArgumentNullException("Can't set a location in memory to be null.");
            }
            this.memory[address] = member;
        }

        /// <summary>
        /// Sets the value of a location in memory.  Throws ArgumentNullException if null is passed.
        /// </summary>
        public void setMember(Token tokenPointer, IVariable member) {
            this.setMember(TokenConverter.tokenToNumber(tokenPointer), member);
        }

        private Token[] fetchBlock(ref int masterPointer, Token[] tokens, int tokensToGet) {
            Token[] block = new Token[tokensToGet];

            for(int i = 0; i < tokensToGet; i++) {
                masterPointer++;

                if(masterPointer >= tokens.Length) {
                    // Not enough room for the required pointers, error!
                    throw new UnexpectedEndOfScriptException();
                }

                block[i] = tokens[masterPointer];
            }

            return block;
        }

        private Token[] fetchBlock(ref int masterPointer, Token[] tokens, Token teminator) {
            List<Token> block = new List<Token>();

            while(true) {
                masterPointer++;

                if(masterPointer >= tokens.Length) {
                    // Not enough room for the required pointers, error!
                    throw new UnexpectedEndOfScriptException();
                }

                if(tokens[masterPointer] == teminator) {
                    break;
                }
                else {
                    block.Add(tokens[masterPointer]);
                }
            }

            return block.ToArray();
        }

        /// <summary>
        /// Helper function for doing math.  Takes in an array of
        /// (hopefull) three elements, and calls the passed Func,
        /// passing the array in.
        /// </summary>
        private void func_mathHelp(Token[] args, Func<int, int, int> func) {
            int i = this.getMember(args[0]).evalInt(this);
            int j = this.getMember(args[1]).evalInt(this);

            this.setMember(args[2], new LScriptInt(func(i, j)));
        }
    }
}