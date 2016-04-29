using System.Collections.Generic;

namespace MiniPL.Parser.AST
{
    public sealed class Variables
    {
        private readonly Dictionary<Identifier, RuntimeVariable> values = new Dictionary<Identifier, RuntimeVariable>();

        public void AddIdentifier(Identifier Identifier, MiniPLType Type)
        {
            values.Add(Identifier, new RuntimeVariable(Type));
        }

        public RuntimeVariable GetValue(Identifier Identifier)
        {
            return values[Identifier];
        }
    }
}
