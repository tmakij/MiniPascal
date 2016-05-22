namespace MiniPascal.Parser.AST
{
    public sealed class VariableOperand : IOperand
    {
        public MiniPascalType Type { get; private set; }
        private readonly VariableReference reference;
        private Variable variable;

        public VariableOperand(VariableReference Variable)
        {
            reference = Variable;
        }

        public void CheckIdentifiers(Scope Current)
        {
            if (!Current.IsUsed(reference.Name))
            {
                throw new UninitializedVariableException(reference.Name);
            }
            variable = Current.Variable(reference.Name);
            if (reference.ArrayIndex != null)
            {
                reference.ArrayIndex.CheckIdentifiers(Current);
            }
        }

        public MiniPascalType NodeType(Scope Current)
        {
            Type = variable.Type;
            if (reference.ArrayIndex != null)
            {
                MiniPascalType refType = reference.ArrayIndex.NodeType(Current);
                if (!refType.Equals(MiniPascalType.Integer))
                {
                    throw new InvalidArrayIndexTypeException(refType);
                }
                if (Type.SimpleType.Equals(SimpleType.Boolean))
                {
                    return MiniPascalType.Boolean;
                }
                else if (Type.SimpleType.Equals(SimpleType.Integer))
                {
                    return MiniPascalType.Integer;
                }
                else if (Type.SimpleType.Equals(SimpleType.String))
                {
                    return MiniPascalType.String;
                }
                else if (Type.SimpleType.Equals(SimpleType.Real))
                {
                    return MiniPascalType.Real;
                }
            }
            return Type;
        }

        public void EmitIR(CILEmitter Emitter, bool Reference)
        {
            if (Reference)
            {
                if (Type.IsArray && reference.ArrayIndex != null)
                {
                    Emitter.LoadVariable(reference.Name);
                    reference.ArrayIndex.EmitIR(Emitter, false);
                    Emitter.LoadArrayIndexAddress(Type);
                }
                else
                {
                    Emitter.LoadVariableAddress(reference.Name);
                }

            }
            else if (variable.IsReference)
            {
                Emitter.LoadReferenceVariable(variable);
            }
            else
            {
                Emitter.LoadVariable(reference.Name);
                if (reference.ArrayIndex != null)
                {
                    reference.ArrayIndex.EmitIR(Emitter, false);
                    Emitter.LoadArrayVariable(variable);
                }
            }
        }
    }
}
