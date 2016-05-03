using System.Collections.Generic;

namespace MiniPascal.Parser.AST
{
    public sealed class Term
    {
        public MiniPascalType Type { get; private set; }
        private readonly IOperand first;
        private readonly List<OperatorPair<IOperand>> toAdd = new List<OperatorPair<IOperand>>();

        public Term(IOperand Factor)
        {
            first = Factor;
        }

        public void Add(OperatorPair<IOperand> Factor)
        {
            toAdd.Add(Factor);
        }

        public void CheckIdentifiers(UsedIdentifiers Used)
        {
            first.CheckIdentifiers(Used);
        }

        public MiniPascalType NodeType(IdentifierTypes Types)
        {
            Type = first.NodeType(Types);
            foreach (OperatorPair<IOperand> opr in toAdd)
            {
                MiniPascalType nextType = opr.Operand.NodeType(Types);
                if (!Type.Equals(nextType))
                {
                    throw new InvalidTypeException(nextType, Type);
                }
                if (!Type.HasOperatorDefined(opr.Operator))
                {
                    throw new UndefinedOperatorException(Type, opr.Operator);
                }
            }
            return Type;
        }

        public void EmitIR(CILEmitter Emitter)
        {
            first.EmitIR(Emitter);
            foreach (OperatorPair<IOperand> factor in toAdd)
            {
                factor.Operand.EmitIR(Emitter);
                Type.BinaryOperation(factor.Operator).EmitIR(Emitter);
            }
        }
    }
}
