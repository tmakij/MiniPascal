namespace MiniPL.Parser.AST
{
    public sealed class RuntimeVariable
    {
        public MiniPascalType Type { get; }
        public object Value { get; set; }

        public RuntimeVariable(MiniPascalType Type)
        {
            this.Type = Type;
            Value = Type.DefaultValue;
        }
    }
}
