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

        public void CheckIdentifiers(UsedIdentifiers Used)
        {
            Used.DeclareMethod(identifier);
            Parameters.CheckIdentifiers(Used);
            block.CheckIdentifiers(Used);
        }

        public void CheckType(IdentifierTypes Types)
        {
            Types.SetProcedureType(identifier, this);
            Parameters.CheckType(Types);
            block.CheckType(Types);
        }

        public void EmitIR(CILEmitter Emitter, IdentifierTypes Types)
        {
            Emitter.CreateProcedure(identifier, Parameters);
            CILEmitter procBlock = Emitter.StartBlock(Parameters);
            //Parameters.EmitIR(procBlock, Types);
            block.EmitIR(procBlock, Types);
            procBlock.EndProcedure();
        }
    }
}
