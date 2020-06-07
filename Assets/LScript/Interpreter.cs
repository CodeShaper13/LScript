using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace LScript {

    public class Interpreter {

        public static IMember[] memory;
        public static Token[] tokens;

        public Interpreter() {
            memory = new IMember[100];
            for(int i = 0; i < memory.Length; i++) {
                memory[i] = new Variable.Integer(0);
            }
        }

        public void exec(string filePath, int tracingLevel = 0) {
            if(!File.Exists(filePath)) {
                throw new Exception("File does not exists");
            }


            // Read the file.
            string text = File.ReadAllText(filePath);


            // Validate that the file has an even number of numbers in it.
            if(text.Length % 2 != 0) {
                throw new Exception("File does not have an even number of numbers in it.");
            }


            // Convert file contents to an array of numbers.
            int[] numbers = new int[text.Length];
            for(int i = 0; i < text.Length; i++) {
                if(Int32.TryParse(text[i].ToString(), out int j)) {
                    numbers[i] = j;
                } else {
                    throw new Exception("A non numerical character was found in file, this is not allowed");
                }
            }


            // Convert the array of numbers to an array of Tokens.
            Token[] tokens = new Token[numbers.Length / 2];
            for(int i = 0; i < tokens.Length; i++) {
                int index = i * 2;
                tokens[i] = new Token((BrickColor)numbers[index], (BrickColor)numbers[index + 1]);
            }

            this.exec(tokens);
        }

        public void exec(Token[] tokens) {
            Interpreter.tokens = tokens;

            int masterIndex = 0;
            while(masterIndex < tokens.Length) {
                Token current = tokens[masterIndex];

                switch(current.first) {
                    case BrickColor.RED:
                        // Define some type of data.
                        this.defineData(ref masterIndex, current.second);
                        break;
                    case BrickColor.GREEN:
                    case BrickColor.YELLOW:
                        // Call function
                        this.callBuiltin(ref masterIndex, current);
                        break;
                    case BrickColor.BLUE:
                        if(current.second == BrickColor.BLUE) {
                            return; // Stop execution
                        }

                        break;
                    default:
                        throw new Exception("Unexpected token " + current.ToString() + " @ " + masterIndex);
                }

                masterIndex++;
            }
        }

        private void defineData(ref int masterIndex, BrickColor color) {
            // Move along until the an end defintion token (RED, RED) is found

            // Create a list of all of the tokens in the defintion.
            List<Token> t = new List<Token>();
            Token? addressToken = null;
            while(true) {
                masterIndex++;
                if(masterIndex >= memory.Length) {
                    // Not enough room for the required pointers, error!
                    throw new Exception("No end defintion token found");
                }

                Token token = Interpreter.tokens[masterIndex];

                if(addressToken == null) {
                    addressToken = token;
                    continue;
                }

                // Break if the end token (RED, RED) is found.
                if(token.first == BrickColor.RED && token.second == BrickColor.RED) {
                    break;
                }

                t.Add(tokens[masterIndex]);
            }

            if(t.Count == 0) {
                throw new Exception("Definition can not contain 0 tokens");
            }

            // Depending on the second color of the defintion token,
            // create a data type and place it in memory.
            IMember newMember;

            if(color == BrickColor.GREEN) { // String
                newMember = new Variable.String(t);
            }
            else if(color == BrickColor.BLUE) { // Integer
                newMember = new Variable.Integer(t);
            }
            else if(color == BrickColor.YELLOW) { // Function
                newMember = new Function(tokens);
            }
            else {
                throw new Exception("Unknown data type (" + color.ToString() + ")");
            }

            int address = BlockConverter.tokenToNumber((Token)addressToken);
            Interpreter.memory[address] = newMember;
        }

        private void callBuiltin(ref int masterIndex, Token token) {
            Builtin.BuiltinFunc func = Builtin.getFunctionFromToken(token);

            if(func != null) {
                Token[] tArray = new Token[func.paramCount];
                for(int i = 0; i < func.paramCount; i++) {
                    masterIndex++;

                    if(masterIndex >= memory.Length) {
                        // Not enough room for the required pointers, error!
                        throw new Exception();
                    }
                    tArray[i] = tokens[masterIndex];
                }

                func.invoke(tArray);
            } else {
                throw new Exception("Unknown function type (" + token.ToString() + ")");
            }
        }
    }
}