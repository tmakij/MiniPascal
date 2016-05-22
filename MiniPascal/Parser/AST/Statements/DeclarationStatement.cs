using System.Collections.Generic;

namespace MiniPascal.Parser.AST
{
    public sealed class DeclarationStatement : IStatement
    {
        private MiniPascalType Type { get; }
        private readonly List<Variable> variables = new List<Variable>();

        public DeclarationStatement(List<Identifier> Identifiers, MiniPascalType Type)
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
            if (Type.IsArray)
            {
                if (Type.Size == null)
                {
                    throw new InvalidArrayIndexTypeException(null);
                }
                Type.Size.CheckIdentifiers(Current);
            }
        }

        public void CheckType(Scope Current)
        {
            if (Type.IsArray)
            {
                Type.Size.NodeType(Current);
                if (!Type.Size.Type.Equals(MiniPascalType.Integer))
                {
                    throw new TypeMismatchException(MiniPascalType.Integer, Type.Size.Type);
                }
            }
        }

        public void EmitIR(CILEmitter Emitter)
        {
            if (Type.IsArray)
            {
                foreach (Variable variable in variables)
                {
                    Emitter.CreateArrayVariable(variable.Identifier, Type);
                    Type.Size.EmitIR(Emitter, false);
                    Emitter.PushArray(Type);
                    Emitter.SaveVariable(variable.Identifier);
                }
            }
            else
            {
                foreach (Variable variable in variables)
                {
                    Emitter.CreateSimpleVariable(variable.Identifier, Type);
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
}
