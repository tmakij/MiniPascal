using System.Collections.Generic;

namespace MiniPascal.Parser.AST
{
    public sealed class UsedIdentifiers
    {
        private readonly HashSet<Identifier> methods = new HashSet<Identifier>();
        private readonly Dictionary<Identifier, Variable> variables = new Dictionary<Identifier, Variable>();

        public void DeclareMethod(Identifier Identifier)
        {
            methods.Add(Identifier);
        }

        public void DeclareVariable(Variable Variable)
        {
            variables.Add(Variable.Identifier, Variable);
        }

        public bool IsUsed(Variable Variable)
        {
            return IsUsed(Variable.Identifier);
        }

        public bool IsUsed(Identifier Identifier)
        {
            return variables.ContainsKey(Identifier) || methods.Contains(Identifier);
        }

        public Variable Variable(Identifier VarIdentifier)
        {
            return variables[VarIdentifier];
        }
    }
}
