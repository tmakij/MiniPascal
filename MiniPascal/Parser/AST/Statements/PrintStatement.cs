namespace MiniPascal.Parser.AST
{
    public sealed class PrintStatement : IStatement
    {
        private readonly Arguments toPrint;

        public PrintStatement(Arguments ToPrint)
        {
            toPrint = ToPrint;
        }

        public void CheckIdentifiers(UsedIdentifiers Used)
        {
            toPrint.CheckIdentifiers(Used);
        }

        public void CheckType(IdentifierTypes Types)
        {
            toPrint.CheckType(Types);
        }

        public void EmitIR(CILEmitter Emitter, IdentifierTypes Types)
        {
            for (int i = 0; i < toPrint.Count; i++)
            {
                Expression expr = toPrint.Expression(i);
                MiniPascalType exprType = toPrint.Type(i);
                expr.EmitIR(Emitter);
                Emitter.CallPrint(exprType);
            }
        }
    }
}
