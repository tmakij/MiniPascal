using System.Collections.Generic;

namespace MiniPascal.Parser.AST
{
    public sealed class Arguments
    {
        public int Count { get { return arguments.Count; } }
        private readonly List<IExpression> arguments = new List<IExpression>();
        private readonly List<MiniPascalType> types = new List<MiniPascalType>();

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
                types.Add(exp.NodeType(Current));
            }
        }

        public void EmitIR(CILEmitter Emitter)
        {
        }

        public MiniPascalType Type(int Index)
        {
            return types[Index];
        }

        public IExpression Expression(int Index)
        {
            return arguments[Index];
        }
    }
}
