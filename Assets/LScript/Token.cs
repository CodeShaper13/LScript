namespace LScript {

    /// <summary>
    /// Represents a single token (two bricks).
    /// </summary>
    public struct Token {

        public readonly BrickColor first;
        public readonly BrickColor second;

        public Token(BrickColor char1, BrickColor char2) {
            this.first = char1;
            this.second = char2;
        }

        public static bool operator ==(Token obj1, Token obj2) {
            if(ReferenceEquals(obj1, obj2)) {
                return true;
            }
            if(ReferenceEquals(obj1, null)) {
                return false;
            }
            if(ReferenceEquals(obj2, null)) {
                return false;
            }

            return obj1.Equals(obj2);
        }

        public static bool operator !=(Token obj1, Token obj2) {
            return !(obj1 == obj2);
        }

        public override bool Equals(object obj) {
            return obj is Token token &&
                   this.first == token.first &&
                   this.second == token.second;
        }

        public override int GetHashCode() {
            int hashCode = 405212230;
            hashCode = hashCode * -1521134295 + first.GetHashCode();
            hashCode = hashCode * -1521134295 + second.GetHashCode();
            return hashCode;
        }

        public override string ToString() {
            return string.Format("Token({0},{1})", this.first.ToString(), this.second.ToString());
        }
    }
}
