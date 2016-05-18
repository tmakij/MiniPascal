namespace MiniPascal.Parser.AST
{
    public sealed class Procedure : IStatement
    {
        public Parameters Parameters { get; }
        public MiniPascalType ReturnType { get; }
        private readonly Identifier identifier;
        private readonly ScopedProgram block;

        public Procedure(Identifier Identifier, Parameters Parameters, ScopedProgram Block, MiniPascalType Type)
        {
            identifier = Identifier;
            this.Parameters = Parameters;
            block = Block;
            ReturnType = Type;
        }

        public void CheckIdentifiers(Scope Current)
        {
            Current.DeclareMethod(identifier, this);
            block.CheckIdentifiers(Current);
        }

        public void CheckType(Scope Current)
        {
            block.CheckType(Current);
        }

        public void EmitIR(CILEmitter Emitter)
        {
            CILEmitter proc = Emitter.StartProcedure(identifier, Parameters, ReturnType);
            block.EmitIR(proc);
            proc.Return();
        }
    }
}
