namespace MiniPascal.Parser.AST
{
    public sealed class AssigmentStatement : IStatement
    {
        private readonly VariableReference reference;
        private readonly IExpression assigment;
        private Variable variable;

        public AssigmentStatement(VariableReference Reference, IExpression Assigment)
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
                        throw new System.Exception("Expected " + variable.Type.SimpleType + " but was " + assigmentType.SimpleType);
                    }
                }
                else
                {
                    throw new TypeMismatchException(type, assigmentType);
                }
            }
            if (reference.ArrayIndex != null)
            {
                if (!reference.ArrayIndex.NodeType(Current).SimpleType.Equals(SimpleType.Integer))
                {
                    throw new System.Exception();
                }
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
