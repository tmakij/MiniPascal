using System.Collections.Generic;

namespace MiniPascal.Parser.AST
{
    public sealed class Scope
    {
        public IEnumerable<Variable> All
        {
            get
            {
                foreach (Variable val in variables.Values)
                {
                    yield return val;
                }
                if (previous != null)
                {
                    foreach (Variable val in previous.variables.Values)
                    {
                        yield return val;
                    }
                }
            }
        }
        public IEnumerable<Variable> Local { get { return variables.Values; } }
        private readonly Dictionary<Identifier, Variable> variables = new Dictionary<Identifier, Variable>();
        private readonly Dictionary<Identifier, Procedure> procedures = new Dictionary<Identifier, Procedure>();

        private readonly Scope previous;

        public Scope(Scope Previous)
        {
            previous = Previous;
        }

        public void DeclareMethod(Identifier Identifier, Procedure Procedure)
        {
            procedures.Add(Identifier, Procedure);
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
            return variables.ContainsKey(Identifier) || procedures.ContainsKey(Identifier);
        }

        public Variable Variable(Identifier Name)
        {
            return variables[Name];
        }

        public Procedure Procedure(Identifier Name)
        {
            return procedures[Name];
        }
    }
}
