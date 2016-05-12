using System;
using System.Linq;
using System.Collections.Generic;

namespace MiniPascal.Parser.AST
{
    public sealed class Parameters
    {
        public int Count { get { return parameters.Count; } }
        public Type[] Types { get { return (from p in types select p.CLRType).ToArray(); } }
        private readonly List<DeclarationStatement> parameters = new List<DeclarationStatement>();
        private readonly List<MiniPascalType> types = new List<MiniPascalType>();

        public ushort Index(Identifier Identifier)
        {
            return (ushort)parameters.FindIndex(p => p.identifiers[0].Equals(Identifier));
        }

        public void Add(DeclarationStatement Parameter)
        {
            parameters.Add(Parameter);
        }

        public void CheckIdentifiers(UsedIdentifiers Used)
        {
            foreach (DeclarationStatement decl in parameters)
            {
                decl.CheckIdentifiers(Used);
            }
        }

        public void CheckType(IdentifierTypes Types)
        {
            foreach (DeclarationStatement decl in parameters)
            {
                decl.CheckType(Types);
                types.Add(decl.Type);
            }
        }

        public MiniPascalType Type(int Index)
        {
            return types[Index];
        }

        public void EmitIR(CILEmitter Emitter, IdentifierTypes Types)
        {
            /*foreach (DeclarationStatement decl in parameters)
            {
                decl.EmitIR(Emitter, Types);
            }*/
        }
    }
}
