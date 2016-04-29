using System;

namespace MiniPL.Parser.AST
{
    public sealed class PrintStatement : IStatement
    {
        private readonly IExpression toPrint;
        private MiniPascalType type;

        public PrintStatement(IExpression ToPrint)
        {
            toPrint = ToPrint;
        }

        public void CheckIdentifiers(UsedIdentifiers Used)
        {
            toPrint.CheckIdentifiers(Used);
        }

        public void CheckType(IdentifierTypes Types)
        {
            type = toPrint.NodeType(Types);
        }

        public void EmitIR(CILEmitter Emitter)
        {
            toPrint.EmitIR(Emitter);
            Emitter.CallPrint(type);
        }

        public void Execute(Variables Scope)
        {
            ReturnValue ret = toPrint.Execute(Scope);
            Console.Write(ret.Value);
        }
    }
}
