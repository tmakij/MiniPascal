using System.Collections.Generic;

namespace MiniPascal.Parser.AST
{
    public sealed class Arguments
    {
        public int Count { get { return arguments.Count; } }
        private readonly List<Expression> arguments = new List<Expression>();
        private readonly List<MiniPascalType> types = new List<MiniPascalType>();

        public void Add(Expression Argument)
        {
            arguments.Add(Argument);
        }

        public void CheckIdentifiers(UsedIdentifiers Used)
        {
        }

        public void CheckType(IdentifierTypes Types)
        {
            foreach (Expression exp in arguments)
            {
                types.Add(exp.NodeType(Types));
            }
        }

        public void EmitIR(CILEmitter Emitter)
        {
        }

        public MiniPascalType Type(int Index)
        {
            return types[Index];
        }

        public Expression Expression(int Index)
        {
            return arguments[Index];
        }
    }
}
