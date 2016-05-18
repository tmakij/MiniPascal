namespace MiniPascal.Parser.AST
{
    public sealed class NoReturnCall : IStatement
    {
        private readonly Call call;

        public NoReturnCall(Call Call)
        {
            call = Call;
        }

        public void CheckIdentifiers(Scope Current)
        {
            call.CheckIdentifiers(Current);
        }

        public void CheckType(Scope Current)
        {
            call.NodeType(Current);
        }

        public void EmitIR(CILEmitter Emitter)
        {
            call.EmitIR(Emitter, false);
            if (call.ToCall.ReturnType != null)
            {
                Emitter.Pop();
            }
        }
    }
}
