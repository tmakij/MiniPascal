using System;
using System.Linq;
using System.Collections.Generic;

namespace MiniPascal.Parser.AST
{
    public sealed class Parameters
    {
        public int Count { get { return parameters.Count; } }
        public Type[] Types { get { return (from p in types select p.CLRType).ToArray(); } }
        private readonly List<Variable> parameters = new List<Variable>();
        private readonly List<MiniPascalType> types = new List<MiniPascalType>();

        public ushort Index(Identifier Identifier)
        {
            return (ushort)parameters.FindIndex(p => p.Identifier.Equals(Identifier));
        }

        public bool HasParameter(Identifier Identifier)
        {
            return parameters.Any(p => p.Identifier.Equals(Identifier));
        }

        public void Add(Variable Parameter)
        {
            parameters.Add(Parameter);
        }

        public void CheckIdentifiers(UsedIdentifiers Used)
        {
            foreach (Variable variable in parameters)
            {
                if (Used.IsUsed(variable.Identifier))
                {
                    throw new VariableNameDefinedException(variable.Identifier);
                }
                Console.WriteLine(variable.Identifier);
                Used.DeclareVariable(variable);
            }
        }

        public void CheckType(IdentifierTypes Types)
        {
            foreach (Variable variable in parameters)
            {
                Types.SetIdentifierType(variable.Identifier, variable.Type);
                types.Add(variable.Type);
            }
        }

        public MiniPascalType Type(int Index)
        {
            return types[Index];
        }

        public Variable At(int Index)
        {
            return parameters[Index];
        }

        /*public void EmitIR(CILEmitter Emitter, IdentifierTypes Types)
        {
            /oreach (DeclarationStatement decl in parameters)
            {
                decl.EmitIR(Emitter, Types);
            }
        }*/
    }
}
