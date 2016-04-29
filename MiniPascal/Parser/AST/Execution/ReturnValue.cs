namespace MiniPL.Parser.AST
{
    public struct ReturnValue
    {
        public MiniPascalType Type { get; }
        public object Value { get; }

        public ReturnValue(MiniPascalType Type, object Value)
        {
            this.Type = Type;
            this.Value = Value;
        }
    }
}
