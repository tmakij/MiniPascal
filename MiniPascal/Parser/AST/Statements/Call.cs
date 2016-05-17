namespace MiniPascal.Parser.AST
{
    public sealed class Call : IStatement
    {
        private readonly Arguments arguments;
        private readonly Identifier toBeCalled;
        private Procedure proc;
        private Scope current;

        public Call(Identifier ToBeCalled, Arguments Arguments)
        {
            toBeCalled = ToBeCalled;
            arguments = Arguments;
        }

        public void CheckIdentifiers(Scope Current)
        {
            current = Current;
            if (!Current.IsUsed(toBeCalled))
            {
                throw new UninitializedVariableException(toBeCalled);
            }
            arguments.CheckIdentifiers(Current);
        }

        public void CheckType(Scope Current)
        {
            arguments.CheckType(Current);
            proc = Current.Procedure(toBeCalled);
            if (arguments.Count != proc.Parameters.DeclaredCount)
            {
                throw new System.Exception("Invalid parameter count");
            }
            Parameters parameters = proc.Parameters;
            for (int i = 0; i < arguments.Count; i++)
            {
                MiniPascalType argType = arguments.Type(i);
                MiniPascalType parType = parameters.At(i).Type;
                if (!argType.Equals(parType))
                {
                    throw new TypeMismatchException(parType, argType);
                }
            }
        }

        public void EmitIR(CILEmitter Emitter)
        {
            for (int i = 0; i < arguments.Count; i++)
            {
                IExpression expr = arguments.Expression(i);
                bool loadReference = proc.Parameters.At(i).IsReference;
                expr.EmitIR(Emitter, loadReference);
            }
            foreach (Variable prevVar in proc.Parameters.PreviousVariables)
            {
                if (current.Variable(prevVar.Identifier).IsReference)
                {
                    Emitter.LoadVariable(prevVar.Identifier);
                }
                else
                {
                    Emitter.LoadVariableAddress(prevVar.Identifier);
                }
            }
            Emitter.Call(toBeCalled);
        }
    }
}
