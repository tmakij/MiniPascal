using System.Collections.Generic;

namespace MiniPL.Parser.AST
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

        public void Execute(Variables Scope)
        {
            foreach (IStatement item in statements)
            {
                item.Execute(Scope);
            }
        }
    }
}
