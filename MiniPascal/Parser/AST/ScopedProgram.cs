using System.Collections.Generic;

namespace MiniPascal.Parser.AST
{
    public sealed class ScopedProgram
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

        public void CheckTypes(IdentifierTypes Types)
        {
            foreach (IStatement item in statements)
            {
                item.CheckType(Types);
            }
        }

        public void EmitIR(CILEmitter Emitter, IdentifierTypes Types, Parameters Parameters)
        {
            CILEmitter blockScope = Emitter.StartBlock(Parameters);
            foreach (IStatement item in statements)
            {
                item.EmitIR(blockScope, Types);
            }
            //System.Console.WriteLine("End block");
        }
    }
}
