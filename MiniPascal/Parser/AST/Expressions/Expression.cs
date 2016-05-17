using System.Collections.Generic;

namespace MiniPascal.Parser.AST
{
    public class Expression<T> : IExpression where T : ITypedNode
    {
        public MiniPascalType Type { get; private set; }
        private readonly List<OperatorPair<T>> simpleExpressions = new List<OperatorPair<T>>();
        private readonly T firstExpression;

        public Expression(T FirstExpression)
        {
            firstExpression = FirstExpression;
        }

        public void Add(OperatorPair<T> AdditionalExpression)
        {
            simpleExpressions.Add(AdditionalExpression);
        }

        public void CheckIdentifiers(Scope Current)
        {
            firstExpression.CheckIdentifiers(Current);
            foreach (OperatorPair<T> opr in simpleExpressions)
            {
                opr.Operand.CheckIdentifiers(Current);
            }
        }

        public virtual MiniPascalType NodeType(Scope Current)
        {
            Type = firstExpression.NodeType(Current);
            foreach (OperatorPair<T> expr in simpleExpressions)
            {
                MiniPascalType anotherType = expr.Operand.NodeType(Current);
                if (!Type.Equals(anotherType))
                {
                    throw new InvalidTypeException(anotherType, Type);
                }
                if (!Type.HasOperatorDefined(expr.Operator))
                {
                    throw new UndefinedOperatorException(Type, expr.Operator);
                }
                Type = anotherType.BinaryOperation(expr.Operator).ReturnType;
            }
            return Type;
        }

        public void EmitIR(CILEmitter Emitter, bool Reference)
        {
            LoadFirst(Emitter, Reference);
            foreach (OperatorPair<T> expr in simpleExpressions)
            {
                expr.Operand.EmitIR(Emitter, Reference);
                expr.Operand.Type.BinaryOperation(expr.Operator).EmitIR(Emitter);
            }
        }

        protected virtual void LoadFirst(CILEmitter Emitter, bool Reference)
        {
            firstExpression.EmitIR(Emitter, Reference);
        }
    }
}
