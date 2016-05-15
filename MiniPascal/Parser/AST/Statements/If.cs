namespace MiniPascal.Parser.AST
{
    public sealed class If : IStatement
    {
        private readonly Expression condition;
        private readonly IStatement thenStatement, elseStatement;

        public If(Expression Condition, IStatement Then, IStatement Else)
        {
            condition = Condition;
            thenStatement = Then;
            elseStatement = Else;
        }

        public void CheckIdentifiers(UsedIdentifiers Used)
        {
            condition.CheckIdentifiers(Used);
            thenStatement.CheckIdentifiers(Used);
            elseStatement?.CheckIdentifiers(Used);
        }

        public void CheckType(IdentifierTypes Types)
        {
            MiniPascalType condType = condition.NodeType(Types);
            if (!condType.Equals(MiniPascalType.Boolean))
            {
                throw new TypeMismatchException(MiniPascalType.Boolean, condType);
            }
            thenStatement.CheckType(Types);
            elseStatement?.CheckType(Types);
        }

        public void EmitIR(CILEmitter Emitter, IdentifierTypes Types)
        {
            condition.EmitIR(Emitter, false);
            if (elseStatement == null)
            {
                Emitter.If(() => thenStatement.EmitIR(Emitter, Types));
            }
            else
            {
                Emitter.IfElse(() => thenStatement.EmitIR(Emitter, Types), () => elseStatement.EmitIR(Emitter, Types));
            }
        }
    }
}
