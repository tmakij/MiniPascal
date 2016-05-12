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
            Used.DeclareVariable(identifier);
            Parameters.CheckIdentifiers(Used);
            block.CheckIdentifiers(Used);
        }

        public void CheckType(IdentifierTypes Types)
        {
            Types.SetProcedureType(identifier, this);
            Parameters.CheckType(Types);
            block.CheckTypes(Types);
        }

        public void EmitIR(CILEmitter Emitter, IdentifierTypes Types)
        {
            Emitter.CreateProcedure(identifier, Parameters);
            Parameters.EmitIR(Emitter, Types);
            block.EmitIR(Emitter, Types);
            Emitter.EndProcedure();
        }
    }
}
