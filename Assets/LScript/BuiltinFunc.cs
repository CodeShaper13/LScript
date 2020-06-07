using System;
using UnityEngine;

namespace LScript {

    public class Builtin {


        #region Builtin Functions

        public static readonly BuiltinFunc add = new BuiltinFunc((Token in1Loc, Token in2Loc, Token outLoc) => {
            setMember(outLoc, new Variable.Integer(
                getMember(in1Loc).evalInt() +
                getMember(in2Loc).evalInt()));
        });

        public static readonly BuiltinFunc subtract = new BuiltinFunc((Token in1Loc, Token in2Loc, Token outLoc) => {
            setMember(outLoc, new Variable.Integer(
                getMember(in1Loc).evalInt() -
                getMember(in2Loc).evalInt()));
        });

        public static readonly BuiltinFunc multiply = new BuiltinFunc((Token in1Loc, Token in2Loc, Token outLoc) => {
            setMember(outLoc, new Variable.Integer(
                getMember(in1Loc).evalInt() *
                getMember(in2Loc).evalInt()));
        });

        public static readonly BuiltinFunc divide = new BuiltinFunc((Token in1Loc, Token in2Loc, Token outLoc) => {
            setMember(outLoc, new Variable.Integer(
                getMember(in1Loc).evalInt() /
                getMember(in2Loc).evalInt()));
        });

        public static readonly BuiltinFunc increment = new BuiltinFunc((Token inLoc, Token outLoc) => {
            setMember(outLoc, new Variable.Integer(
                getMember(inLoc).evalInt() + 1));
        });

        public static readonly BuiltinFunc decrement = new BuiltinFunc((Token inLoc, Token outLoc) => {
            setMember(outLoc, new Variable.Integer(
                getMember(inLoc).evalInt() - 1));
        });

        public static readonly BuiltinFunc print = new BuiltinFunc((Token token) => {
            int address = BlockConverter.tokenToNumber(token);

            string output = Interpreter.memory[address].evalString();

            if(Application.isPlaying) {
                OutputConsole console = GameObject.FindObjectOfType<OutputConsole>();
                console.log(output);
            } else {
                Debug.Log(output);
            }
        });

        public static readonly BuiltinFunc input = new BuiltinFunc((Token outputLoc) => {
            // TODO show dialog box
        });

        #endregion

        public static BuiltinFunc getFunctionFromToken(Token token) {
            BrickColor first = token.first;
            BrickColor second = token.second;

            if(first == BrickColor.GREEN) {
                // Math function.
                switch(second) {
                    case BrickColor.RED:
                        return add;
                    case BrickColor.GREEN:
                        return subtract;
                    case BrickColor.BLUE:
                        return multiply;
                    case BrickColor.YELLOW:
                        return divide;
                    case BrickColor.WHITE:
                        return increment;
                    case BrickColor.BLACK:
                        return decrement;
                }
            } else if(first == BrickColor.YELLOW) {
                // Standard function.
                switch(second) {
                    case BrickColor.WHITE:
                        return print;
                }
            }

            return null;
        }

        private static IMember getMember(Token token) {
            return Interpreter.memory[BlockConverter.tokenToNumber(token)];
        }

        private static void setMember(Token token, IMember member) {
            Interpreter.memory[BlockConverter.tokenToNumber(token)] = member;
        }

        public class BuiltinFunc {

            private readonly Action<Token> func1arg;
            private readonly Action<Token, Token> func2arg;
            private readonly Action<Token, Token, Token> func3arg;
            public readonly int paramCount;

            private BuiltinFunc(int paramCount) {
                this.paramCount = paramCount;
            }

            public BuiltinFunc(Action<Token> function) : this(1) {
                this.func1arg = function;
            }

            public BuiltinFunc(Action<Token, Token> function) : this(2) {
                this.func2arg = function;
            }

            public BuiltinFunc(Action<Token, Token, Token> function) : this(3) {
                this.func3arg = function;
            }

            public void invoke(params Token[] pars) {
                //Debug.Log("invoking func");
                //Debug.Log(this.func1arg);
                //Debug.Log(this.func2arg);
                //Debug.Log(this.func3arg);

                if(this.func1arg != null) {
                    this.func1arg.Invoke(pars[0]);
                }
                else if(this.func2arg != null) {
                    this.func2arg.Invoke(pars[0], pars[1]);
                }
                else if(this.func3arg != null) {
                    this.func3arg.Invoke(pars[0], pars[1], pars[2]);
                }
            }
        }
    }
}
