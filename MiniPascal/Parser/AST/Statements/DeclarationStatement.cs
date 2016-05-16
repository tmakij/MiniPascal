using System.Collections.Generic;

namespace MiniPascal.Parser.AST
{
    public sealed class DeclarationStatement : IStatement
    {
        private MiniPascalType Type { get; }
        private readonly List<Variable> variables = new List<Variable>();

        public DeclarationStatement(List<Identifier> Identifiers, MiniPascalType Type, IExpression OptionalAssigment)
        {
            foreach (Identifier ident in Identifiers)
            {
                variables.Add(new Variable(ident, Type, false));
            }
            this.Type = Type;
        }

        public void CheckIdentifiers(Scope Current)
        {
            foreach (Variable variable in variables)
            {
                if (Current.IsUsed(variable))
                {
                    throw new VariableNameDefinedException(variable.Identifier);
                }
                Current.DeclareVariable(variable);
            }
        }

        public void CheckType(Scope Current)
        {
        }

        public void EmitIR(CILEmitter Emitter)
        {
            foreach (Variable variable in variables)
            {
                Emitter.CreateVariable(variable.Identifier, Type);
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
                Emitter.SaveVariable(variable.Identifier);
            }
        }
    }
}
