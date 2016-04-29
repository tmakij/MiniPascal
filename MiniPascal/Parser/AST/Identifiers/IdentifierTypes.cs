using System.Collections.Generic;

namespace MiniPL.Parser.AST
{
    public sealed class IdentifierTypes
    {
        private readonly Dictionary<Identifier, MiniPLType> types = new Dictionary<Identifier, MiniPLType>();

        public void SetIdentifierType(Identifier Identifier, MiniPLType Type)
        {
            types.Add(Identifier, Type);
        }

        public MiniPLType GetIdentifierType(Identifier Identifier)
        {
            return types[Identifier];
        }
    }
}
