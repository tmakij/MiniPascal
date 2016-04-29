using System;

namespace MiniPL.Parser.AST
{
    public sealed class ReadStatement : IStatement
    {
        private readonly Identifier variableToReadInto;

        public ReadStatement(Identifier VariableToReadInto)
        {
            variableToReadInto = VariableToReadInto;
        }

        public void CheckIdentifiers(UsedIdentifiers Used)
        {
            if (!Used.IsUsed(variableToReadInto))
            {
                throw new UninitializedVariableException(variableToReadInto);
            }
            if (!Used.IsMutable(variableToReadInto))
            {
                throw new ImmutableVariableException(variableToReadInto);
            }
        }

        public void CheckType(IdentifierTypes Types)
        {
            MiniPLType varType = Types.GetIdentifierType(variableToReadInto);
            if (!varType.Equals(MiniPLType.Integer) && !varType.Equals(MiniPLType.String))
            {
                throw new InvalidTypeException(varType, MiniPLType.Integer, MiniPLType.String);
            }
        }

        public void Execute(Variables Scope)
        {
            RuntimeVariable var = Scope.GetValue(variableToReadInto);
            MiniPLType varType = var.Type;
            string input = Console.ReadLine() ?? string.Empty;
            if (varType.Equals(MiniPLType.Integer))
            {
                try
                {
                    int value = int.Parse(input);
                    var.Value = value;
                }
                catch (OverflowException)
                {
                    throw new IntegerParseOverflowException(input);
                }
                catch (FormatException)
                {
                    throw new IntegerFormatException(input);
                }
            }
            else if (varType.Equals(MiniPLType.String))
            {
                var.Value = input;
            }
        }
    }
}
