using System.Collections.Generic;

namespace MiniPascal.Parser.AST
{
    public sealed class Arguments
    {
        private readonly List<Expression> arguments = new List<Expression>();
        private readonly Dictionary<Expression, MiniPascalType> types = new Dictionary<Expression, MiniPascalType>();

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
                types.Add(exp, exp.NodeType(Types));
            }
        }

        public void EmitIR(CILEmitter Emitter)
        {
        }

        public MiniPascalType ArgumentType(Expression Arg)
        {
            return types[Arg];
        }

        public List<Expression>.Enumerator GetEnumerator()
        {
            return arguments.GetEnumerator();
        }
    }
}
