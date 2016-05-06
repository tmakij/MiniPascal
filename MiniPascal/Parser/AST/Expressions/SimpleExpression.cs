using System.Collections.Generic;

namespace MiniPascal.Parser.AST
{
    public sealed class SimpleExpression : IExpression
    {
        public MiniPascalType Type { get; private set; }
        private readonly OperatorType sign;
        private readonly Term first;
        private readonly List<OperatorPair<Term>> toAdd = new List<OperatorPair<Term>>();

        public SimpleExpression(OperatorType Sign, Term Term)
        {
            sign = Sign;
            first = Term;
        }

        public void Add(OperatorPair<Term> TermToAdd)
        {
            toAdd.Add(TermToAdd);
        }

        public void CheckIdentifiers(UsedIdentifiers Used)
        {
            first.CheckIdentifiers(Used);
        }

        public MiniPascalType NodeType(IdentifierTypes Types)
        {
            Type = first.NodeType(Types);
            if (sign != OperatorType.None && !Type.HasOperatorDefined(sign))
            {
                throw new UndefinedOperatorException(Type, sign);
            }
            foreach (OperatorPair<Term> opr in toAdd)
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
                Type = nextType.BinaryOperation(opr.Operator).ReturnType;
                /*MiniPascalType operationResultType = nextType.BinaryOperation(opr.Operator).ReturnType;
                if (!Type.Equals(operationResultType))
                {
                    throw new InvalidTypeException(operationResultType, Type);
                }*/
            }
            return Type;
        }

        public void EmitIR(CILEmitter Emitter)
        {
            if (sign == OperatorType.Substraction)
            {
                Emitter.PushInt32(-1);
                first.EmitIR(Emitter);
                Emitter.Multiply();
            }
            else
            {
                first.EmitIR(Emitter);
            }
            foreach (OperatorPair<Term> opr in toAdd)
            {
                opr.Operand.EmitIR(Emitter);
                Type.BinaryOperation(opr.Operator).EmitIR(Emitter);
            }
        }
    }
}
