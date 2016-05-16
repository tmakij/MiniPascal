using System.Collections.Generic;

namespace MiniPascal.Parser.AST
{
    public sealed class SimpleExpression
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

        public void CheckIdentifiers(Scope Current)
        {
            first.CheckIdentifiers(Current);
            foreach (OperatorPair<Term> opr in toAdd)
            {
                opr.Operand.CheckIdentifiers(Current);
            }
        }

        public MiniPascalType NodeType(Scope Current)
        {
            Type = first.NodeType(Current);
            if (sign != OperatorType.None && !Type.HasOperatorDefined(sign))
            {
                throw new UndefinedOperatorException(Type, sign);
            }
            foreach (OperatorPair<Term> opr in toAdd)
            {
                MiniPascalType nextType = opr.Operand.NodeType(Current);
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

        public void EmitIR(CILEmitter Emitter, bool Reference)
        {
            if (sign == OperatorType.Substraction)
            {
                if (Type.Equals(MiniPascalType.Integer))
                {
                    Emitter.PushInt32(-1);
                }
                else if (Type.Equals(MiniPascalType.Real))
                {
                    Emitter.PushSingle(-1f);
                }
                first.EmitIR(Emitter, Reference);
                Emitter.Multiply();
            }
            else
            {
                first.EmitIR(Emitter, Reference);
            }
            foreach (OperatorPair<Term> opr in toAdd)
            {
                opr.Operand.EmitIR(Emitter, Reference);
                Type.BinaryOperation(opr.Operator).EmitIR(Emitter);
            }
        }
    }
}
