namespace MiniPascal.Parser.AST
{
    public sealed class PrintStatement : IStatement
    {
        private readonly Arguments toPrint;

        public PrintStatement(Arguments ToPrint)
        {
            toPrint = ToPrint;
        }

        public void CheckIdentifiers(Scope Current)
        {
            toPrint.CheckIdentifiers(Current);
        }

        public void CheckType(Scope Current)
        {
            toPrint.CheckType(Current);
        }

        public void EmitIR(CILEmitter Emitter)
        {
            for (int i = 0; i < toPrint.Count; i++)
            {
                Expression expr = toPrint.Expression(i);
                MiniPascalType exprType = toPrint.Type(i);
                expr.EmitIR(Emitter, false);
                Emitter.CallPrint(exprType);
            }
        }
    }
}
