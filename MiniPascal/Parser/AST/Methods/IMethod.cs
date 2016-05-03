namespace MiniPascal.Parser.AST
{
    public interface IMethod
    {
        void EmitIR(Identifier Caller, CILEmitter Emitter);
        MiniPascalType ReturnType { get; }
    }
}
