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

        public void CheckIdentifiers(Scope Current)
        {
            condition.CheckIdentifiers(Current);
            thenStatement.CheckIdentifiers(Current);
            elseStatement?.CheckIdentifiers(Current);
        }

        public void CheckType(Scope Current)
        {
            MiniPascalType condType = condition.NodeType(Current);
            if (!condType.Equals(MiniPascalType.Boolean))
            {
                throw new TypeMismatchException(MiniPascalType.Boolean, condType);
            }
            thenStatement.CheckType(Current);
            elseStatement?.CheckType(Current);
        }

        public void EmitIR(CILEmitter Emitter)
        {
            condition.EmitIR(Emitter, false);
            if (elseStatement == null)
            {
                Emitter.If(() => thenStatement.EmitIR(Emitter));
            }
            else
            {
                Emitter.IfElse(() => thenStatement.EmitIR(Emitter), () => elseStatement.EmitIR(Emitter));
            }
        }
    }
}
