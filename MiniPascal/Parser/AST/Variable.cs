namespace MiniPascal.Parser.AST
{
    public sealed class Variable
    {
        public Identifier Identifier { get; }
        public MiniPascalType Type { get; }
        public bool IsReference { get; }

        public Variable(Identifier Identifier, MiniPascalType Type, bool IsReference)
        {
            this.Identifier = Identifier;
            this.Type = Type;
            this.IsReference = IsReference;
        }
    }
}
