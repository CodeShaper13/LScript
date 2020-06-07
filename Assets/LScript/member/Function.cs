namespace LScript {

    public class Function : IMember {

        private readonly Token[] contents;

        public Function(Token[] contents) {
            this.contents = contents;
        }

        public int evalInt() {
            // TODO run func
            throw new System.NotImplementedException();
        }

        public string evalString() {
            // TODO run func
            throw new System.NotImplementedException();
        }
    }
}
