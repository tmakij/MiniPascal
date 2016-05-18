namespace MiniPascal.Parser.AST
{
    public sealed class Call : IOperand
    {
        public Procedure ToCall { get; private set; }
        public MiniPascalType Type { get; private set; }
        private readonly Arguments arguments;
        private readonly Identifier toBeCalled;
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

        public MiniPascalType NodeType(Scope Current)
        {
            arguments.CheckType(Current);
            ToCall = Current.Procedure(toBeCalled);
            if (arguments.Count != ToCall.Parameters.DeclaredCount)
            {
                throw new System.Exception("Invalid parameter count");
            }
            Parameters parameters = ToCall.Parameters;
            for (int i = 0; i < arguments.Count; i++)
            {
                MiniPascalType argType = arguments.Type(i);
                MiniPascalType parType = parameters.At(i).Type;
                if (!argType.Equals(parType))
                {
                    throw new TypeMismatchException(parType, argType);
                }
            }
            Type = ToCall.ReturnType;
            return Type;
        }

        public void EmitIR(CILEmitter Emitter, bool Reference)
        {
            for (int i = 0; i < arguments.Count; i++)
            {
                IExpression expr = arguments.Expression(i);
                bool loadReference = ToCall.Parameters.At(i).IsReference;
                expr.EmitIR(Emitter, loadReference);
            }
            foreach (Variable prevVar in ToCall.Parameters.PreviousVariables)
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
