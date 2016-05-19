namespace MiniPascal.Parser.AST
{
    public sealed class While : IStatement
    {
        private readonly IExpression condition;
        private readonly IStatement doStatement;

        public While(IExpression Condition, IStatement Do)
        {
            condition = Condition;
            doStatement = Do;
        }

        public void CheckIdentifiers(Scope Current)
        {
            condition.CheckIdentifiers(Current);
            doStatement.CheckIdentifiers(Current);
        }

        public void CheckType(Scope Current)
        {
            condition.NodeType(Current);
            doStatement.CheckType(Current);
        }

        public void EmitIR(CILEmitter Emitter)
        {
            Emitter.While(() => condition.EmitIR(Emitter, false), () => doStatement.EmitIR(Emitter));
        }
    }
}
