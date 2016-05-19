namespace MiniPascal.Parser.AST
{
    public sealed class Assert : IStatement
    {
        private readonly IExpression toAssert;

        public Assert(IExpression ToAssert)
        {
            toAssert = ToAssert;
        }

        public void CheckIdentifiers(Scope Current)
        {
            toAssert.CheckIdentifiers(Current);
        }

        public void CheckType(Scope Current)
        {
            toAssert.NodeType(Current);
            if (!toAssert.Type.Equals(MiniPascalType.Boolean))
            {
                throw new InvalidTypeException(toAssert.Type, MiniPascalType.Boolean);
            }
        }

        public void EmitIR(CILEmitter Emitter)
        {
            toAssert.EmitIR(Emitter, false);
            Emitter.Assert();
        }
    }
}
