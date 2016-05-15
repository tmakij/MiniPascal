using System.Collections.Generic;

namespace MiniPascal.Parser.AST
{
    public sealed class ScopedProgram : IStatement
    {
        private readonly List<IStatement> statements = new List<IStatement>();

        public void Add(IStatement Statement)
        {
            statements.Add(Statement);
        }

        public void CheckIdentifiers(UsedIdentifiers Identifiers)
        {
            foreach (IStatement item in statements)
            {
                item.CheckIdentifiers(Identifiers);
            }
        }

        public void CheckType(IdentifierTypes Types)
        {
            foreach (IStatement item in statements)
            {
                item.CheckType(Types);
            }
        }

        public void EmitIR(CILEmitter Emitter, IdentifierTypes Types)
        {
            CILEmitter blockScope = Emitter.StartBlock();
            foreach (IStatement item in statements)
            {
                item.EmitIR(blockScope, Types);
            }
            //System.Console.WriteLine("End block");
        }
    }
}
