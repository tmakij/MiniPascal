using System.Collections.Generic;

namespace MiniPascal.Parser.AST
{
    public sealed class DeclarationStatement : IStatement
    {
        public MiniPascalType Type { get; }
        public readonly List<Identifier> identifiers;

        public DeclarationStatement(List<Identifier> Identifiers, MiniPascalType Type, IExpression OptionalAssigment)
        {
            identifiers = Identifiers;
            this.Type = Type;
        }

        public void CheckIdentifiers(UsedIdentifiers Used)
        {
            foreach (Identifier ident in identifiers)
            {
                if (Used.IsUsed(ident))
                {
                    throw new VariableNameDefinedException(ident);
                }
                Used.DeclareVariable(ident);
            }
        }

        public void CheckType(IdentifierTypes Types)
        {
            foreach (Identifier ident in identifiers)
            {
                Types.SetIdentifierType(ident, Type);
            }
        }

        public void EmitIR(CILEmitter Emitter, IdentifierTypes Types)
        {
            foreach (Identifier ident in identifiers)
            {
                Emitter.CreateVariable(ident, Type);
                if (Type.Equals(MiniPascalType.Integer))
                {
                    Emitter.PushInt32(0);
                }
                else if (Type.Equals(MiniPascalType.Real))
                {
                    Emitter.PushSingle(0f);
                }
                else if (Type.Equals(MiniPascalType.String))
                {
                    Emitter.PushString("");
                }
                else if (Type.Equals(MiniPascalType.Boolean))
                {
                    Emitter.PushInt32(0);
                }
                Emitter.SaveVariable(ident);
            }
        }
    }
}
