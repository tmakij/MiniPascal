using System.Collections.Generic;

namespace MiniPascal.Parser.AST
{
    public sealed class VariableReferences
    {
        public IEnumerable<Variable> All { get { return variables; } }
        private readonly List<VariableReference> identifiers = new List<VariableReference>();
        private readonly List<Variable> variables = new List<Variable>();

        public void Add(VariableReference Identifier)
        {
            identifiers.Add(Identifier);
        }

        public void CheckIdentifiers(Scope Current)
        {
            foreach (VariableReference id in identifiers)
            {
                if (!Current.IsUsed(id.Name))
                {
                    throw new UninitializedVariableException(id.Name);
                }
                variables.Add(Current.Variable(id.Name));
            }
        }
    }
}
