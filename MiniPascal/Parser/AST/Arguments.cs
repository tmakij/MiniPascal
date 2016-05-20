using System.Collections.Generic;

namespace MiniPascal.Parser.AST
{
    public sealed class Arguments
    {
        public int Count { get { return arguments.Count; } }
        private readonly List<IExpression> arguments = new List<IExpression>();

        public void Add(IExpression Argument)
        {
            arguments.Add(Argument);
        }

        public void CheckIdentifiers(Scope Current)
        {
            foreach (IExpression exp in arguments)
            {
                exp.CheckIdentifiers(Current);
            }
        }

        public void CheckType(Scope Current)
        {
            foreach (IExpression exp in arguments)
            {
                exp.NodeType(Current);
            }
        }

        public MiniPascalType Type(int Index)
        {
            return arguments[Index].Type;
        }

        public IExpression Expression(int Index)
        {
            return arguments[Index];
        }
    }
}
