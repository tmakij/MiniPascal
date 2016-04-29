namespace MiniPL.Parser.AST
{
    public struct ReturnValue
    {
        public MiniPLType Type { get; }
        public object Value { get; }

        public ReturnValue(MiniPLType Type, object Value)
        {
            this.Type = Type;
            this.Value = Value;
        }
    }
}
