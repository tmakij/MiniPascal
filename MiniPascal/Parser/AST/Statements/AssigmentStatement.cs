namespace MiniPascal.Parser.AST
{
    public sealed class AssigmentStatement : IStatement
    {
        private readonly VariableReference reference;
        private readonly ITypedNode assigment;
        private Variable variable;

        public AssigmentStatement(VariableReference Reference, ITypedNode Assigment)
        {
            reference = Reference;
            assigment = Assigment;
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
            assigment.CheckIdentifiers(Current);
        }

        public void CheckType(Scope Current)
        {
            MiniPascalType type = variable.Type;
            MiniPascalType assigmentType = assigment.NodeType(Current);
            if (!assigmentType.Equals(type))
            {
                if (variable.Type.IsArray && !assigmentType.IsArray)
                {
                    if (!variable.Type.SimpleType.Equals(assigmentType.SimpleType))
                    {
                        throw new InvalidSimpleTypeException(assigmentType.SimpleType, variable.Type.SimpleType);
                    }
                }
                else
                {
                    throw new TypeMismatchException(type, assigmentType);
                }
            }
            if (reference.ArrayIndex != null)
            {
                if (assigmentType.IsArray)
                {
                    throw new ArrayAssigmentExpection(type.SimpleType, assigmentType);
                }
                MiniPascalType indexType = reference.ArrayIndex.NodeType(Current);
                if (!indexType.Equals(MiniPascalType.Integer))
                {
                    throw new InvalidArrayIndexTypeException(indexType);
                }
            }
            else if (reference.ArrayIndex == null && variable.Type.IsArray && !assigmentType.IsArray)
            {
                throw new InvalidArrayIndexTypeException(null);
            }
        }

        public void EmitIR(CILEmitter Emitter)
        {
            if (variable.IsReference)
            {
                if (reference.ArrayIndex == null)
                {
                    Emitter.LoadVariable(reference.Name);
                    assigment.EmitIR(Emitter, false);
                    Emitter.SaveReferenceVariable(variable);
                }
                else
                {
                    Emitter.LoadReferenceVariable(variable);
                    reference.ArrayIndex.EmitIR(Emitter, false);
                    assigment.EmitIR(Emitter, false);
                    Emitter.SaveArray(variable);
                }
            }
            else
            {
                if (reference.ArrayIndex == null)
                {
                    assigment.EmitIR(Emitter, false);
                    Emitter.SaveVariable(reference.Name);
                }
                else
                {
                    Emitter.LoadVariable(variable.Identifier);
                    reference.ArrayIndex.EmitIR(Emitter, false);
                    assigment.EmitIR(Emitter, false);
                    Emitter.SaveArray(variable);
                }
            }
        }
    }
}
