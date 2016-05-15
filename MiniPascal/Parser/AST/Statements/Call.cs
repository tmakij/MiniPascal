namespace MiniPascal.Parser.AST
{
    public sealed class Call : IStatement
    {
        private readonly Arguments arguments;
        private readonly Identifier toBeCalled;
        private Procedure proc;

        public Call(Identifier ToBeCalled, Arguments Arguments)
        {
            toBeCalled = ToBeCalled;
            arguments = Arguments;
        }

        public void CheckIdentifiers(UsedIdentifiers Used)
        {
            if (!Used.IsUsed(toBeCalled))
            {
                throw new UninitializedVariableException(toBeCalled);
            }
            arguments.CheckIdentifiers(Used);
        }

        public void CheckType(IdentifierTypes Types)
        {
            arguments.CheckType(Types);
            proc = Types.Procedure(toBeCalled);
            if (arguments.Count != proc.Parameters.Count)
            {
                throw new System.Exception("Invalid parameter count");
            }
            Parameters parameters = proc.Parameters;
            for (int i = 0; i < arguments.Count; i++)
            {
                MiniPascalType argType = arguments.Type(i);
                MiniPascalType parType = parameters.Type(i);
                if (!argType.Equals(parType))
                {
                    throw new TypeMismatchException(parType, argType);
                }
            }
        }

        public void EmitIR(CILEmitter Emitter, IdentifierTypes Types)
        {
            for (int i = 0; i < arguments.Count; i++)
            {
                Expression expr = arguments.Expression(i);
                bool loadReference = proc.Parameters.At(i).IsReference;
                expr.EmitIR(Emitter, loadReference);
            }
            Emitter.Call(toBeCalled);
        }
    }
}
