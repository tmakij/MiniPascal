using System;
using System.Linq;
using System.Collections.Generic;

namespace MiniPascal.Parser.AST
{
    public sealed class Parameters : IStatement
    {
        public IEnumerable<Variable> All
        {
            get
            {
                for (int i = 0; i < parameters.Count; i++)
                {
                    yield return parameters[i];
                }
                for (int i = 0; i < parametersFromPreviousScope.Count; i++)
                {
                    yield return parametersFromPreviousScope[i];
                }
            }
        }

        public IEnumerable<Variable> PreviousVariables { get { return parametersFromPreviousScope; } }
        public int DeclaredCount { get { return parameters.Count; } }
        private readonly List<Variable> parameters = new List<Variable>();
        private readonly List<Variable> parametersFromPreviousScope = new List<Variable>();
        private readonly List<Variable> allParameters = new List<Variable>();

        public byte Index(Identifier Identifier)
        {
            return (byte)allParameters.FindIndex(p => p.Identifier.Equals(Identifier));
        }

        public bool HasParameter(Identifier Identifier)
        {
            return allParameters.Any(p => p.Identifier.Equals(Identifier));
        }

        public void Add(Variable Parameter)
        {
            parameters.Add(Parameter);
            allParameters.Add(Parameter);
        }

        public void CheckIdentifiers(Scope Current)
        {
            foreach (Variable previousScopevariable in Current.All)
            {
                Variable paraVar;
                if (previousScopevariable.IsReference)
                {
                    paraVar = previousScopevariable;
                }
                else
                {
                    paraVar = new Variable(previousScopevariable.Identifier, previousScopevariable.Type, true);
                }
                parametersFromPreviousScope.Add(paraVar);
                allParameters.Add(paraVar);
                Current.DeclareVariable(paraVar);
            }
            foreach (Variable variable in parameters)
            {
                Current.DeclareVariable(variable);
            }
        }

        public void CheckType(Scope Current)
        {
        }

        public Variable At(int Index)
        {
            return parameters[Index];
        }

        public void EmitIR(CILEmitter Emitter)
        {
        }
    }
}
