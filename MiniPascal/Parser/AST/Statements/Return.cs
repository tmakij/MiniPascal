namespace MiniPascal.Parser.AST
{
    public sealed class Return : IStatement
    {
        private readonly IExpression toReturn;

        public Return(IExpression ToReturn)
        {
            toReturn = ToReturn;
        }

        public void CheckIdentifiers(Scope Current)
        {
            toReturn?.CheckIdentifiers(Current);
        }

        public void CheckType(Scope Current)
        {
            toReturn?.NodeType(Current);
        }

        public void EmitIR(CILEmitter Emitter)
        {
            toReturn?.EmitIR(Emitter, false);
            Emitter.Return();
        }
    }
}
