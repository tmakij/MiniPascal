namespace MiniPascal.Parser.AST
{
    public sealed class Procedure : IStatement
    {
        public Parameters Parameters { get; }
        private readonly Identifier identifier;
        private readonly ScopedProgram block;

        public Procedure(Identifier Identifier, Parameters Parameters, ScopedProgram Block)
        {
            identifier = Identifier;
            this.Parameters = Parameters;
            block = Block;
        }

        public void CheckIdentifiers(Scope Current)
        {
            Current.DeclareMethod(identifier, this);
            //Parameters.CheckIdentifiers(Current);
            block.CheckIdentifiers(Current);
        }

        public void CheckType(Scope Current)
        {
            //Parameters.CheckType(Current);
            block.CheckType(Current);
        }

        public void EmitIR(CILEmitter Emitter)
        {
            CILEmitter proc = Emitter.StartProcedure(identifier, Parameters);
            //CILEmitter procBlock = proc.StartBlock();
            block.EmitIR(proc);
            proc.EndProcedure();
        }
    }
}
