namespace MiniPascal.Parser.AST
{
    public sealed class AssigmentStatement : IStatement
    {
        private readonly Identifier identifier;
        private readonly Expression expression;
        private Variable variable;

        public AssigmentStatement(Identifier Identifier, Expression Expression)
        {
            identifier = Identifier;
            expression = Expression;
        }

        public void CheckIdentifiers(Scope Current)
        {
            System.Console.WriteLine("All");
            foreach (Variable var in Current.All)
            {
                System.Console.WriteLine(var.Identifier);
            }
            if (!Current.IsUsed(identifier))
            {
                throw new UninitializedVariableException(identifier);
            }
            variable = Current.Variable(identifier);
            expression.CheckIdentifiers(Current);
        }

        public void CheckType(Scope Current)
        {
            MiniPascalType type = Current.Variable(identifier).Type;
            MiniPascalType assigmentType = expression.NodeType(Current);
            if (!assigmentType.Equals(type))
            {
                throw new TypeMismatchException(type, assigmentType);
            }
        }

        public void EmitIR(CILEmitter Emitter)
        {
            if (variable.IsReference)
            {
                Emitter.LoadVariable(identifier);
            }
            expression.EmitIR(Emitter, false);
            if (variable.IsReference)
            {
                Emitter.SaveReferenceVariable(variable);
            }
            else
            {
                Emitter.SaveVariable(identifier);
            }
        }
    }
}
