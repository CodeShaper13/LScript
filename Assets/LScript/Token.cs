namespace LScript {

    public struct Token {

        public readonly BrickColor first;
        public readonly BrickColor second;

        public Token(BrickColor char1, BrickColor char2) {
            this.first = char1;
            this.second = char2;
        }

        public override string ToString() {
            return "TOKEN(" + this.first.ToString() + ", " + this.second.ToString() + ")";
        }
    }
}
